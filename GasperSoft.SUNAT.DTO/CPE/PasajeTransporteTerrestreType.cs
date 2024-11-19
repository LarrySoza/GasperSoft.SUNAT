// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    public class PasajeTransporteTerrestreType
    {
        public string numeroAsiento { get; set; }

        public string numeroManifiesto { get; set; }

        public string tipoDocumentoIdentificacionPasajero { get; set; }

        public string numeroDocumentoIdentificacionPasajero { get; set; }

        public string nombrePasajero { get; set; }

        public string ubigeoOrigen { get; set; }

        public string direccionOrigen { get; set; }

        public string ubigeoDestino { get; set; }

        public string direccionDestino { get; set; }

        public DateTime? fechaViaje { get; set; }

        public string horaViaje { get; set; }
    }
}
