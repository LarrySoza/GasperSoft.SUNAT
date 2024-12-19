// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Información del conductor
    /// </summary>
    public class InfoConductorType
    {
        /// <summary>
        /// Codigo del paise
        /// </summary>
        public string codigoPais { get; set; }

        /// <summary>
        /// Tipo de documento de identificación
        /// </summary>
        public string tipoDocumentoIdentificacion { get; set; }

        /// <summary>
        /// Numero de documento de identificación
        /// </summary>
        public string numeroDocumentoIdentificacion { get; set; }

        /// <summary>
        /// Nombre del conductor
        /// </summary>
        public string nombres { get; set; }

        /// <summary>
        /// Apellidos del conductor
        /// </summary>
        public string apellidos { get; set; }

        /// <summary>
        /// Licencia de conducir
        /// </summary>
        public string licenciaConducir { get; set; }
    }
}
