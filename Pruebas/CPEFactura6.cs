// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    internal class CPEFactura6
    {
        /// <summary>
        /// FACTURA CON ANTICIPOS - CON MONTO PENDIENTE DE PAGO
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
                    nombre = "PRODUCTO DE PRUEBA",
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

            //En este ejemplo anteriormente al cliente le emití 2 facturas por 118 soles cada una a manera de anticipos
            var _anticipos = new List<AnticipoCPEType>()
            {
                //esta factura se la emiti hace 30 dias
                new AnticipoCPEType()
                {
                    tipoDocumento = "02",//Catalogo N° 12 SUNAT Solo se permite "02" o "03" sino dara error 2505
                    serie = "F001",
                    numero = 1,
                    tipoDocumentoIdentificacionEmisor = "6",
                    numeroDocumentoIdentificacionEmisor = "20606433094",
                    importeTotal = 118,
                    totalOperacionesGravadas = 100,
                    totalOperacionesExoneradas = 0,
                    totalOperacionesInafectas = 0,
                    fechaPago = DateTime.Now.Subtract(new TimeSpan(30,0,0,0))
                },
                //esta factura se la emiti hace 15 dias
                new AnticipoCPEType()
                {
                    tipoDocumento = "02",//Catalogo N° 12 SUNAT Solo se permite "02" o "03" sino dara error 2505
                    serie = "F001",
                    numero = 2,
                    tipoDocumentoIdentificacionEmisor = "6",
                    numeroDocumentoIdentificacionEmisor = "20606433094",
                    importeTotal = 118,
                    totalOperacionesGravadas = 100,
                    totalOperacionesExoneradas = 0,
                    totalOperacionesInafectas = 0,
                    fechaPago = DateTime.Now.Subtract(new TimeSpan(15,0,0,0))
                },
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
                numero = 6,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 800,//la suma del valor venta de todos los items grabados - la suma de totalOperacionesGravadas de todos los anticipos
                sumatoriaIGV = 144,//Cuando hay anticipos este valor se recalcula en base a las totalOperacionesGravadas * 0.18
                sumatoriaImpuestos = 144,//en este caso solo hay IGV pero si hubiera ICBPER tambien se suma aqui
                valorVenta = 1000,//Sumatoria de la propiedad valorVenta de cada item - DescuentoGlobalesAfectaBI(codigo 02 del catalogo 53) + cargosGlobalesAfectaBI(codigo 49 catalogo 53)
                precioVenta = 1180,//Valor de venta total de la operación incluido los impuestos.(importeTotal - sumatoriaOtrosCargos + totalDescuentos + totalAnticipos - totalRedondeo)
                anticipos = _anticipos,
                totalAnticipos = 236m,//sumatoria del importeTotal de cada anticipo
                importeTotal = 944,//precioVenta- total antipos
            };

            return _cpe;
        }
    }
}
