// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Almacena informacion de la detraccion
    /// </summary>
    public class SPOTType
    {
        /// <summary>
        /// Numero de cuenta del banco de la nacion
        /// </summary>
        public string numeroCuentaBancoNacion { get; set; }

        /// <summary>
        /// Codigo de detraccion Catalogo 54
        /// </summary>
        public string codigoBienServicio { get; set; }

        /// <summary>
        /// Porcentaje de detracción
        /// </summary>
        public decimal porcentaje { get; set; }

        /// <summary>
        /// El codigo de moneda Catalogo 02 Sunat
        /// </summary>
        public string codMoneda { get; set; }

        /// <summary>
        /// Monto de la detracción
        /// </summary>
        public decimal importe { get; set; }

        /// <summary>
        /// Catalogo 59 de SUNAT, por defecto "001"(Depósito en cuenta)
        /// </summary>
        public string metodoPago { get; set; } = "001";
    }
}
