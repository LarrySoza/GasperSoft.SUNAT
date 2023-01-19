// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// El id en formato "Cuota[0-9]{3}" se generara automaticamente
    /// </summary>
    public class CuotaType
    {
        /// <summary>
        /// El monto de la cuota
        /// </summary>
        public decimal monto { get; set; }

        /// <summary>
        /// La fecha en la que debe realizar el pago
        /// </summary>
        public DateTime fechaPago { get; set; }
    }
}
