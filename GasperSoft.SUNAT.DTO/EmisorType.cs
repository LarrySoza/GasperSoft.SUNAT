// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO
{
    /// <summary>
    /// Esta clase contiene informacion del Emisor
    /// </summary>
    public class EmisorType
    {
        /// <summary>
        /// Tipo de documento de identificacion
        /// </summary>
        public string tipoDocumentoIdentificacion { get { return "6"; } }

        /// <summary>
        /// Ruc
        /// </summary>
        public string ruc { get; set; }

        /// <summary>
        /// Razon Social
        /// </summary>
        public string razonSocial { get; set; }

        /// <summary>
        /// Nombre Comercial (opcional)
        /// </summary>
        public string nombreComercial { get; set; }

        /// <summary>
        /// Codigo del Pais
        /// </summary>
        public string codigoPais { get { return "PE"; } }

        /// <summary>
        /// Codigo de ubigeo SUNAT
        /// </summary>
        public string codigoUbigeo { get; set; }

        /// <summary>
        /// Urbanizacion (opcional)
        /// </summary>
        public string urbanizacion { get; set; }

        /// <summary>
        /// Domicilio fiscal
        /// </summary>
        public string direccion { get; set; }

        /// <summary>
        /// Nombre del Departamento del Domicilio fiscal
        /// </summary>
        public string departamento { get; set; }

        /// <summary>
        /// Nombre de la Provincia del Domicilio fiscal
        /// </summary>
        public string provincia { get; set; }

        /// <summary>
        /// Nombre del Distrito del Domicilio fiscal
        /// </summary>
        public string distrito { get; set; }

        /// <summary>
        /// Resolución Sunat (opcional antes de usaba)
        /// </summary>
        public string resolucionSunat { get; set; }
    }
}
