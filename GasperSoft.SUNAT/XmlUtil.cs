// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace GasperSoft.SUNAT
{
    public class XmlUtil
    {
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

        public static string Serializar(object documento, Encoding encoding = null)
        {
            if (encoding == null) encoding = SunatEncoding;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(
                new[]
                {
                    new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                    new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                    new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"),
                    new XmlQualifiedName("sac","urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1"),
                });

            XmlSerializer serializer = new XmlSerializer(documento.GetType());

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, GetXmlWriterSettings(encoding)))
                {
                    serializer.SerializeWithDecimalFormatting(xmlWriter, documento, ns);
                }

                return encoding.GetString(stream.ToArray());
            }
        }

        public static string FirmarXml(string xml, X509Certificate2 certificado, out string digestValue, string signature = null, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(xml)) throw new ArgumentNullException(nameof(xml));

            if (certificado == null) throw new ArgumentNullException(nameof(certificado));

            var _rsaKey = certificado.GetRSAPrivateKey() ?? throw new ArgumentException("Error al leer la clave privada del certificado", nameof(certificado));

            //Si no se manda el encoding intentamos leer el que se especifica en la primera linea del XML
            //ejemplo: <?xml version="1.0" encoding="iso-8859-1"?> deberia devolver el encoding iso-8859-1
            //si no se encuentra entonces genera un error
            if (encoding == null)
            {
                encoding = GetXmlEncoding(xml);
            }

            if (string.IsNullOrEmpty(signature)) signature = "signatureGASPERSOFT";

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

            //Agregamos un nuevo nodo donde colocar la firma digital
            XmlNode _extensionContent = _xmlDoc.CreateNode(XmlNodeType.Element, "ext", "ExtensionContent", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            var _UBLExtension = _xmlDoc.GetElementsByTagName("UBLExtension", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            _UBLExtension.Item(0)?.AppendChild(_extensionContent);

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

            return _xmlDoc.OuterXml;

            //using (var stream = new MemoryStream())
            //{
            //    using (var writer = XmlWriter.Create(stream, GetXmlWriterSettings(encoding)))
            //    {
            //        _xmlDoc.WriteTo(writer);
            //    }

            //    return encoding.GetString(stream.ToArray());
            //}
        }

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
    }
}
