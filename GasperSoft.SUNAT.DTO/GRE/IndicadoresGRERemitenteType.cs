// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class IndicadoresGRERemitenteType
    {
        /// <summary>
        /// SUNAT_Envio_IndicadorTransbordoProgramado
        /// </summary>
        public bool indTransbordoProgramado { get; set; }

        /// <summary>
        /// SUNAT_Envio_IndicadorTrasladoVehiculoM1L
        /// </summary>
        public bool indTrasladoVehiculoM1L { get; set; }

        /// <summary>
        /// SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio
        /// </summary>
        public bool indRetornoVehiculoEnvaseVacio { get; set; }

        /// <summary>
        /// SUNAT_Envio_IndicadorRetornoVehiculoVacio
        /// </summary>
        public bool indRetornoVehiculoVacio { get; set; }

        /// <summary>
        /// SUNAT_Envio_IndicadorTrasladoTotalDAMoDS
        /// </summary>
        public bool indTrasladoTotalDAMoDS { get; set; }

        /// <summary>
        /// SUNAT_Envio_IndicadorVehiculoConductoresTransp
        /// </summary>
        public bool indVehiculoConductoresTransp { get; set; }
    }
}
