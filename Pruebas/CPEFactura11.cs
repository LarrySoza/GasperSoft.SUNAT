// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;

namespace Pruebas
{
    internal class CPEFactura11
    {
        /// <summary>
        /// Factura al contado pagado con deposito en cuenta
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
                    nombre = "PRODUCTO DE PRUEBA",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario=100,
                    precioVentaUnitario = 118,
                    valorVenta = 100,
                    montoBaseIGV = 100,
                    montoIGV = 18,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 18
                }
            };

            //Forma de pago al contado
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Contado,
                metodoPago = "001"//Catalogo N° 59
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                ordenCompra = "000055",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",//Catalogo N° 01
                serie = "F001",
                numero = 11,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 100,
                sumatoriaIGV = 18,
                sumatoriaImpuestos = 18,
                valorVenta = 100,
                precioVenta = 118,
                importeTotal = 118,
            };

            return _cpe;
        }
    }
}
