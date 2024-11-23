// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT;
using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using GasperSoft.SUNAT.DTO.GRE;
using GasperSoft.SUNAT.UBL.V2;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace Pruebas
{
    internal class Program
    {
        static ConsoleColor _colorOriginal;

        static void Main(string[] args)
        {
            //En algunas partes del ejemplo cambio el color de las letras y necesito saber el color original
            _colorOriginal = Console.ForegroundColor;

            //Informacion del Emisor
            var _emisor = new EmisorType()
            {
                ruc = "20606433094",
                razonSocial = "GASPERSOFT EIRL",
                codigoUbigeo = "150125",
                direccion = "CAL. LOS LIRIOS INT. 20 LT. 1 MZ. B URB. MONTEGRANDE",
                departamento = "LIMA",
                provincia = "LIMA",
                distrito = "PUENTE PIEDRA"
            };

            //Leer el certificado(Estoy usando uno generado de manera gratuita en https://llama.pe/certificado-digital-de-prueba-sunat)
            var _bytesCertificado = File.ReadAllBytes("20606433094.pfx");
            var _certificado = new X509Certificate2(_bytesCertificado, "1234567890", X509KeyStorageFlags.MachineKeySet);

            //El valor de esta variable se refleja en el tag <cac:Signature><cbc:ID> en el XML
            var _signature = "signatureMIEMPRESA";

            var salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Seleccione Opción:");
                Console.WriteLine("==================");
                Console.WriteLine("1: Ejemplo XML FACTURA SIMPLE");
                Console.WriteLine("2: Ejemplo XML FACTURA GRATUITA");
                Console.WriteLine("3: Ejemplo XML NOTA CREDITO MOTIVO 13");
                Console.WriteLine("4: Ejemplo XML GUIA REMISION REMITENTE (Transporte Publico)");
                Console.WriteLine("S: Salir");

                var _input = Console.ReadLine();

                Console.Clear();

                switch (_input)
                {
                    case "1":
                        EjemploCPE(CPE1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "2":
                        EjemploCPE(CPE2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "3":
                        EjemploCPE(CPE3.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "4":
                        var _gre = GRERemitente1.GetDocumento(_emisor);
                        EjemploGRE(_gre, _emisor, _certificado, _signature);
                        break;
                    case "S":
                    case "s":
                        salir = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no valida presione una tecla para continuar.");
                        Console.ReadKey();
                        Console.ForegroundColor = _colorOriginal;
                        break;
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Bye");
            Console.ForegroundColor = _colorOriginal;
        }

        private static void EjemploGRE(GREType gre, EmisorType emisor, X509Certificate2 certificado, string signature)
        {
            //Iniciamos la Validacion 
            var _validador = new ValidadorGRE(gre);
            _validador.OnValidarCatalogoSunat += ValidarCatalogoSunat;
            var _validado = _validador.Validar();

            //Para este ejemplo quiero ignorar si existen abvertencias(las que contienen codigo con V4xxxx o S4xxxx)
            if (_validador.Errors.Count > 0)
            {
                var _existeSoloAdvertencias = true;

                foreach (var item in _validador.Errors)
                {
                    if (!(item.codigo.StartsWith("V4") || item.codigo.StartsWith("S4")))
                    {
                        _existeSoloAdvertencias = false;
                    }
                }

                _validado = _existeSoloAdvertencias;
            }

            if (_validado)
            {
                //aqui se almacena el digestValue del xml firmado
                string _digestValue;
                //Generamos el XML
                var _xml = GetXML(gre, emisor, certificado, out _digestValue, signature);
                //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
                var _nombreXml = $"{emisor.ruc}-{gre.tipoGuia}-{gre.serie}-{gre.numero}.xml";
                GuardarXml(_nombreXml, _xml, _digestValue);
            }
            else
            {
                MostrarErrores(_validador.Errors);
            }
        }

        private static void EjemploCPE(CPEType cpe, EmisorType emisor, X509Certificate2 certificado, string signature)
        {
            //Iniciamos la Validacion 
            var _validador = new ValidadorCPE(cpe);
            _validador.OnValidarCatalogoSunat += ValidarCatalogoSunat;
            var _validado = _validador.Validar();

            if (_validado)
            {
                //aqui se almacena el digestValue del xml firmado
                string _digestValue;
                //Generamos el XML
                var _xml = GetXML(cpe, emisor, certificado, out _digestValue, signature);
                //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
                var _nombreXml = $"{emisor.ruc}-{cpe.tipoDocumento}-{cpe.serie}-{cpe.numero}.xml";
                GuardarXml(_nombreXml, _xml, _digestValue);
            }
            else
            {
                MostrarErrores(_validador.Errors);
            }
        }

        static void GuardarXml(string nombre, string xml, string digestValue)
        {
            File.WriteAllText(nombre, xml);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Archivo guardado correctamente");
            Console.WriteLine("==============================");
            Console.ForegroundColor = _colorOriginal;

            Console.WriteLine($"Nombre archivo: {nombre}");
            Console.WriteLine($"DigestValue: {digestValue}");
            Console.WriteLine($"Directorio: {AppDomain.CurrentDomain.BaseDirectory}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------------------------");
            Console.ForegroundColor = _colorOriginal;
            Console.WriteLine("\nPresione una tecla para continuar.");
            Console.ReadKey();
        }

        static string GetXML(CPEType cpe, EmisorType emisor, X509Certificate2 certificado, out string digestValue, string signature)
        {
            object _cpeType;

            switch (cpe.tipoDocumento)
            {
                case "01":
                case "03":
                    _cpeType = FacturaBoleta.GetDocumento(cpe, emisor, signature);
                    break;
                case "07":
                    _cpeType = NotaCredito.GetDocumento(cpe, emisor, signature);
                    break;
                case "08":
                    _cpeType = NotaDebito.GetDocumento(cpe, emisor, signature);
                    break;
                default:
                    throw new Exception("Tipo de documento no valido");
            }

            var _xml = XmlUtil.Serializar(_cpeType);

            //Firmar el XML y obtener el digestValue
            var _xmlFirmado = XmlUtil.FirmarXml(_xml, certificado, out digestValue, signature);

            return _xmlFirmado;
        }

        static string GetXML(GREType gre, EmisorType emisor, X509Certificate2 certificado, out string digestValue, string signature)
        {
            var _cpeType = GuiaRemision.GetDocumento(gre, emisor, signature);

            var _xml = XmlUtil.Serializar(_cpeType);

            //Firmar el XML y obtener el digestValue
            var _xmlFirmado = XmlUtil.FirmarXml(_xml, certificado, out digestValue, signature);

            return _xmlFirmado;
        }

        static void MostrarErrores(List<Error> errors)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Se encontró los siguientes errores al validar el documento");
            Console.WriteLine("==========================================================");
            Console.ForegroundColor = _colorOriginal;

            foreach (var _error in errors)
            {
                Console.WriteLine($"Codigo: {_error.codigo}");
                Console.WriteLine($"Mensaje: {_error.detalle}");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("-------------------------");
                Console.ForegroundColor = _colorOriginal;
            }

            Console.WriteLine("\nPresione una tecla para continuar.");
            Console.ReadKey();
        }

        static bool ValidarCatalogoSunat(string catalogo, string valor)
        {
            //Aqui se deberia validar que un determinado 'valor' existe en un determinado catalogo
            //Los catalogos los encontramos en el Anexo 8 de SUNAT
            //Para este ejemplo en donde coloco datos correctos devuelvo siempre verdadero
            return true;
        }
    }
}
