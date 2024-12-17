// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.CRE
{
    /// <summary>
    /// Esta clase contiene todos los datos necesarios para armar una Retencion
    /// </summary>
    public class CREType
    {
        /// <summary>
        /// Fecha de Emision
        /// </summary>
        public DateTime fechaEmision { get; set; }

        /// <summary>
        /// Serie de la retencion
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// Numero de retencion
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Informacion del proveedor
        /// </summary>
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

        /// <summary>
        /// Indica si se debe incluir los valores de la propiedad informacionAdicional en el XML
        /// </summary>
        public bool informacionAdicionalEnXml { get; set; }

        /// <summary>
        /// Informacion adicional del comprobante de pago estos datos no se envian a sunat por defecto
        /// si se requiere estos datos en el XML puede establecer la propiedad "informacionAdicionalEnXml" = true,
        /// su propósito es poder usar estos datos en la versión impresa del comprobante
        /// </summary>
        public List<DatoAdicionalType> informacionAdicional { get; set; }
    }
}
