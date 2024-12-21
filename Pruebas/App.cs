// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT;
using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using GasperSoft.SUNAT.DTO.CRE;
using GasperSoft.SUNAT.DTO.GRE;
using GasperSoft.SUNAT.DTO.Resumen;
using GasperSoft.SUNAT.UBL.V1;
using GasperSoft.SUNAT.UBL.V2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Pruebas
{
    internal class App
    {
        public static string GetPathStorage => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
        public static EmisorType GetEmisior => new EmisorType()
        {
            ruc = "20606433094",
            razonSocial = "GASPERSOFT EIRL",
            codigoUbigeo = "150125",
            direccion = "CAL. LOS LIRIOS INT. 20 LT. 1 MZ. B URB. MONTEGRANDE",
            departamento = "LIMA",
            provincia = "LIMA",
            distrito = "PUENTE PIEDRA"
        };

        static void Main(string[] args)
        {
            //Si no existe el Directorio Storage lo creamos
            if (!Directory.Exists(GetPathStorage))
            {
                Directory.CreateDirectory(GetPathStorage);
            }

            //Informacion del Emisor
            var _emisor = GetEmisior;

            //Leer el certificado(Estoy usando uno generado de manera gratuita en https://llama.pe/certificado-digital-de-prueba-sunat)
#if NET9_0
            var _certificado = X509CertificateLoader.LoadPkcs12FromFile("20606433094.pfx", "1234567890");
#else
            var _bytesCertificado = File.ReadAllBytes("20606433094.pfx");
            var _certificado = new X509Certificate2(_bytesCertificado, "1234567890", X509KeyStorageFlags.MachineKeySet);
#endif


            //El valor de esta variable se refleja en el tag <cac:Signature><cbc:ID> en el XML
            var _signature = "signatureMIEMPRESA";

            var salir = false;

            while (!salir)
            {
                ConsoleEx.Clear();
                ConsoleEx.WriteLine("Seleccione Ejemplo:");
                ConsoleEx.WriteLine("==================");
                ConsoleEx.WriteLine("0: BOLETA DE VENTA GRAVADA CON DOS ÍTEMS Y UNA BONIFICACIÓN - Pagina 60 Manual SUNAT");
                ConsoleEx.WriteLine("1: BOLETA CON ICBPER - COBRANDO BOLSA");
                ConsoleEx.WriteLine("2: BOLETA CON ICBPER - REGALANDO BOLSA");
                ConsoleEx.WriteLine("3: FACTURA CREDITO (CUOTAS)");
                ConsoleEx.WriteLine("4: FACTURA GRATUITA - Pagina 98 Manual SUNAT");
                ConsoleEx.WriteLine("5: FACTURA CONTADO CON DETRACCION");
                ConsoleEx.WriteLine("6: FACTURA CON 4 ÍTEMS Y UNA BONIFICACIÓN - Pagina 77 Manual SUNAT");
                ConsoleEx.WriteLine("7: FACTURA CON 2 ÍTEMS E ISC - Pagina 88 Manual SUNAT");
                ConsoleEx.WriteLine("8: FACTURA CON ANTICIPOS - CON MONTO PENDIENTE DE PAGO");
                ConsoleEx.WriteLine("9: FACTURA CON ANTICIPOS - MONTO TOTAL EN CERO");
                ConsoleEx.WriteLine("10: FACTURA CON RETENCION");
                ConsoleEx.WriteLine("11: FACTURA CON PERCEPCION");
                ConsoleEx.WriteLine("12: NOTA CREDITO MOTIVO 13");
                ConsoleEx.WriteLine("13: GUIA REMISION REMITENTE - Transporte Publico");
                ConsoleEx.WriteLine("14: GUIA REMISION REMITENTE - Transporte Privado (Vehiculo y Conductor)");
                ConsoleEx.WriteLine("15: GUIA REMISION REMITENTE - Transporte Privado (M1 o L)");
                ConsoleEx.WriteLine("16: GUIA REMISION TRANSPORTISTA");
                ConsoleEx.WriteLine("17: RESUMEN DIARIO DE BOLETAS - INFORMAR");
                ConsoleEx.WriteLine("18: RESUMEN DIARIO DE BOLETAS - DAR DE BAJA");
                ConsoleEx.WriteLine("19: COMUNICACION DE BAJA (SOLO FACTURAS)");
                ConsoleEx.WriteLine("20: RETENCION FACTURA SOLES");
                ConsoleEx.WriteLine("21: RETENCION FACTURA DOLARES - CON TIPO DE CAMBIO");
                ConsoleEx.WriteLine("22: REVERSION (BAJAS DE RETENCIONES)");
                ConsoleEx.WriteLine("23: GUIA REMITENTE CON INFORMACION ADICIONAL EN 'UBLExtension'", ConsoleColor.Blue);
                ConsoleEx.WriteLine("24: FACTURA CON INFORMACION ADICIONAL EN 'UBLExtension'", ConsoleColor.Blue);
                ConsoleEx.WriteLine("25: GUIA REMISION REMITENTE - EXPORTACION (PENDIENTE DE VERIFICACION CON SUNAT)");
                ConsoleEx.WriteLine("26: FACTURA AL CONTADO PAGO CON DEPOSITO EN CUENTA (MEDIO DE PAGO CATALOGO N° 59)");

#if NET462_OR_GREATER || NET6_0_OR_GREATER
                //Pruebas de envio()
                ConsoleEx.WriteLine("P1: ENVIAR GUIA REMITENTE",ConsoleColor.Green);
#endif

                ConsoleEx.Write("\nX", ConsoleColor.Red);
                ConsoleEx.WriteLine(": Salir");

                var _input = (ConsoleEx.ReadLine() ?? "").ToUpper();

                ConsoleEx.Clear();

                switch (_input)
                {
                    case "0":
                        EjemploCPE(CPEBoleta1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "1":
                        EjemploCPE(CPEBoleta2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "2":
                        EjemploCPE(CPEBoleta3.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "3":
                        EjemploCPE(CPEFactura1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "4":
                        EjemploCPE(CPEFactura2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "5":
                        EjemploCPE(CPEFactura3.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "6":
                        EjemploCPE(CPEFactura4.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "7":
                        EjemploCPE(CPEFactura5.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "8":
                        EjemploCPE(CPEFactura6.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "9":
                        EjemploCPE(CPEFactura7.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "10":
                        EjemploCPE(CPEFactura8.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "11":
                        EjemploCPE(CPEFactura9.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "12":
                        EjemploCPE(CPENotaCredito1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "13":
                        EjemploGRE(GRERemitente1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "14":
                        EjemploGRE(GRERemitente2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "15":
                        EjemploGRE(GRERemitente3.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "16":
                        EjemploGRE(GRETransportista1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "17":
                        EjemploResumenDiario(ResumenDiario1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "18":
                        EjemploResumenDiario(ResumenDiario2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "19":
                        EjemploComunicacionBaja(ComunicacionBaja1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "20":
                        EjemploCRE(CRE1.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "21":
                        EjemploCRE(CRE2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "22":
                        EjemploComunicacionBaja(ComunicacionBaja2.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "23":
                        EjemploGRE(GRERemitente4.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "24":
                        EjemploCPE(CPEFactura10.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "25":
                        EjemploGRE(GRERemitente5.GetDocumento(), _emisor, _certificado, _signature);
                        break;
                    case "26":
                        EjemploCPE(CPEFactura11.GetDocumento(), _emisor, _certificado, _signature);
                        break;

#if NET462_OR_GREATER || NET6_0_OR_GREATER

                    case "P1":
                        EnvioGRE1.Run();
                        break;
#endif

                    case "X":
                        salir = true;
                        break;
                    default:
                        ConsoleEx.WriteLine("Opción no valida presione una tecla para continuar.", ConsoleColor.Red);
                        ConsoleEx.ReadKey();
                        break;
                }
            }

            ConsoleEx.WriteLine("Bye", ConsoleColor.Green);
        }

        private static void EjemploComunicacionBaja(ComunicacionBajaType baja, EmisorType emisor, X509Certificate2 certificado, string signature)
        {
            //aqui se almacena el digestValue del xml firmado
            string _digestValue;
            //Generamos el XML
            var _xml = GetXML(baja, emisor, certificado, out _digestValue, signature);
            //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
            var _nombreArchivo = $"{emisor.ruc}-{baja.id}";
            GuardarXml(_xml, _nombreArchivo, _digestValue);
        }

        private static void EjemploResumenDiario(ResumenDiarioV2Type resumen, EmisorType emisor, X509Certificate2 certificado, string signature)
        {
            //aqui se almacena el digestValue del xml firmado
            string _digestValue;
            //Generamos el XML
            var _xml = GetXML(resumen, emisor, certificado, out _digestValue, signature);
            //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
            var _nombreArchivo = $"{emisor.ruc}-{resumen.id}";
            GuardarXml(_xml, _nombreArchivo, _digestValue);
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
                var _nombreArchivo = $"{emisor.ruc}-{gre.tipoGuia}-{gre.serie}-{gre.numero}";
                GuardarXml(_xml, _nombreArchivo, _digestValue);
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
                var _nombreArchivo = $"{emisor.ruc}-{cpe.tipoDocumento}-{cpe.serie}-{cpe.numero}";
                GuardarXml(_xml, _nombreArchivo, _digestValue);
            }
            else
            {
                MostrarErrores(_validador.Errors);
            }
        }

        private static void EjemploCRE(CREType cre, EmisorType emisor, X509Certificate2 certificado, string signature)
        {
            //Iniciamos la Validacion 
            var _validador = new ValidadorCRE(cre);

            //Opcional el evento de validacion
            _validador.OnValidarTasaRetencion += ValidarTasaRetencion;
            var _validado = _validador.Validar();

            if (_validado)
            {
                //aqui se almacena el digestValue del xml firmado
                string _digestValue;
                //Generamos el XML
                var _xml = GetXML(cre, emisor, certificado, out _digestValue, signature);
                //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
                var _nombreArchivo = $"{emisor.ruc}-20-{cre.serie}-{cre.numero}";
                GuardarXml(_xml, _nombreArchivo, _digestValue);
            }
            else
            {
                MostrarErrores(_validador.Errors);
            }
        }

        static void GuardarXml(string xml, string nombreArchivo, string digestValue)
        {
            var _pathArchivoZipXml = Path.Combine(GetPathStorage, $"{nombreArchivo}.zip");

            //Importante guardar el XML como zip para no perder el encoding del XML y un posible error 2335(XMl adulterado)
            var _bytesZip = XmlUtil.Comprimir(xml, $"{nombreArchivo}.xml");

            File.WriteAllBytes(_pathArchivoZipXml, _bytesZip);

            ConsoleEx.WriteLine("Archivo guardado correctamente", ConsoleColor.Blue);
            ConsoleEx.WriteLine("==============================", ConsoleColor.Blue);
            ConsoleEx.WriteLine($"Nombre archivo: {nombreArchivo}.zip");
            ConsoleEx.WriteLine($"DigestValue: {digestValue}");
            ConsoleEx.WriteLine($"Directorio: {GetPathStorage}");
            ConsoleEx.WriteLine("-------------------------", ConsoleColor.Green);
            ConsoleEx.WriteLine("\nPresione una tecla para continuar.");
            ConsoleEx.ReadKey();
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

        static string GetXML(CREType cre, EmisorType emisor, X509Certificate2 certificado, out string digestValue, string signature)
        {
            var _cpeType = Retencion.GetDocumento(cre, emisor, signature);
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

        static string GetXML(ResumenDiarioV2Type resumen, EmisorType emisor, X509Certificate2 certificado, out string digestValue, string signature)
        {
            var _resumenType = ResumenDiarioV2.GetDocumento(resumen, emisor, signature);

            var _xml = XmlUtil.Serializar(_resumenType);

            //Firmar el XML y obtener el digestValue
            var _xmlFirmado = XmlUtil.FirmarXml(_xml, certificado, out digestValue, signature);

            return _xmlFirmado;
        }

        static string GetXML(ComunicacionBajaType baja, EmisorType emisor, X509Certificate2 certificado, out string digestValue, string signature)
        {
            var _resumenType = ComunicacionBaja.GetDocumento(baja, emisor, signature);

            var _xml = XmlUtil.Serializar(_resumenType);

            //Firmar el XML y obtener el digestValue
            var _xmlFirmado = XmlUtil.FirmarXml(_xml, certificado, out digestValue, signature);

            return _xmlFirmado;
        }

        static void MostrarErrores(List<Error> errors)
        {
            ConsoleEx.WriteLine("Se encontró los siguientes errores al validar el documento", ConsoleColor.Red);
            ConsoleEx.WriteLine("==========================================================");

            foreach (var _error in errors)
            {
                ConsoleEx.WriteLine($"Codigo: {_error.codigo}");
                ConsoleEx.WriteLine($"Mensaje: {_error.detalle}");
                ConsoleEx.WriteLine("-------------------------", ConsoleColor.Green);
            }

            ConsoleEx.WriteLine("\nPresione una tecla para continuar.");
            ConsoleEx.ReadKey();
        }

        static bool ValidarCatalogoSunat(string catalogo, string valor)
        {
            //Aqui se deberia validar que un determinado 'valor' existe en un determinado catalogo
            //Los catalogos los encontramos en el Anexo 8 de SUNAT
            //Para este ejemplo en donde coloco datos correctos devuelvo siempre verdadero
            return true;
        }

        private static bool ValidarTasaRetencion(string codigoRegimenRetencion, decimal tasa)
        {
            //Aquí podría colocar código para validar que exista el codigoRegimenRetencion en el catálogo 23 y que la tasa corresponda a ese código
            return true;
        }
    }
}
