// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.GRE
{
    /// <summary>
    /// Esta clase contiene todos los datos necesarios para armar una Guia Remision Remitente/Transportista
    /// </summary>
    public class GREType
    {
        /// <summary>
        /// El tipo de guia de Remision 09 = Guia Remision Remitente, 31 = Guia Remision Transportista
        /// </summary>
        public string tipoGuia { get; set; }

        /// <summary>
        /// La serie de la Guia de Remision
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// El numero de la Guia de Remision
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Fecha de emision del comprobante
        /// </summary>
        public DateTime fechaEmision { get; set; }

        /// <summary>
        /// Hora de emision del comprobante
        /// </summary>
        public string horaEmision { get; set; }

        /// <summary>
        /// Observaciones sobre la guia de remision
        /// </summary>
        public List<string> observaciones { get; set; }

        /// <summary>
        /// Documentos relacionados Catalogo 61
        /// </summary>
        public List<DocumentoRelacionadoGREType> documentosRelacionados { get; set; }

        /// <summary>
        /// Informacion del transportista
        /// </summary>
        public InfoTransportistaType transportista { get; set; }

        /// <summary>
        /// Empresa que subcontrata(Solo guías remisión transportista)
        /// </summary>
        public InfoTransportistaType empresaSubcontrata { get; set; }

        /// <summary>
        /// Informacion del remitente
        /// </summary>
        public InfoRemitenteType remitente { get; set; }

        /// <summary>
        /// Informacion del destinatario
        /// </summary>
        public InfoPersonaType destinatario { get; set; }

        /// <summary>
        /// Informacion del Proveedor
        /// </summary>
        public InfoPersonaType proveedor { get; set; }

        /// <summary>
        /// Informacion del Comprador
        /// </summary>
        public InfoPersonaType comprador { get; set; }

        /// <summary>
        /// Informacion del persona(Solo guías remisión transportista) 
        /// </summary>
        public InfoPersonaType pagadorFlete { get; set; }

        /// <summary>
        /// Informacion del envio 
        /// </summary>
        public DatosEnvioGREType datosEnvio { get; set; }

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

        /// <summary>
        /// Bienes a transportar - Obligatorio
        /// </summary>
        public List<ItemGREType> detalles { get; set; }
    }
}
