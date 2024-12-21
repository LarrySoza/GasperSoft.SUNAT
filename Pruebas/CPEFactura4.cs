// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class CPEFactura4
    {
        /// <summary>
        /// FACTURA CON 4 ÍTEMS Y UNA BONIFICACIÓN - Pagina 77 Manual SUNAT
        /// Nota: Los cálculos utilizados en este ejemplo se realizaron utilizando un software de facturación 
        /// que implementan todos los posibles casos de SUNAT, es una aplicación de escritorio de escrita hace 
        /// unos años cuando aún se requería pasar homologación, si desea adquirirlo puede escribir a it@gaspersoft.com
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

            //Tomar en cuentas que todo lo unitario soportan hasta 10 decimales para asegurar la presión de cálculo
            //según los manuales oficiales de SUNAT
            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "GLG199",
                    codigoProductoSunat = "52161515",
                    nombre = "GRABADORA LG EXTERNO MODELO: GE20LU10",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 2000,
                    valorVentaUnitario = 83.0508474576m,
                    precioVentaUnitario = 88.2m,
                    descuento = new DescuentoCargoType()
                    {
                        montoBase = 166101.69m,
                        importe = 16610.17m,
                        tasa = 0.1m
                    },
                    valorVenta = 149491.53m,
                    montoBaseIGV = 149491.53m,
                    montoIGV = 26908.47m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 26908.47m
                },
                new ItemCPEType()
                {
                    codigoProducto = "MVS546",
                    codigoProductoSunat = "43211902",
                    nombre = "Monitor LCD ViewSonic VG2028WM 20\"",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 300,
                    valorVentaUnitario = 525.4237288136m,
                    precioVentaUnitario = 527,
                    descuento = new DescuentoCargoType()
                    {
                        montoBase = 157627.12m,
                        importe = 23644.07m,
                        tasa = 0.15m
                    },
                    valorVenta = 133983.05m,
                    montoBaseIGV = 133983.05m,
                    montoIGV = 24116.95m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 24116.95m
                },
                //Notar que en este ítem exonerado el "valorVentaUnitario" y "precioVentaUnitario" son iguales,
                //y a pesar que no tiene IGV el "montoBaseIGV" es un campo obligatorio por diseño de esta librería
                new ItemCPEType()
                {
                    codigoProducto = "MPC35",
                    codigoProductoSunat = "43202010",
                    nombre = "MEMORIA DDR-3 B1333 KINGSTON",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 250,
                    valorVentaUnitario = 52,
                    precioVentaUnitario = 52,
                    valorVenta = 13000,
                    montoBaseIGV = 13000,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "20",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                },
                new ItemCPEType()
                {
                    codigoProducto = "TMS22",
                    codigoProductoSunat = "43211706",
                    nombre = "TECLADO MICROSOFT SIDEWINDER X6",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 500,
                    valorVentaUnitario = 166.1016949153m,
                    precioVentaUnitario = 196,
                    valorVenta = 83050.85m,
                    montoBaseIGV = 83050.85m,
                    montoIGV = 14949.15m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 14949.15m
                },
                //Mirar que a pesar de ser gratuito mando:
                //  precioVentaUnitario = 30,
                //  valorVenta = 150,
                //  montoBaseIGV = 150,
                //Esto es por diseño y esta informacion es de referencia segun lo establecido por SUNAT
                new ItemCPEType()
                {
                    codigoProducto = "WCG01",
                    codigoProductoSunat = "45121520",
                    nombre = "WEB CAM GENIUS ISLIM 310",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 5,
                    valorVentaUnitario = 0,
                    precioVentaUnitario = 30,
                    valorVenta = 150,
                    montoBaseIGV = 150,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "31",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                }
            };

            //Forma de pago al contado
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Contado
            };

            //El descuento del 5 %, hay que tomar en cuenta que el valor tasa de este elemento no se usa en la generación del XML,
            //en su remplazo se usara el valor de la propiedad "tasaDescuentoGlobal" del objeto CPEType, este es un comportamiento
            //establecido con el fin de guardar compatibilidad con clientes antiguos de la librería en futuras actualizaciones esto
            //podría cambiar por lo que es recomendable mandar la tasa de descuento en este objeto

            //Este descuento se aplica a las operaciones grabadas
            var _descuentoGlobalAfectaBI = new DescuentoCargoType()
            {
                montoBase = 366525.43m,
                importe = 18326.27m,
                tasa = 0.05m
            };

            //Este descuento se aplica a las operaciones exoneradas
            var _descuentoGlobalNoAfectaBI = new DescuentoCargoType()
            {
                montoBase = 13000,
                importe = 650,
                tasa = 0.05m
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
                numero = 4,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                tasaDescuentoGlobal = 0.05m,
                descuentoGlobalAfectaBI = _descuentoGlobalAfectaBI,
                descuentoGlobalNoAfectaBI = _descuentoGlobalNoAfectaBI,
                totalDescuentosNoAfectaBI = 650,
                totalOperacionesGravadas = 348199.15m,
                totalOperacionesExoneradas = 13000,
                totalOperacionesGratuitas = 150,
                sumatoriaIGV = 62675.85m,
                sumatoriaImpuestos = 62675.85m,
                valorVenta = 361199.16m,
                precioVenta = 423875,
                importeTotal = 423225 //Notar que difiere del precioVenta en 650 que es el importe de totalDescuentosNoAfectaBI
            };

            return _cpe;
        }
    }
}
