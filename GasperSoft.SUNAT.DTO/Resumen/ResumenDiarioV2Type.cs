// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.Resumen
{
    /// <summary>
    /// Resumen diario (Boletas/Notas credito debito relacionadas)
    /// </summary>
    public class ResumenDiarioV2Type
    {
        /// <summary>
        /// Id del resumen ren formato RC-[yyyyMMdd]-#####
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Fecha de emision de los documentos
        /// </summary>
        public DateTime fechaEmisionDocumentos { get; set; }

        /// <summary>
        /// Fecha de generación del resumen
        /// </summary>
        public DateTime fechaGeneracion { get; set; }

        /// <summary>
        /// Los detalles del resumen diario
        /// </summary>
        public List<ItemResumenDiarioV2Type> detalles { get; set; }
    }
}
