// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.DTO.CPE
{
    /// <summary>
    /// Esta clase contiene todos los datos necesarios para armar una Factura-Boleta-Nota Credito-Nota Debito
    /// </summary>
    public class CPEType
    {
        /// <summary>
        /// UBL 2.1
        /// Para efectos de identificar la transacción se deberá indicar el código de operación que
        /// corresponda de acuerdo al catálogo N° 51 del Anexo 8 aprobado por la Resolución de
        /// Superintendencia N° 097-2012/SUNAT y modificatorias.
        /// </summary>
        public string codigoTipoOperacion { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Código de cuatro dígitos asignado por SUNAT, que identifica al
        /// establecimiento anexo.Dicho código se genera al momento la respectiva comunicación del
        /// establecimiento. Tratándose del domicilio fiscal y en el caso de no poder determinar el lugar
        /// de la venta, informar “0000”
        /// </summary>
        public string codigoEstablecimiento { get; set; }

        /// <summary>
        /// Colocar aqui el numero de orden de compra(opcional)
        /// </summary>
        public string ordenCompra { get; set; }

        /// <summary>
        /// Requerido por SUNAT el 1ª Abril
        /// </summary>
        public InformacionPagoType informacionPago { get; set; }

        /// <summary>
        /// Fecha de emision del comprobante
        /// </summary>
        public DateTime fechaEmision { get; set; }

        /// <summary>
        /// Hora de emision del comprobante Formato "HH:mm:ss"
        /// </summary>
        public string horaEmision { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Fecha de vencimiento del comprobante
        /// </summary>
        public DateTime? fechaVencimiento { get; set; }

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
        /// Datos del Adquiriente del comprobante
        /// </summary>
        public InfoPersonaType adquirente { get; set; }

        /// <summary>
        /// Los items del comprobante
        /// </summary>
        public List<ItemCPEType> detalles { get; set; }

        /// <summary>
        /// El codigo de moneda Catalogo 02 Sunat
        /// </summary>
        public string codMoneda { get; set; }

        /// <summary>
        /// La tasa de descuento aplicada a operaciones Gravadas-Exoneradas-Inafectas y Exportacion
        /// </summary>
        public decimal tasaDescuentoGlobal { get; set; }

        /// <summary>
        /// Es la sumatoria de la propiedad valorVenta de items gravados(valor de codAfectacionIgv  del item comiensa con 1..)
        /// </summary>
        public decimal totalOperacionesGravadas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad valorVenta de items exonerados(valor de codAfectacionIgv  del item comiensa con 2..)
        /// </summary>
        public decimal totalOperacionesExoneradas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad valorVenta de items inafectos(valor de codAfectacionIgv  del item comiensa con 3..)
        /// </summary>
        public decimal totalOperacionesInafectas { get; set; }

        /// <summary>
        /// Es la sumatoria de la propiedad valorVenta de items de exportacion(valor de codAfectacionIgv  del item = 40)
        /// </summary>
        public decimal totalOperacionesExportacion { get; set; }

        /// <summary>
        /// Colocar aqui la sumatoria de la propiedad valorVenta de items que no son 10,20,30,40
        /// </summary>
        public decimal totalOperacionesGratuitas { get; set; }

        /// <summary>
        /// descuento aplicado a las operaciones Gravadas
        /// </summary>
        public DescuentoCargoType descuentoGlobalAfectaBI { get; set; }

        /// <summary>
        ///         /// Al igual que 'descuentoOperacionesGravadas' la tasa sera el valor de 'tasaDescuentoGlobal'
        /// Colocar aqui la suma de descuento aplicado a operaciones Exonerdas,Inafectas,Exportacion,Recargo al consumo
        /// </summary>
        public DescuentoCargoType descuentoGlobalNoAfectaBI { get; set; }

        /// <summary>
        /// La sumatoria de descuentoGlobalNoAfectaBI + descuentoNoAfectaBI de cada linea
        /// </summary>
        public decimal totalDescuentosNoAfectaBI { get; set; }

        /// <summary>
        /// El importe que se quito/agrego para el redondeo debe ser menor que 1 y mayor que -1
        /// </summary>
        public decimal totalRedondeo { get; set; }

        #region Impuestos

        #region ISC

        /// <summary>
        /// UBL 2.1
        /// Representa el monto sobre el cual se calcula el ISC
        /// </summary>
        public decimal sumatoriaMontoBaseISC { get; set; }

        /// <summary>
        /// La sumatoria de la propiedad montoISC de cada item
        /// </summary>
        public decimal sumatoriaISC { get; set; }

        #endregion

        #region IGV

        /// <summary>
        /// El IGV se calcula sobre la suma : [Total valor de venta operaciones gravadas] + [Sumatoria ISC].
        /// </summary>
        public decimal sumatoriaIGV { get; set; }

        /// <summary>
        /// Las Operaciones Gravadas Gratuitas estan obligadas a consignar el IGV
        /// </summary>
        public decimal sumatoriaIGVGratuitas { get; set; }

        #endregion

        #region ICBPER

        /// <summary>
        /// La sumatoria de la propiedad montoICBPER de cada item
        /// </summary>
        public decimal sumatoriaICBPER { get; set; }

        #endregion

        #region OTH

        /// <summary>
        /// UBL 2.1
        /// Representa el monto sobre el cual se calcula el valor otros tributos(sumatoriaOth)
        /// </summary>
        public decimal sumatoriaMontoBaseOTH { get; set; }

        /// <summary>
        /// La suma total de otros tributos aplicados al comprobante
        /// </summary>
        public decimal sumatoriaOTH { get; set; }

        #endregion

        /// <summary>
        /// UBL 2.1
        /// La suma de los valores de sumatoriaIGV, sumatoriaISC, sumatoriaOTH y sumatoriaICBPER
        /// </summary>
        public decimal sumatoriaImpuestos { get; set; }

        #endregion

        #region Otros Cargos

        /// <summary>
        /// UBL 2.1
        /// El Fondo de Inclusión Social Energético (FISE)
        /// </summary>
        public DescuentoCargoType recargoFISE { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Recargo al consumo/propina de existir
        /// </summary>
        public DescuentoCargoType recargoAlConsumo { get; set; }

        /// <summary>
        /// Informacion de percepcion
        /// </summary>
        public PercepcionType percepcion { get; set; }

        /// <summary>
        /// Otros cargos globales que no afectan a la base imponible, PaidOut(Prestamos)
        /// </summary>
        public DescuentoCargoType otrosCargosGlobalNoAfectaBI { get; set; }

        /// <summary>
        /// Corresponde al total de otros cargos cobrados al adquirente o usuario y que no forman parte de la operación que se factura, 
        /// es decir no forman parte del(os) valor(es) de venta señalados anteriormente, pero sí forman parte del importe total de la Venta 
        /// (recargpFISE + montoRecargoAlConsumo + montoPropina + percepcion.importe + otrosCargosGlobalNoAfectaBI) + sumatoria del valor de la propiedad montoOtrosCargosNoAfectaBI de cada Item
        /// </summary>
        public decimal sumatoriaOtrosCargosNoAfectaBI { get; set; }

        #endregion

        #region Anticipos

        /// <summary>
        /// Lista de documentos de anticipo
        /// </summary>
        public List<AnticipoCPEType> anticipos { get; set; }

        /// <summary>
        /// La sumatoria total de los anticipos
        /// </summary>
        public decimal totalAnticipos { get; set; }

        #endregion

        /// <summary>
        /// Informacion de la retencion
        /// </summary>
        public DescuentoCargoType retencion { get; set; }

        /// <summary>
        /// Informacion de detraccion
        /// </summary>
        public SPOTType detraccion { get; set; }

        /// <summary>
        /// Sumatoria de la propiedad valorVenta de cada item - DescuentoGlobalesAfectaBI(codigo 02 del catalogo 53) + cargosGlobalesAfectaBI(codigo 49 catalogo 53)
        /// </summary>
        public decimal valorVenta { get; set; }

        /// <summary>
        ///  Valor de venta total de la operación incluido los impuestos.(importeTotal - sumatoriaOtrosCargos + totalDescuentos + totalAnticipos - totalRedondeo)
        /// </summary>
        public decimal precioVenta { get; set; }

        /// <summary>
        /// Representa el importe total a pagar para el documento:
        /// Nota: No incluye monto de Percepcion
        /// </summary>
        public decimal importeTotal { get; set; }

        /// <summary>
        /// Importe total en letras
        /// </summary>
        public string importeTotalLetras { get; set; }

        /// <summary>
        ///  hace referencia a documentos de transporte asociados a la nota de crédito.
        /// </summary>
        public List<DocumentoRelacionadoCPEType> guiasRemision { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Tipo y número de otro documento y/ código documento relacionado con la operación que se factura.
        /// </summary>
        public List<DocumentoRelacionadoCPEType> documentosRelacionados { get; set; }

        /// <summary>
        /// Datos para las notas de credito y debito
        /// </summary>
        public List<MotivoNotaType> motivosNota { get; set; }

        #region Propiedades que agregan leyendas al comprobante catalogo 52

        /// <summary>
        /// Agregar la leyenda codigo: 1002("TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE") 
        /// </summary>
        public bool indTransferenciaGratuita { get; set; }

        /// <summary>
        /// Agregar la leyenda codigo: 2001("BIENES TRANSFERIDOS EN LA AMAZONÍA REGIÓN SELVA PARA SER CONSUMIDOS EN LA MISMA")
        /// </summary>
        public bool indBienesTransferidosSelva { get; set; }

        /// <summary>
        /// Agregar la leyenda codigo: 2002("SERVICIOS PRESTADOS EN LA AMAZONÍA REGIÓN SELVA PARA SER CONSUMIDOS EN LA MISMA")
        /// </summary>
        public bool indServiciosSelva { get; set; }

        /// <summary>
        /// Agregar la leyenda codigo: 2003("CONTRATOS DE CONSTRUCCIÓN EJECUTADOS EN LA AMAZONÍA REGIÓN SELVA")
        /// </summary>
        public bool indContratosSelva { get; set; }

        #endregion

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

        ///// <summary>
        ///// Informacion del envio (Factura Guia)
        ///// </summary>
        //public DatosEnvioGREType datosEnvio { get; set; }
    }
}
