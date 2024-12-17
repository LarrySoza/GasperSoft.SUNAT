// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Informacion de un vehiculo
    /// </summary>
    public class InfoVehiculoType
    {
        /// <summary>
        /// Número de placa del vehiculo
        /// </summary>
        public string numeroPlaca { get; set; }

        /// <summary>
        /// Tarjeta Única de Circulación Electrónica o Certificado de Habilitación vehicular
        /// </summary>
        public string numeroTarjeta { get; set; }

        /// <summary>
        /// Datos de la autorización especial para el traslado de la carga
        /// </summary>
        public List<AutorizacionEspecialType> autorizacionesEspeciales { get; set; }
    }
}
