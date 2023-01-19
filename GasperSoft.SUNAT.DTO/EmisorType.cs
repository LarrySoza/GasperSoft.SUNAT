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
        public string tipoDocumentoIdentificacion { get { return "6"; } }
        public string ruc { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public string codigoPais { get { return "PE"; } }
        public string codigoUbigeo { get; set; }
        public string urbanizacion { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public string resolucionSunat { get; set; }
    }
}
