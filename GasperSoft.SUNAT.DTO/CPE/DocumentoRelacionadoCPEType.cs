// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Esta clase se usa para asignar documentos de referencia relacionados con el comprobante
    /// </summary>
    public class DocumentoRelacionadoCPEType
    {
        /// <summary>
        /// Tipo de documento 
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// Si es guía (an..30 <Serie>-<Número>)
        /// </summary>
        public string numeroDocumento { get; set; }
    }
}
