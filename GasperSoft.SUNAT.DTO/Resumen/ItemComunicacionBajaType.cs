// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.Resumen
{
    public class ItemComunicacionBajaType
    {
        /// <summary>
        /// El identificador unico del comprobante en Simple-Ubl
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
