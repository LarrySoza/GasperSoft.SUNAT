// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT.DTO.CPE
{
    public class MotivoNotaType
    {
        /// <summary>
        /// Fecha de emision del comprobante, de tratarse de un documento electronico este valor se asigna por SimpleUbl 
        /// al consultar informacion del documento de referencia, este valor no se envia a SUNAT
        /// </summary>
        public DateTime fechaEmision { get; set; }

        /// <summary>
        /// El tipo de documento, Catalogo 01 sunat
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// La serie del comprobante
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// El numero del comprobante
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Catalogo 9 y 10 de Sunat
        /// </summary>
        public string tipoNota { get; set; }

        /// <summary>
        /// Sustento o Motivo de modificar un documento mediante una Nota
        /// </summary>
        public string sustento { get; set; }
    }
}
