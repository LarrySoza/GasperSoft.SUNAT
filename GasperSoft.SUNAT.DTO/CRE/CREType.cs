// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.CRE
{
    public class CREType
    {
        public DateTime fechaEmision { get; set; }

        public string serie { get; set; }

        public int numero { get; set; }

        public InfoPersonaType proveedor { get; set; }

        /// <summary>
        /// Catalogo N° 23
        /// </summary>
        public string codigoRegimenRetencion { get; set; }

        /// <summary>
        /// Representa el % del retencion valor del 1 al 100
        /// </summary>
        public decimal tasaRetencion { get; set; }

        /// <summary>
        /// Importe total retenido
        /// </summary>
        public decimal importeTotalRetenido { get; set; }

        /// <summary>
        /// Importe total Pagado
        /// </summary>
        public decimal importeTotalPagado { get; set; }

        /// <summary>
        /// El importe que se quito/agrego para el redondeo del importe total Pagado debe ser menor que 1 y mayor que -1
        /// </summary>
        public decimal totalRedondeoImporteTotalPagado { get; set; }

        /// <summary>
        /// Catalogo 02 Sunat
        /// </summary>
        public string codMoneda { get; } = "PEN";

        /// <summary>
        /// Opcional - an..250
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Los items del comprobante
        /// </summary>
        public List<ItemCREType> detalles { get; set; }
    }
}
