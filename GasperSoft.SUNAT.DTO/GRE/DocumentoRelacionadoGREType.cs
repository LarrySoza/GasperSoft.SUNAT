// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class DocumentoRelacionadoGREType
    {
        public InfoPersonaType emisor { get; set; }

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
