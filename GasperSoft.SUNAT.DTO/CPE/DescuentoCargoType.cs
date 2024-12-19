// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Descuento o cargo aplicado al comprobante
    /// </summary>
    public class DescuentoCargoType
    {
        /// <summary>
        /// Monto sobre el cual se calcula del descuento/cargo
        /// </summary>
        public decimal montoBase { get; set; }

        /// <summary>
        /// Monto del descuento/cargo
        /// </summary>
        public decimal importe { get; set; }

        /// <summary>
        /// Porcentaje del descuento/cargo
        /// </summary>
        public decimal tasa { get; set; }
    }
}
