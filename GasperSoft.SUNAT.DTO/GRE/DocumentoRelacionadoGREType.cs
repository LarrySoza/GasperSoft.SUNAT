// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Documento relacionado con el comprobante de retención
    /// </summary>
    public class DocumentoRelacionadoGREType
    {
        /// <summary>
        /// Informacion del emisor del documento relacionado
        /// </summary>
        public InfoPersonaType emisor { get; set; }

        /// <summary>
        /// Tipo de documento de documento relacionado (Catalogo 61)
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// Si es guía (an..30 [Serie]-[Número])
        /// </summary>
        public string numeroDocumento { get; set; }
    }
}
