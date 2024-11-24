// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GasperSoft.SUNAT
{
    public class XmlUtil
    {
        public static Encoding GlobalEncoding => Encoding.GetEncoding("ISO-8859-1");

        internal static XmlWriterSettings GetXmlWriterSettings()
        {
            var _xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true,
                NewLineOnAttributes = false,
                Encoding = GlobalEncoding
            };

            return _xmlWriterSettings;
        }

        public static string Serializar(object documento)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(
                new[]
                {
                    new XmlQualifiedName("cbc","urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2"),
                    new XmlQualifiedName("cac","urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2"),
                    new XmlQualifiedName("ext","urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2"),
                    new XmlQualifiedName("sac","urn:sunat:names:specification:ubl:peru:schema:xsd:SunatAggregateComponents-1"),
                });

            XmlSerializer serializer = new XmlSerializer(documento.GetType());
            string xml = string.Empty;

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, GetXmlWriterSettings()))
                {
                    serializer.SerializeWithDecimalFormatting(xmlWriter, documento, ns);
                }

                xml = GlobalEncoding.GetString(stream.ToArray());
            }
            return xml;
        }

        public static string FirmarXml(string xml, X509Certificate2 certificado, out string digestValue, string signature = null)
        {
            if (string.IsNullOrEmpty(signature))
            {
                signature = "signatureGASPERSOFT";
            }

            var xmlDoc = new XmlDocument();
            digestValue = string.Empty;

            using (var documento = new MemoryStream(GlobalEncoding.GetBytes(xml)))
            {
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(documento);

                //Agregamos un nuevo nodo donde colocar la firma digital
                XmlNode _extensionContent = xmlDoc.CreateNode(XmlNodeType.Element, "ext", "ExtensionContent", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                var _UBLExtension = xmlDoc.GetElementsByTagName("UBLExtension", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                _UBLExtension.Item(0)?.AppendChild(_extensionContent);

                // Creamos el objeto SignedXml.
                var signedXml = new SignedXml(xmlDoc) { SigningKey = certificado.GetRSAPrivateKey() };

                var xmlSignature = signedXml.Signature;

                var env = new XmlDsigEnvelopedSignatureTransform();

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

                using (var stream = new MemoryStream())
                {
                    using (var writer = XmlWriter.Create(stream, GetXmlWriterSettings()))
                    {
                        xmlDoc.WriteTo(writer);
                    }

                    xml = GlobalEncoding.GetString(stream.ToArray());
                }
            }

            return xml;
        }
    }
}
