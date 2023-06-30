// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class InfoRemitenteType
    {
        public string ruc { get; set; }
        public string razonSocial { get; set; }

        /// <summary>
        /// Datos de la autorización especial para el traslado de la carga
        /// codigo debe estar en el catalogo D37
        /// </summary>
        public List<DatoAdicionalType> autorizacionesEspeciales { get; set; }
    }
}
