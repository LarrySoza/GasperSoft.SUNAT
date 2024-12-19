// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Informacion de anticipo
    /// </summary>
    public class AnticipoCPEType
    {
        /// <summary>
        /// Tipo de documento de identificacion del emisor del anticipo
        /// </summary>
        public string tipoDocumentoIdentificacionEmisor { get; set; }

        /// <summary>
        /// Numero de documento de identificacion del emisor del anticipo
        /// </summary>
        public string numeroDocumentoIdentificacionEmisor { get; set; }

        /// <summary>
        /// Fecha de pago del comprobante
        /// </summary>
        public DateTime fechaPago { get; set; }

        /// <summary>
        /// tipo de documento- Catalogo 12 SUNAT
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// La serie del comprobante de anticipo
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// El numero del comprobante de anticipo
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Importe total del anticipo = valorVenta + montoIGV
        /// </summary>
        public decimal importeTotal { get; set; }

        /// <summary>
        /// Genera Un AllowanceCharge con AllowanceChargeReasonCode = '04'
        /// </summary>
        public decimal totalOperacionesGravadas { get; set; }

        /// <summary>
        /// Genera Un AllowanceCharge con AllowanceChargeReasonCode = '06'
        /// </summary>
        public decimal totalOperacionesExportacion { get; set; }

        /// <summary>
        ///  Genera Un AllowanceCharge con AllowanceChargeReasonCode = '05'
        /// </summary>
        public decimal totalOperacionesExoneradas { get; set; }

        /// <summary>
        ///  Genera Un AllowanceCharge con AllowanceChargeReasonCode = '06'
        /// </summary>
        public decimal totalOperacionesInafectas { get; set; }

        /// <summary>
        ///  Genera Un AllowanceCharge con AllowanceChargeReasonCode = '20'
        /// </summary>
        public decimal totalISC { get; set; }
    }
}
