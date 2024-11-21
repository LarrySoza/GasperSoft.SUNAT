// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class DatosEnvioGREType
    {
        /// <summary>
        /// Catalogo 20 SUNAT
        /// </summary>
        public string motivoTraslado { get; set; }

        /// <summary>
        /// Descripcion de hasta 100 caracteres del motivo de traslado
        /// </summary>
        public string descripcionMotivoTraslado { get; set; }

        /// <summary>
        /// El peso bruto total de los bienes que se estan transportando
        /// </summary>
        public decimal pesoBrutoTotalBienes { get; set; }

        /// <summary>
        /// Catalogo 03 SUNAT
        /// </summary>
        public string unidadMedidaPesoBruto { get; set; }

        /// <summary>
        /// Este valor es condicional, indica la cantidad de bultos que se estan transportando,
        /// Solo cuando 'motivoTraslado' ='08' o '09'(importacion/exportacion)
        /// </summary>
        public int totalBultos { get; set; }

        /// <summary>
        /// Catalogo 18 SUNAT
        /// </summary>
        public string modalidadTraslado { get; set; }

        /// <summary>
        /// La fecha en la que se inicia el traslado de bienes
        /// </summary>
        public DateTime fechaInicioTraslado { get; set; }

        /// <summary>
        /// Número de placa(s) del vehiculo
        /// </summary>
        [Obsolete("Usar la propiedad \"vehículos\" para asignar información de los vehículos usados en el transporte")]
        public List<string> placasVehiculo { get; set; }

        /// <summary>
        /// Información de los Vehículos, según las validaciones de SUNAT se permiten máximo 3 unidades uno principal y otros 2 secundarios
        /// </summary>
        public List<InfoVehiculo> vehiculos { get; set; }

        /// <summary>
        /// Informacion de los conductores
        /// </summary>
        public List<InfoConductorType> conductores { get; set; }

        /// <summary>
        /// Datos del contenedor (Motivo Importación) - Condicional
        /// </summary>
        public string numeroContenedor { get; set; }

        /// <summary>
        /// Datos del contenedor (Motivo Importación) - Condicional
        /// </summary>
        public string codigoPuerto { get; set; }

        /// <summary>
        /// Informacion del punto de llegada
        /// </summary>
        public InfoDireccionGREType puntoLlegada { get; set; }

        /// <summary>
        /// Informacion del punto de partida
        /// </summary>
        public InfoDireccionGREType puntoPartida { get; set; }

        /// <summary>
        /// Indicadores para la guia de remision Remitente
        /// </summary>
        public IndicadoresGRERemitenteType indicadoresGRERemitente { get; set; }

        /// <summary>
        /// Indicadores para la guia de remision Transportista
        /// </summary>
        public IndicadoresGRETransportistaType indicadoresGRETransportista { get; set; }
    }
}
