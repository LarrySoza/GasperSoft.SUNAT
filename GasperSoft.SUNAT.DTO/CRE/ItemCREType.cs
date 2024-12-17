// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CRE
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemCREType
    {
        /// <summary>
        /// El tipo de documento, Catalogo 01 sunat
        /// </summary>
        public string documentoRelacionadoTipoDocumento { get; set; }

        /// <summary>
        /// La serie del comprobante
        /// </summary>
        public string documentoRelacionadoSerie { get; set; }

        /// <summary>
        /// El numero del comprobante
        /// </summary>
        public int documentoRelacionadoNumero { get; set; }

        /// <summary>
        /// Fecha de emision del comprobante
        /// </summary>
        public DateTime documentoRelacionadoFechaEmision { get; set; }

        /// <summary>
        /// El codigo de moneda Catalogo 02 Sunat
        /// </summary>
        public string documentoRelacionadoCodMoneda { get; set; }

        /// <summary>
        /// Representa el importe total a pagar para el documento
        /// </summary>
        public decimal documentoRelacionadoImporteTotal { get; set; }

        public DateTime fechaPago { get; set; }

        public int numeroPago { get; set; }

        public decimal pagoTotalSinRetencion { get; set; }

        public decimal importeRetenido { get; set; }

        public DateTime importeRetenidoFecha { get; set; }

        public decimal importePagadoConRetencion { get; set; }

        /// <summary>
        /// Solo cuando "documentoRelacionadoCodMoneda" es diferente de PEN
        /// </summary>
        public TipoCambioType tipoCambio { get; set; }
    }
}
