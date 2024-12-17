// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.Resumen
{
    /// <summary>
    /// Detalle de una comunicación de baja
    /// </summary>
    public class ItemComunicacionBajaType
    {
        /// <summary>
        /// El identificador unico del comprobante en los sistemas del contribuyente
        /// Nota: Esta propiedad no se usa en la generación del XML pero puede ser útil para integración en los sistemas 
        /// que requieran un identificador único del documento a dar de baja.
        /// </summary>
        public string comprobanteId { get; set; }

        /// <summary>
        /// El tipo de documento
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// Numero de serie de los documentos
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// El numero del comprobante
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// El motivo por el cual se da de baja al documento
        /// </summary>
        public string motivo { get; set; }
    }
}
