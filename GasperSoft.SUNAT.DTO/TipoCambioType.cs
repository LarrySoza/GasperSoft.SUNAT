// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO
{
    public class TipoCambioType
    {
        /// <summary>
        /// Catalogo 02 Sunat
        /// </summary>
        public string codMonedaOrigen { get; set; }

        /// <summary>
        /// Catalogo 02 Sunat
        /// </summary>
        public string codMonedaDestino { get; } = "PEN";

        /// <summary>
        /// Fecha del tipo de cambio
        /// </summary>
        public DateTime fecha { get; set; }

        /// <summary>
        /// Factor aplicado a la moneda de origen para calcular la moneda de destino (Tipo de cambio)
        /// </summary>
        public decimal factorConversion { get; set; }
    }
}
