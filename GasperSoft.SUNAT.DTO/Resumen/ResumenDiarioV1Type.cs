// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.Resumen
{
    public class ResumenDiarioV1Type
    {
        /// <summary>
        /// Este valor es inicializado por Simle-Ubl antes de registrar el resumen
        /// RC-<Fecha>-#####
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
        public List<ItemResumenDiarioV1Type> detalles { get; set; }
    }
}
