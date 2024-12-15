// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;

namespace Pruebas
{
    internal class CPEFactura5
    {
        /// <summary>
        /// FACTURA CON 2 ÍTEMS E ISC - Pagina 88 Manual SUNAT
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
                /* 2000 Unidades con un Precio venta unitario de S/ 38
                 * Tasa ISC en sistema de precios de venta al público: 27.8% de precio de venta sugerido. Precio sugerido: S/. 37
                 * Descuento de 20% por compras mayores a 500 cajas de cerveza
                 * 
                 * PASOS(No aplicar redondeo hasta el final)
                 * =====
                 * 1° El producto esta afecto al ISC por precio de referencia entonces calculamos el ISC correspondiente
                 * 
                 *    montoISCUnitario = 37 * 27.8 / 100 = 10.286
                 *    montoISC = montoISCUnitario * cantidad = 10.286 * 2000 = 20572
                 *    
                 * 2° El valor venta unitario no contiene ningun impuesto entonces procedemos a quitar el igv y el ISC unitario
                 * 
                 *    valorVentaUnitario = 38 / 1.18 - 10.286 = 21.9173898305
                 *    
                 * 3° Ahora procedemos a calcular el Valor Venta
                 * 
                 *    valorVenta = valorVentaUnitario * cantidad = 21.9173898305 * 2000 = 43834.77966 <== Ahun no le aplicamos el descuento
                 *    
                 * 4° Ahora ese valorVenta esta sujeto a un descuento del 20%
                 * 
                 *    montoBase = 43834.77966 <== El monto base es el valorVenta previamente calculado 
                 *    tasa = 0.2
                 *    importe = montoBase * tasa = 8766.955932 <== Este es el importe total del descuento
                 *    
                 * 5° Ahora el valorVenta se le tiene que restar el descuento
                 * 
                 *    valorVenta = valorVenta - descuento = 43834.77966 - 8766.955932 = 35067.82373
                 *    
                 * 6° El calculo del IGV se realiza sobre la suma del valorVenta + montoISC
                 * 
                 *    montoBaseIGV = 35067.82373 + 20572 = 55639.82373
                 *    montoIGV = 55639.82373 * 0.18 = 10015.16827
                 *    
                 * 7° Para este ejemplo la sumatoria de impuestos es la suma de IGV + ISC 
                 * 
                 *    sumatoriaImpuestos = 10015.16827 + 20572 = 30587.16827
                 *    
                 * 8° Muy importante SUNAT aplica una regla de validación con Código 3270 que calcula el precio venta por item y lo divide entre la cantidad
                 *    
                 *    PrecioVenta = ValorVenta + MontoISC + MontoIGV + MontoICBPER(En este ejemplo no existe) = 35067.82373 + 20572 + 10015.16827 = 65654.992
                 *    
                 *    precioVentaUnitario = PrecioVenta / cantidad = 65654.992 / 2000 = 32.827496
                 *
                 * 9° Teniendo el montoISC y la tasaISC calculamos el montoBaseISC
                 * 
                 *    montoBaseISC = montoISC * 100 / tasaISC = 20572 * 100 / 27.8 = 74000
                 * 
                 */

                new ItemCPEType()
                {
                    codigoProducto = "GLG199",
                    codigoProductoSunat = "50202201",
                    nombre = "CERVEZA “CLÁSICA” X 12 BOT. 620 ML.",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 2000,
                    valorVentaUnitario = 21.9173898305m,
                    precioVentaUnitario = 32.827496m,
                    descuento = new DescuentoCargoType()
                    {
                        montoBase = 43834.78m,
                        importe = 8766.96m,
                        tasa = 0.2m
                    },
                    valorVenta = 35067.82m,
                    montoBaseIGV = 55639.82m,
                    montoIGV = 10015.17m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    montoBaseISC= 74000m,
                    montoISC= 20572m,
                    tasaISC= 27.8m,
                    codSistemaCalculoISC = "03",
                    sumatoriaImpuestos = 30587.17m
                },

                /* 300 Unidades con un Precio venta unitario de S/ 20
                 * Tasa ISC en sistema al valor para el agua: 17%
                 * Descuento de 5% por compras mayores a 250 cajas de agua mineral en cualquier presentación y/o marca
                 * 
                 * PASOS(No aplicar redondeo hasta el final)
                 * =====
                 * 1° El producto tiene un ISC del 17% aplicado al valorVenta sabiendo esto podemos calcular el valorVentaUnitario
                 * 
                 *    valorVentaUnitario = 20 / 1.18 / 1.17 = 14.48645516
                 * 
                 * 2° Ahora procedemos a calcular el Valor Venta
                 * 
                 *    valorVenta = valorVentaUnitario * cantidad = 14.48645516 * 300 = 4345.936549 <== Ahun no le aplicamos el descuento
                 *    
                 * 3° Ahora ese valorVenta esta sujeto a un descuento del 5%
                 * 
                 *    montoBase = 4345.936549 <== El monto base es el valorVenta previamente calculado 
                 *    tasa = 0.05
                 *    importe = montoBase * tasa = 217.2968275 <== Este es el importe total del descuento
                 *    
                 * 4° Ahora el valorVenta se le tiene que restar el descuento
                 * 
                 *    valorVenta = valorVenta - descuento = 4345.936549 - 217.2968275 = 4128.639722
                 * 
                 * 5° En este caso(ISC en sistema al valor) el ISC representa un 17% del valorVenta
                 * 
                 *    montoBaseISC = valorVenta = 4128.639722
                 *    montoISC = montoBaseISC * tasaISC = 4128.639722 * 0.17 = 701.8687527
                 *    
                 * 6° El calculo del IGV se realiza sobre la suma del valorVenta + montoISC
                 * 
                 *    montoBaseIGV = 4128.639722 + 701.8687527 = 4830.508475
                 *    montoIGV = 4830.508475 * 0.18 = 869.4915255
                 *    
                 * 7° Para este ejemplo la sumatoria de impuestos es la suma de IGV + ISC 
                 * 
                 *    sumatoriaImpuestos = 869.4915255 + 701.8687527 = 1571.360278
                 *    
                 * 8° Muy importante SUNAT aplica una regla de validación con Código 3270 que calcula el precio venta por item y lo divide entre la cantidad
                 *    
                 *    PrecioVenta = ValorVenta + MontoISC + MontoIGV + MontoICBPER(En este ejemplo no existe) = 4128.639722 + 701.8687527 + 869.4915255 = 5700
                 *    precioVentaUnitario = PrecioVenta / cantidad = 5700 / 300 = 19
                 *
                 */
                new ItemCPEType()
                {
                    codigoProducto = "MVS546",
                    codigoProductoSunat = "50202310",
                    nombre = "AGUA MINERAL SIN GAS “SAN BLAS” X 12 BOT. 400 ML.",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 300,
                    valorVentaUnitario = 14.4864551644m,
                    precioVentaUnitario = 19,
                    descuento = new DescuentoCargoType()
                    {
                        montoBase = 4345.94m,
                        importe = 217.3m,
                        tasa = 0.05m
                    },
                    valorVenta = 4128.64m,
                    montoBaseIGV = 4830.51m,
                    montoIGV = 869.49m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    montoBaseISC= 4128.64m,
                    montoISC= 701.87m,
                    tasaISC= 17m,
                    codSistemaCalculoISC = "01",
                    sumatoriaImpuestos = 1571.36m
                },

                /* Entrega de 100 vasos descartables con el logo de la compañía de cerveza. Campaña “Mundial 2014”. Código: PROM23
                 * Mirar que a pesar de ser gratuito mando:
                 *  precioVentaUnitario = 0.5m,
                 *  valorVenta = 50 <== precioVentaUnitario * cantidad
                 *  montoBaseIGV = 50 <== precioVentaUnitario * cantidad
                 *  Esto es porque la SUNAT requiere información del precio de referencia y así está diseñada esta librería para poder generar el XML correctamente
                 */
                new ItemCPEType()
                {
                    codigoProducto = "PROM23",
                    codigoProductoSunat = "52151504",
                    nombre = "VASOS DESCARTABLES CON EL LOGO DE LA COMPAÑÍA DE CERVEZA",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 100,
                    valorVentaUnitario = 0,
                    precioVentaUnitario = 0.5m,
                    valorVenta = 50,
                    montoBaseIGV = 50,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "36",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                }
            };

            //Forma de pago al contado
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Contado
            };

            var _percepcion = new PercepcionType()
            {
                tasa = 0.02m,
                montoBase = 71354.99m,
                importe = 1427.1m,
                codigo = "51", //Catalogo N° 53 solo se permite '51', '52', '53'
                importeTotalEnSolesConPercepcion = 72782.09m
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "2001",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",//Catalogo N° 01
                serie = "F001",
                numero = 5,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 39196.46m,
                totalOperacionesGratuitas = 50,
                sumatoriaMontoBaseISC = 78128.64m,
                sumatoriaISC = 21273.87m,
                sumatoriaIGV = 10884.66m,
                sumatoriaImpuestos = 32158.53m,
                valorVenta = 39196.46m,
                precioVenta = 71354.99m,
                importeTotal = 71354.99m,
                percepcion = _percepcion
            };

            return _cpe;
        }
    }
}
