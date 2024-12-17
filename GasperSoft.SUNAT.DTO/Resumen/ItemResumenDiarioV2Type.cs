// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.Resumen
{
    /// <summary>
    /// Detalle de un resumen diario
    /// </summary>
    public class ItemResumenDiarioV2Type
    {
        /// <summary>
        /// Número de orden del Ítem
        /// </summary>
        public int secuencia { get; set; }

        /// <summary>
        /// El tipo de documento
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
        /// Códigos de estado de ítem (Catalogo 19 SUNAT)
        /// </summary>
        public string estadoItem { get; set; }

        #region Percepcion

        /// <summary>
        /// 51 o 52 o 53 Catálogo No. 53
        /// </summary>
        public string regimenPercepcion { get; set; }

        /// <summary>
        /// Tasa de percepcion del item
        /// </summary>
        public decimal tasaPercepcion { get; set; }

        /// <summary>
        /// Monto de percepcion del item
        /// </summary>
        public decimal montoPercepcion { get; set; }

        #endregion

        #region Adquirente

        /// <summary>
        /// Este campo es obligatorio si es factura
        /// </summary>
        public string tipoDocumentoIdentificacionAdquirente { get; set; }

        /// <summary>
        /// Este campo es obligatorio si es factura
        /// </summary>
        public string numeroDocumentoIdentificacionAdquirente { get; set; }

        #endregion

        #region Documento de referencia(Notas credito-debito)

        /// <summary>
        /// El tipo de documento
        /// </summary>
        public string tipoDocumentoModifica { get; set; }

        /// <summary>
        /// La serie del comprobante que modifica
        /// </summary>
        public string serieModifica { get; set; }

        /// <summary>
        /// El numero del comprobante que modifica
        /// </summary>
        public int numeroModifica { get; set; }

        #endregion

        /// <summary>
        /// El codigo de moneda Catalogo 02 Sunat
        /// </summary>
        public string codMoneda { get; set; }

        /// <summary>
        /// Representa el importe total a pagar para el documento:
        /// Nota: No incluye monto de Percepcion
        /// </summary>
        public decimal importeTotal { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad valorVenta de items gravados(valor de codAfectacionIgv = 10)
        /// </summary>
        public decimal totalOperacionesGravadas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad valorVenta de items exonerados(valor de codAfectacionIgv = 20)
        /// </summary>
        public decimal totalOperacionesExoneradas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad valorVenta de items inafectos(valor de codAfectacionIgv = 30)
        /// </summary>
        public decimal totalOperacionesInafectas { get; set; }

        /// <summary>
        /// Es la sumatoria de la propiedad valorVenta de items de exportacion(valor de codAfectacionIgv  del item = 40)
        /// </summary>
        public decimal totalOperacionesExportacion { get; set; }

        /// <summary>
        /// Es la sumatoria de la propiedad valorVenta de items que no son 10,20,30,40
        /// </summary>
        public decimal totalOperacionesGratuitas { get; set; }

        /// <summary>
        /// Corresponde al total de otros cargos cobrados al adquirente o usuario y que no forman parte de la operación que se factura, 
        /// es decir no forman parte del(os) valor(es) de venta señalados anteriormente, pero sí forman parte del importe total de la Venta 
        /// Ejemplo: propinas, garantías para devolución de envases, etc.
        /// </summary>
        public decimal sumatoriaOtrosCargos { get; set; }

        /// <summary>
        /// La sumatoria de la propiedad montoISC de cada item
        /// </summary>
        public decimal sumatoriaISC { get; set; }

        /// <summary>
        /// El IGV se calcula sobre la suma : [Total valor de venta operaciones gravadas] + [Sumatoria ISC].
        /// </summary>
        public decimal sumatoriaIGV { get; set; }

        /// <summary>
        /// La sumatoria de la propiedad montoICBPER de cada item
        /// </summary>
        public decimal sumatoriaICBPER { get; set; }

        /// <summary>
        /// La suma total de otro tributos aplicados al comprobante
        /// </summary>
        public decimal sumatoriaOTH { get; set; }
    }
}
