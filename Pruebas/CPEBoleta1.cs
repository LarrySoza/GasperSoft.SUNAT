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
    internal class CPEBoleta1
    {
        /// <summary>
        /// BOLETA DE VENTA GRAVADA CON DOS ÍTEMS Y UNA BONIFICACIÓN - Pagina 60 Manual SUNAT
        /// la unicas diferencias son el fecha emision, emisor, receptor, codigo de establecimiento y serie-numero
        /// </summary>
        public static CPEType GetDocumento()
        {
            //Informacion del Receptor
            var _adquirente = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "1",
                numeroDocumentoIdentificacion = "45288569",
                nombre = "VELEZMORO SOZA JHON LARRY"
            };

            //Tomar en cuentas que todo lo unitario soportan hasta 10 decimales para asegurar la presión de cálculo
            //según los manuales oficiales de SUNAT
            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "REF564",
                    codigoProductoSunat = "52141501",
                    nombre = "REFRIGERADORA MARCA \"AXM\" NO FROST DE 200 LTRS.",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 845.7627118644m,
                    precioVentaUnitario = 998,
                    valorVenta = 845.76m,
                    montoBaseIGV = 845.76m,
                    montoIGV = 152.24m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 152.24m
                },
                new ItemCPEType()
                {
                    codigoProducto = "COC124",
                    codigoProductoSunat = "95141606",
                    nombre = "COCINA A GAS GLP, MARCA \"AXM\" DE 5 HORNILLAS",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 635.593220339m,
                    precioVentaUnitario = 750,
                    valorVenta = 635.59m,
                    montoBaseIGV = 635.59m,
                    montoIGV = 114.41m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 114.41m
                },
                //Mirar que a pesar de ser gratuito mando:
                //  precioVentaUnitario = 4.8m,
                //  valorVenta = 4.8m <== precioVentaUnitario * cantidad
                //  montoBaseIGV = 4.8m <== precioVentaUnitario * cantidad
                //Esto es porque la SUNAT requiere información del precio de referencia y así está diseñada esta librería para poder generar el XML correctamente
                new ItemCPEType()
                {
                    codigoProducto = "NOB012",
                    codigoProductoSunat = "24121803",
                    nombre = "SIXPACK DE GASEOSA \"GUERENÉ\" DE 400 ML",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 0,
                    precioVentaUnitario = 4.8m,
                    valorVenta = 4.8m,
                    montoBaseIGV = 4.8m,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "31",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                },
            };

            //El descuento del 5 %, hay que tomar en cuenta que el valor tasa de este elemento no se usa en la generación del XML,
            //en su remplazo se usara el valor de la propiedad "tasaDescuentoGlobal" del objeto CPEType, este es un comportamiento
            //establecido con el fin de guardar compatibilidad con clientes antiguos de la librería en futuras actualizaciones esto
            //podría cambiar por lo que es recomendable mandar la tasa de descuento en este objeto
            var _descuento = new DescuentoCargoType()
            {
                montoBase = 1481.35m,
                importe = 74.07m,
                tasa = 0.05m
            };

            //Cuerpo de una factura
            //Observe que "precioVenta" y "importeTotal" son iguales, en algunos casos por ejemplo en anticipos
            //tenemos una factura por "precioVenta" X, al cual despues de restarle los importes de anticipos
            //dan como resultado el "importeTotal".
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "03",//Catalogo N° 01
                serie = "B001",
                numero = 1,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                tasaDescuentoGlobal = 0.05m, //5%
                descuentoGlobalAfectaBI = _descuento,
                totalOperacionesGravadas = 1407.29m,
                totalOperacionesGratuitas = 4.8m,
                sumatoriaIGV = 253.31m,
                sumatoriaImpuestos = 253.31m,
                valorVenta = 1407.28m,
                precioVenta = 1660.6m,
                importeTotal = 1660.6m
            };

            return _cpe;
        }
    }
}
