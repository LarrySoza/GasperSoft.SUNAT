// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CRE;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class CRE2
    {
        /// <summary>
        /// Ejemplo de comprobante de Retencion de Factura en DOLARES
        /// </summary>
        /// <returns></returns>
        public static CREType GetDocumento()
        {
            //Informacion del proveedor
            var _proveedor = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20454738056",
                nombre = "CHARACATO EXPRESS S.R.L.",
                direccion = "AV. BOLOGNESI NRO. 210 URB. FRANCISCO BOLOGNESI - AREQUIPA AREQUIPA CAYMA"
            };

            //Contiene informacion de los documentos relacionados a la retencion
            var _detalles = new List<ItemCREType>()
            {
                //Una factura emitida hace 2 dias por 1000 USD le vamos a retener el 3%
                new ItemCREType()
                {
                    documentoRelacionadoTipoDocumento = "01",
                    documentoRelacionadoSerie = "F001",
                    documentoRelacionadoNumero = 1,
                    documentoRelacionadoFechaEmision = DateTime.Now.Subtract(new TimeSpan(2,0,0,0)),
                    documentoRelacionadoCodMoneda = "USD",
                    documentoRelacionadoImporteTotal = 1000,
                    numeroPago = 1,
                    fechaPago = DateTime.Now,
                    pagoTotalSinRetencion = 1000,//Es el importe que le debemos pagar al proveedor sin retencion(Se usa la Moneda del documento Relacionado)
                    importePagadoConRetencion = 3659.81m,//El importe que le debemos pagar al proveedor con retencion(EN SOLES) - (970 * 3.773)
                    importeRetenido = 113,//El importe que le estamos reteniendo(EN SOLES) - redondeado a entero
                    importeRetenidoFecha = DateTime.Now,//la fecha en la que le estamos reteniendo
                    tipoCambio = new TipoCambioType()
                    {
                        codMonedaOrigen = "USD",
                        factorConversion = 3.773m,
                        fecha = DateTime.Now
                    }
                }
            };

            //Notar que no se asigna el codMoneda ya que las retenciones son en soles y esta propiedad es solo de lectura
            var _cre = new CREType()
            {
                fechaEmision = DateTime.Now,
                serie = "R001",
                numero = 2,
                proveedor = _proveedor,
                codigoRegimenRetencion = "01",//Catalogo 23
                tasaRetencion = 3,//Catalogo 23
                importeTotalRetenido = 113,//Suma del 'importeRetenido' de cada detalle (sin considerar los tipos de documentos '07' y '20')
                totalRedondeoImporteTotalPagado = 0.19m,//importeTotalRetenido podria salir con decimales y se debe redondear colocar aqui esa diferencia puede ser negativo o positivo 
                importeTotalPagado = 3660,//Suma del 'importePagadoConRetencion' de cada detalle (sin considerar los tipos de documentos '07' y '20') + totalRedondeoImporteTotalPagado
                detalles = _detalles,
                Observaciones = "COMPROBANTE DE RETENCION DE PRUEBA",
            };

            return _cre;
        }
    }
}
