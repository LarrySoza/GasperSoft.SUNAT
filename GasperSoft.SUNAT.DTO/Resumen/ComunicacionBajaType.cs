// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.Resumen
{
    /// <summary>
    /// Comunicaciones de Bajas (Facturas/Notas credito debito relacionadas y Retenciones)
    /// </summary>
    public class ComunicacionBajaType
    {
        /// <summary>
        /// Id del resumen ren formato RA-[yyyyMMdd]-##### (Facturas/Notas credito debito relacionadas) o RR-[yyyyMMdd]-##### (Retenciones)
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Debe ser igual a la fecha de emision de todos los detalles
        /// </summary>
        public DateTime fechaBaja { get; set; }

        /// <summary>
        /// Fecha de generación de la comunicación
        /// </summary>
        public DateTime fechaGeneracion { get; set; }

        /// <summary>
        /// Los detalles de la comunicacion de baja
        /// </summary>
        public List<ItemComunicacionBajaType> detalles { get; set; }
    }
}
