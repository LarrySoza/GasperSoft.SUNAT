// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Información de un pasaje de transporte terrestre
    /// </summary>
    public class PasajeTransporteTerrestreType
    {
        /// <summary>
        /// Número de asiento
        /// </summary>
        public string numeroAsiento { get; set; }

        /// <summary>
        /// Información de manifiesto de pasajeros
        /// </summary>
        public string numeroManifiesto { get; set; }

        /// <summary>
        /// Tipo de documento de identidad del pasajero
        /// </summary>
        public string tipoDocumentoIdentificacionPasajero { get; set; }

        /// <summary>
        /// Número de documento de identidad del pasajero
        /// </summary>
        public string numeroDocumentoIdentificacionPasajero { get; set; }

        /// <summary>
        /// Nombres y apellidos del pasajero
        /// </summary>
        public string nombrePasajero { get; set; }

        /// <summary>
        /// Ubigeo de origen
        /// </summary>
        public string ubigeoOrigen { get; set; }

        /// <summary>
        /// Ciudad o lugar de origen
        /// </summary>
        public string direccionOrigen { get; set; }

        /// <summary>
        /// Ubigeo de destino
        /// </summary>
        public string ubigeoDestino { get; set; }

        /// <summary>
        /// Ciudad o lugar de destino
        /// </summary>
        public string direccionDestino { get; set; }

        /// <summary>
        /// Fecha de inicio programado
        /// </summary>
        public DateTime? fechaViaje { get; set; }

        /// <summary>
        /// Hora de inicio programado
        /// </summary>
        public string horaViaje { get; set; }
    }
}
