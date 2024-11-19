// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.CPE
{
    public class PercepcionType : DescuentoCargoType
    {
        /// <summary>
        /// 51 o 52 o 53 Catálogo No. 53
        /// </summary>
        public string codigo { get; set; }

        public string codMoneda { get; } = "PEN";

        /// <summary>
        /// Monto total en SOlES del comprobante (incluido percepcion)
        /// </summary>
        public decimal importeTotalEnSolesConPercepcion { get; set; }
    }
}
