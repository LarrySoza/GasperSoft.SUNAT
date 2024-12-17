﻿// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Información del Remitente de la guía de remisión
    /// </summary>
    public class InfoRemitenteType : InfoPersonaType
    {
        /// <summary>
        /// Datos de la autorización especial para el traslado de la carga
        /// </summary>
        public List<AutorizacionEspecialType> autorizacionesEspeciales { get; set; }
    }
}
