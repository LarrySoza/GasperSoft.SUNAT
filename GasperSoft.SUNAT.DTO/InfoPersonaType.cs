// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO
{
    /// <summary>
    /// Informacion de una Persona
    /// </summary>
    public class InfoPersonaType
    {
        /// <summary>
        /// Código de país Catalogo nro. 04
        /// </summary>
        public string codigoPais { get; set; }

        /// <summary>
        /// Tipo de documento de identificación Catalogo nro. 06
        /// </summary>
        public string tipoDocumentoIdentificacion { get; set; }

        /// <summary>
        /// Numero de documento de identificacion
        /// </summary>
        public string numeroDocumentoIdentificacion { get; set; }

        /// <summary>
        /// Nombre
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// Direccion
        /// </summary>
        public string direccion { get; set; }
    }
}
