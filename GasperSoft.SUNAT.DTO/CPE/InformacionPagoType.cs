// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Información de pago
    /// </summary>
    public class InformacionPagoType
    {
        /// <summary>
        /// Forma de pago
        /// </summary>
        public FormaPagoType formaPago { get; set; }

        /// <summary>
        /// Solo cuando formaPago="Credito"
        /// </summary>
        public decimal montoPendientePago { get; set; }

        /// <summary>
        /// Solo cuando formpago="Credito"
        /// </summary>
        public List<CuotaType> cuotas { get; set; }

        /// <summary>
        /// Indicar solo si formaPago="Contado"
        /// Catalogo 59 de SUNAT
        /// </summary>
        public string metodoPago { get; set; }
    }
}
