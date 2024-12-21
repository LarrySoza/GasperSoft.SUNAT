// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.CPE;
using GasperSoft.SUNAT.DTO.GRE;
using GasperSoft.SUNAT.UBL.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

#if NET35 || NET40
using Ionic.Zip;
#endif

namespace GasperSoft.SUNAT
{
    /// <summary>
    /// Contiene metodos para Generar,Firmar y Comprimir el XML
    /// </summary>
    public static class XmlUtil
    {
        /// <summary>
        /// Encoding predeterminado por SUNAT("ISO-8859-1")
        /// </summary>
        public static Encoding SunatEncoding => Encoding.GetEncoding("ISO-8859-1");

        private static XmlWriterSettings GetXmlWriterSettings(Encoding encoding)
        {
            var _xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true,
                NewLineOnAttributes = false,
                Encoding = encoding
            };

            return _xmlWriterSettings;
        }

        private static Encoding GetXmlEncoding(string xmlString)
        {
            Regex regex = new Regex(@"encoding=""([^""]+)""");

            Match match = regex.Match(xmlString);

            if (match.Success)
            {
                var _encodingName = match.Groups[1].Value;

                return Encoding.GetEncoding(_encodingName);
            }

            throw new Exception("Error al leer el encoding del xml");
        }

        /// <summary>
        /// Convierte un objeto en un documento XML
        /// </summary>
        /// <param name="documento">InvoiceType, DespatchAdviceType, DebitNoteType, CreditNoteType, RetentionType, VoidedDocumentsType o SummaryDocumentsType</param>
        /// <param name="encoding">Encoding personalizado, por defecto se usa el predeterminado por SUNAT, tomar en cuenta que se debe usar un mismo encoding para serializar, firmar y comprimir, para evitar un posible error 2335 generado por la codificación de caracteres especiales</param>
        /// <returns>XML sin firmar</returns>
        public static string Serializar(object documento, Encoding encoding = null)
        {
            if (encoding == null) encoding = SunatEncoding;
            bool _informacionAdicionalEnXml = false;

            if (documento is InvoiceType)
            {
                _informacionAdicionalEnXml = (documento as InvoiceType).UBLExtensions?[0]?.ExtensionContent != null;
            }

            if (documento is DespatchAdviceType)
            {
                _informacionAdicionalEnXml = (documento as DespatchAdviceType).UBLExtensions?[0]?.ExtensionContent != null;
            }

            if (documento is DebitNoteType)
            {
                _informacionAdicionalEnXml = (documento as DebitNoteType).UBLExtensions?[0]?.ExtensionContent != null;
            }

            if (documento is CreditNoteType)
            {
                _informacionAdicionalEnXml = (documento as CreditNoteType).UBLExtensions?[0]?.ExtensionContent != null;
            }

            if (documento is GasperSoft.SUNAT.UBL.V1.RetentionType)
            {
                _informacionAdicionalEnXml = (documento as GasperSoft.SUNAT.UBL.V1.RetentionType).UBLExtensions?[0]?.ExtensionContent != null;
            }

            var _nss = new List<XmlQualifiedName>()
            {
                new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"),
                new XmlQualifiedName("sac","urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1")
            };

            if (_informacionAdicionalEnXml)
            {
                _nss.Add(new XmlQualifiedName("cacadd", "urn:e-billing:aggregates"));
                _nss.Add(new XmlQualifiedName("cbcadd", "urn:e-billing:basics"));
            }

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(_nss.ToArray());

            XmlSerializer serializer = new XmlSerializer(documento.GetType());

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, GetXmlWriterSettings(encoding)))
                {
                    serializer.SerializeWithDecimalFormatting(xmlWriter, documento, ns);
                }

                var _xml = encoding.GetString(stream.ToArray());

                //Esto es por compatibilidad con un Proveedor de Firma que requiere que el XML contenga si o si un "<ext:ExtensionContent/>"
                //donde colocar la firma
                _xml = _xml.Replace("<ext:UBLExtension />", "<ext:UBLExtension>\r\n      <ext:ExtensionContent/>\r\n    </ext:UBLExtension>");

                return _xml;
            }
        }

        /// <summary>
        /// Firma un documento xml
        /// </summary>
        /// <param name="xml">La cadena XML a firmar, dbe contener un ExtensionContent vacio</param>
        /// <param name="certificado">El certificado a usar</param>
        /// <param name="digestValue">Variable donde se almacenara hash calculado del XML</param>
        /// <param name="signature">Una cadena de texto que se usa para "Signature ID" del XML, por defecto se usará la cadena predeterminada "signatureGASPERSOFT"</param>
        /// <param name="encoding">Encoding personalizado, por defecto se usa el predeterminado por SUNAT, tomar en cuenta que se debe usar un mismo encoding para serializar, firmar y comprimir, para evitar un posible error 2335 generado por la codificación de caracteres especiales</param>
        /// <returns>XML firmado</returns>
        public static string FirmarXml(string xml, X509Certificate2 certificado, out string digestValue, string signature = null, Encoding encoding = null)
        {
            if (Validaciones.IsNullOrWhiteSpace(xml)) throw new ArgumentNullException(nameof(xml));

            if (certificado == null) throw new ArgumentNullException(nameof(certificado));

#if NET462_OR_GREATER || NET6_0_OR_GREATER
            var _rsaKey = certificado.GetRSAPrivateKey() ?? throw new ArgumentException("Error al leer la clave privada del certificado", nameof(certificado));
#else
            var _rsaKey = (RSA)certificado.PrivateKey;
#endif

            //Si no se manda el encoding intentamos leer el que se especifica en la primera linea del XML
            //ejemplo: <?xml version="1.0" encoding="iso-8859-1"?> deberia devolver el encoding iso-8859-1
            //si no se encuentra entonces genera un error
            if (encoding == null)
            {
                encoding = GetXmlEncoding(xml);
            }

            //Se podria leer el <cac:Signature><cbc:ID> del XML (posiblemente en versiones futuras)
            if (Validaciones.IsNullOrWhiteSpace(signature)) signature = "signatureGASPERSOFT";

            var _xmlDoc = new XmlDocument();
            digestValue = string.Empty;

            var _bytesXml = encoding.GetBytes(xml);

            #region Quitar BOM si existe

            /* 13/12/2024
             * Dependiendo del encoding que se uso para generar el XML se podrían haber agregado un BOM (byte-order mark)
             * que son una codificación de bytes al inicio del archivo que especifica que codificacion se ha usado y
             * si no se quita el método LoadXml del XmlDocument fallara.
             */
            if (_bytesXml[0] != (byte)0x3C)
            {
                var _inicioXML = Array.IndexOf(_bytesXml, (byte)0x3C);

                if (_inicioXML != -1)
                {
                    var _newBytesXml = new byte[_bytesXml.Length - _inicioXML];

                    for (int i = _inicioXML; i < _bytesXml.Length; i++)
                    {
                        _newBytesXml[i - _inicioXML] = _bytesXml[i];
                    }

                    _bytesXml = _newBytesXml;

                }
                else
                {
                    throw new ArgumentException("Error al procesar el XML", nameof(xml));
                }
            }

            #endregion

            _xmlDoc.PreserveWhitespace = true;
            _xmlDoc.LoadXml(encoding.GetString(_bytesXml));

            //Leer el nodo ExtensionContent
            var _extensionContentNode = _xmlDoc.GetElementsByTagName("ExtensionContent", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

            var _totalExtensionContent = _extensionContentNode.Count;

            if (_totalExtensionContent == 0) throw new Exception("No existe un tag '<ext:ExtensionContent/>' en el XML");

            //Colocar la firma en el ultimo ExtensionContent
            XmlNode _extensionContent = _extensionContentNode.Item(_totalExtensionContent - 1);

            if (!Validaciones.IsNullOrWhiteSpace(_extensionContent.InnerText)) throw new Exception("No existe un tag '<ext:ExtensionContent/>' vacio en el XML");

            // Creamos el objeto SignedXml.
            var signedXml = new SignedXml(_xmlDoc) { SigningKey = _rsaKey };

            var xmlSignature = signedXml.Signature;

            var env = new XmlDsigEnvelopedSignatureTransform();
            //var env = new XmlDsigC14NTransform();

            var reference = new Reference(string.Empty);
            reference.AddTransform(env);
            xmlSignature.SignedInfo?.AddReference(reference);

            var keyInfo = new KeyInfo();
            var x509Data = new KeyInfoX509Data(certificado);

            x509Data.AddSubjectName(certificado.Subject);

            keyInfo.AddClause(x509Data);
            xmlSignature.KeyInfo = keyInfo;
            xmlSignature.Id = signature;

            signedXml.ComputeSignature();

            // Recuperamos el valor Hash de la firma para este documento.
            if (reference.DigestValue != null)
                digestValue = Convert.ToBase64String(reference.DigestValue);

            //nodoExtension.AppendChild(signedXml.GetXml());
            _extensionContent.AppendChild(signedXml.GetXml());

            //Solucionar problema de XML Adulterado
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, GetXmlWriterSettings(encoding)))
                {
                    _xmlDoc.WriteTo(writer);
                }

                return encoding.GetString(stream.ToArray());
            }
        }

#if NET35 || NET40

        /// <summary>
        /// Comprimir una cadena XML y devuelve la cadena de bytes del zip
        /// </summary>
        /// <param name="xml">cadena XML</param>
        /// <param name="nombreArchivo">nombre del archivo incluyendo la extrension Ejemplo: 20606433094-01-T001-1.xml</param>
        /// <param name="encoding">Encoding a usar para la codificación del XML</param>
        public static byte[] Comprimir(string xml, string nombreArchivo, Encoding encoding = null)
        {
            if (encoding == null) encoding = SunatEncoding;

            var _dataBytes = encoding.GetBytes(xml);

            using (var memDestino = new MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(nombreArchivo, _dataBytes);

                    zip.Save(memDestino);
                }

                return memDestino.ToArray();
            }
        }

#else
        /// <summary>
        /// Comprimir una cadena XML y devuelve la cadena de bytes del zip
        /// </summary>
        /// <param name="xml">cadena XML</param>
        /// <param name="nombreArchivo">nombre del archivo incluyendo la extrension Ejemplo: 20606433094-01-T001-1.xml</param>
        /// <param name="encoding">Encoding a usar para la codificación del XML</param>
        public static byte[] Comprimir(string xml, string nombreArchivo, Encoding encoding = null)
        {
            if (encoding == null) encoding = SunatEncoding;

            var _dataBytes = encoding.GetBytes(xml);

            using (var memDestino = new MemoryStream())
            {
                using (var fileZip = new ZipArchive(memDestino, ZipArchiveMode.Create))
                {
                    ZipArchiveEntry zipItem = fileZip.CreateEntry(nombreArchivo);

                    using (Stream ZipFile = zipItem.Open())
                    {
                        ZipFile.Write(_dataBytes, 0, _dataBytes.Length);
                    }
                }

                return memDestino.ToArray();
            }
        }
#endif

    }
}
