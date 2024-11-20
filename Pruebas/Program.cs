// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using GasperSoft.SUNAT.DTO.Validar;
using GasperSoft.SUNAT.UBL.V2;
using System.Security.Cryptography.X509Certificates;

namespace Pruebas
{
    internal class Program
    {
        static void Main(string[] args)
        {
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

            //Informacion del Receptor
            var _adquirente = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20415932376",
                nombre = "COCA-COLA SERVICIOS DE PERU S.A",
                direccion = "AV. REPÚBLICA DE PANAMÁ NRO. 4050 URB. LIMATAMBO"
            };

            //Como ejemplo un unico de datalle de S / 100 + IGV
            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "00001",
                    nombre = "PRODUCTO DE PRUEBA",
                    unidadMedida = "NIU",
                    cantidad = 1,
                    valorVentaUnitario=100,
                    precioVentaUnitario = 118,
                    valorVenta = 100,
                    montoBaseIGV = 100,
                    montoIGV = 18,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 7
                    sumatoriaImpuestos = 18
                }
            };

            //Forma de pago al contado
            //var _informacionPago = new InformacionPagoType()
            //{
            //    formaPago = FormaPagoType.Contado
            //};

            //Forma de pago al credito en una cuota
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Credito,
                montoPendientePago = 118,
                cuotas = new List<CuotaType>()
                {
                    new CuotaType()
                    {
                        fechaPago = DateTime.Now.AddDays(30).Date,
                        monto = 118
                    }
                }
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",
                codigoEstablecimiento = "0000",
                ordenCompra = "000055",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",
                serie = "F001",
                numero = 2,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",
                totalOperacionesGravadas = 100,
                sumatoriaIGV = 18,
                tasaIGV = 18,
                sumatoriaImpuestos = 18,
                valorVenta = 100,
                precioVenta = 118,
                importeTotal = 118,
            };

            //Iniciamos la Validacion 
            var _validador = new ValidadorCPE(_cpe);
            _validador.OnValidarCatalogoSunat += Validador_OnValidarCatalogoSunat;

            var _validado = _validador.Validar();

            //Si no se valido el comprobante entonces mostramos los errores
            if (!_validado)
            {
                foreach (var _error in _validador.Errors)
                {
                    Console.WriteLine($"Error {_error.codigo} => {_error.detalle}");
                }
            }
            else
            {
                //El valor de esta variable se refleja en el tag <cac:Signature><cbc:ID> en el XML
                var _signature = "signatureMIEMPRESA";

                var _cpeType = FacturaBoleta.GetDocumento(_cpe, _emisor, _signature);

                var _xml = Metodos.Serializar(_cpeType);

                //Leer el certificado(Estoy usando uno generado de manera gratuita en https://llama.pe/certificado-digital-de-prueba-sunat)
                var _bytesCertificado = File.ReadAllBytes("20606433094.pfx");
                var _certificado = new X509Certificate2(_bytesCertificado, "1234567890", X509KeyStorageFlags.MachineKeySet);

                //Firmar el XML y obtener el digestValue
                var _digestValue = string.Empty;
                var _xmlFirmado = Metodos.FirmarXml(_xml, _certificado, out _digestValue, _signature);
                var _nombreXml = $"{_emisor.ruc}-{_cpe.tipoDocumento}-{_cpe.serie}-{_cpe.numero}.xml";

                //Guardamos el XML y luego podemos validarlo en https://probar-xml.nubefact.com/
                File.WriteAllText(_nombreXml, _xmlFirmado);
                Console.WriteLine($"Archivo {_nombreXml} generado con digestValue {_digestValue}");
            }

            Console.WriteLine("Operación terminada presione una tecla para continuar...");
            Console.ReadKey();
        }

        private static bool Validador_OnValidarCatalogoSunat(string catalogo, string valor)
        {
            //Aqui se deberia validar que un determinado 'valor' existe en un determinado catalogo
            //Los catalogos los encontramos en el Anexo 8 de SUNAT
            return true;
        }
    }
}
