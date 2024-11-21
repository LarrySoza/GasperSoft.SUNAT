// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class AutorizacionEspecialType
    {
        /// <summary>
        /// Código de entidad emisora - catalogo D37
        /// </summary>
        public string codigoEntidadAutorizadora { get; set; }

        /// <summary>
        /// Número de autorización
        /// </summary>
        public string numeroAutorizacion { get; set; }
    }
}
