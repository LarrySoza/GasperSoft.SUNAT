// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Información del transportista
    /// </summary>
    public class InfoTransportistaType
    {
        /// <summary>
        /// Ruc del transportista
        /// </summary>
        public string ruc { get; set; }

        /// <summary>
        /// Razon social del transportista
        /// </summary>
        public string razonSocial { get; set; }

        /// <summary>
        /// Registro MTC
        /// </summary>
        public string registroMTC { get; set; }

        /// <summary>
        /// Datos de la autorización especial para el traslado de la carga
        /// </summary>
        public List<AutorizacionEspecialType> autorizacionesEspeciales { get; set; }
    }
}
