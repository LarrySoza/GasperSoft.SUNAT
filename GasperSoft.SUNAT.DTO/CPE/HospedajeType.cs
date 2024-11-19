// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    public class HospedajeType
    {
        /// <summary>
        /// Código de país de emisión del pasaporte, Codigo "4000" del catalogo 55
        /// </summary>
        public string codigoPaisEmisionPasaporte { get; set; }

        /// <summary>
        /// Código de país de residencia del sujeto no domiciliado, Codigo "4001" del catalogo 55
        /// </summary>
        public string codigoPaisResidenciaSujetoNoDomiciliado { get; set; }

        /// <summary>
        /// Fecha de ingreso al país, Codigo "4002" del catalogo 55
        /// </summary>
        public DateTime? fechaIngresoPais { get; set; }

        /// <summary>
        /// Fecha de Ingreso al Establecimiento, Codigo "4003" del catalogo 55
        /// </summary>
        public DateTime? fechaIngresoEstablecimiento { get; set; }

        /// <summary>
        /// Fecha de Salida del Establecimiento, Codigo "4004" del catalogo 55
        /// </summary>
        public DateTime? fechaSalidaEstablecimiento { get; set; }

        /// <summary>
        /// Número de Días de Permanencia, Codigo "4005" del catalogo 55
        /// </summary>
        public int numeroDiasPermanencia { get; set; }

        /// <summary>
        /// Fecha de Consumo, Codigo "4006" del catalogo 55
        /// </summary>
        public DateTime? fechaConsumo { get; set; }

        /// <summary>
        /// Nombres y apellidos del huesped, Codigo "4007" del catalogo 55
        /// </summary>
        public string nombreHuesped { get; set; }

        /// <summary>
        /// Tipo de documento de identidad del huesped, Codigo "4008" del catalogo 55
        /// </summary>
        public string tipoDocumentoIdentificacionHuesped { get; set; }

        /// <summary>
        /// Número de documento de identidad del huesped, Codigo "4009" del catalogo 55
        /// </summary>
        public string numeroDocumentoIdentificacionHuesped { get; set; }
    }
}
