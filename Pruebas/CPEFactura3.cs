// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;

namespace Pruebas
{
    internal class CPEFactura3
    {
        /// <summary>
        /// Factura al contado con detraccion del 12%
        /// </summary>
        public static CPEType GetDocumento()
        {
            //Informacion del Receptor
            var _adquirente = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20415932376",
                nombre = "COCA-COLA SERVICIOS DE PERU S.A",
                direccion = "AV. REPÚBLICA DE PANAMÁ NRO. 4050 URB. LIMATAMBO"
            };

            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "00001",
                    nombre = "SERVICIO DESARROLLO SOFTWARE",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario=1000,
                    precioVentaUnitario = 1180,
                    valorVenta = 1000,
                    montoBaseIGV = 1000,
                    montoIGV = 180,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 180
                }
            };

            //Forma de pago al contado
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Contado
            };

            //detraccion
            var _detraccion = new SPOTType()
            {
                numeroCuentaBancoNacion = "00-045-091619",
                codigoBienServicio = "022",
                porcentaje = 12,
                codMoneda = "PEN",
                importe = 123,
                metodoPago = "001"
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "1001",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                ordenCompra = "000055",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",//Catalogo N° 01
                serie = "F001",
                numero = 3,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 1000,
                sumatoriaIGV = 180,
                sumatoriaImpuestos = 180,
                valorVenta = 1000,
                precioVenta = 1180,
                importeTotal = 1180,
                detraccion = _detraccion
            };

            return _cpe;
        }
    }
}
