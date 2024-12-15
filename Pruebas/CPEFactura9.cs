// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;

namespace Pruebas
{
    internal class CPEFactura9
    {
        /// <summary>
        /// FACTURA CON PERCEPCION
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

            //Como ejemplo un unico de datalle de S / 100 + IGV
            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "00001",
                    nombre = "COMBUSTIBLE",
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

            var _percepcion = new PercepcionType()
            {
                tasa = 0.03m,
                montoBase = 1180,
                importe = 35.4m,
                codigo = "52", //Catalogo N° 53 solo se permite '51', '52', '53'
                importeTotalEnSolesConPercepcion = 1215.4m
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "2001",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                ordenCompra = "000055",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",//Catalogo N° 01
                serie = "F001",
                numero = 9,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 1000,
                sumatoriaIGV = 180,
                sumatoriaImpuestos = 180,
                valorVenta = 1000,
                precioVenta = 1180,
                percepcion = _percepcion,
                importeTotal = 1180,
            };

            return _cpe;
        }
    }
}
