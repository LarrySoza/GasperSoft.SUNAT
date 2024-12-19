// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Indicadores de la Guia Transportista
    /// </summary>
    public class IndicadoresGRETransportistaType
    {
        /// <summary>
        /// SUNAT_Envio_IndicadorTransbordoProgramado
        /// </summary>
        public bool indTransbordoProgramado { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio
        /// </summary>
        public bool indRetornoVehiculoEnvaseVacio { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorRetornoVehiculoVacio
        /// </summary>
        public bool indRetornoVehiculoVacio { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorTrasporteSubcontratado
        /// </summary>
        public bool indTrasporteSubcontratado { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorPagadorFlete_Remitente
        /// </summary>
        public bool indPagadorFleteRemitente { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorPagadorFlete_Subcontratador
        /// </summary>
        public bool indPagadorFleteSubcontratador { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorPagadorFlete_Tercero
        /// </summary>
        public bool indPagadorFleteTercero { get; set; }
        /// <summary>
        /// SUNAT_Envio_IndicadorTrasladoTotal
        /// </summary>
        public bool indTrasladoTotal { get; set; }
    }
}
