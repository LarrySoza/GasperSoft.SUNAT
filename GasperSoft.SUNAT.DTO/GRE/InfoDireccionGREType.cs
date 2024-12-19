// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Información de una dirección
    /// </summary>
    public class InfoDireccionGREType
    {
        /// <summary>
        /// Codigo de ubigeo
        /// </summary>
        public string ubigeo { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        public string direccion { get; set; }

        /// <summary>
        /// Ruc asociado a la dirección
        /// </summary>
        public string rucAsociado { get; set; }

        /// <summary>
        /// Codigo de establecimiento
        /// </summary>
        public string codigoEstablecimiento { get; set; }
    }
}
