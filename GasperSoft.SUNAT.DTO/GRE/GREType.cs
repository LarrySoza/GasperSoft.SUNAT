// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class GREType
    {
        /// <summary>
        /// El codigo de la unidad de negocio en la cual se genera el comprobante, esta variable se usa para determinar la plantilla
        /// que se va usar para generar el PDF, de ser vacio o nulo se usa la plantilla predeterminada
        /// </summary>
        public string codigoUnidadNegocio { get; set; }

        /// <summary>
        /// IDentificador unico del comprobante en los sistemas del contribuyente
        /// </summary>
        public string idTransaccion { get; set; }

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
        /// Guía de Remisión Dada de Baja
        /// </summary>
        public List<DocumentoRelacionadoGREType> guiasRemisionDadasDeBaja { get; set; }

        /// <summary>
        /// Documentos relacionados Catalogo 61
        /// </summary>
        public List<DocumentoRelacionadoGREType> documentosRelacionados { get; set; }

        /// <summary>
        /// Informacion del remitente
        /// </summary>
        public InfoPersonaType remitente { get; set; }

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
        /// Informacion del envio 
        /// </summary>
        public DatosEnvioGREType datosEnvio { get; set; }

        /// <summary>
        /// Informacion adicional del comprobante de pago estos datos no se envian a sunat.
        /// Sirven para infromar a "Simple-Ubl" datos que se deben consignar en la representacion impresa
        /// </summary>
        public List<DatoAdicionalType> informacionAdicional { get; set; }

        /// <summary>
        /// Bienes a transportar - Obligatorio
        /// </summary>
        public List<ItemGREType> detalles { get; set; }
    }
}
