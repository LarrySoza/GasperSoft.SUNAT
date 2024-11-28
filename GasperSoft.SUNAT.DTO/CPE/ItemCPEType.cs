// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.CPE
{
    public class ItemCPEType
    {
        /// <summary>
        /// Código del producto de acuerdo al tipo de codificación interna que se utilice.
        /// Su uso será obligatorio si el emisor electrónico, opta por consignar este código, en
        /// reemplazo de la descripción detallada.Para tal efecto el código a usar será aquél, que las
        /// normas que regulan el llevado de libros y registros, denominan como código de existencia.
        /// </summary>
        public string codigoProducto { get; set; }

        /// <summary>
        /// solo se permite GTIN-8, GTIN-12, GTIN-13, GTIN-14
        /// </summary>
        public string tipoCodigoProductoGS1 { get; set; }

        /// <summary>
        /// UBL 2.1
        /// El codigo del Articulo-Servicio GS1 
        /// </summary>
        public string codigoProductoGS1 { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Código del producto de acuerdo al estándar internacional de la ONU denominado:
        /// United Nations Standard Products and Services Code - Código de productos y servicios
        /// estándar de las Naciones Unidas - UNSPSC v14_0801, a que hace referencia el catálogo N° 25
        /// </summary>
        public string codigoProductoSunat { get; set; }

        /// <summary>
        /// Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando el nombre y las características, 
        /// tales como marca del bien vendido o cedido en uso.
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// Catalogo 03
        /// Se consigna la unidad de medida de los bienes por ítem, para el caso peruano se usará el catalogo internacional UN/ECE rec 20- Unit Of Measure.
        /// No será necesario colocar la unidad de medida si ésta es “NIU”(unidad) 0 “ZZ”.
        /// </summary>
        public string unidadMedida { get; set; }

        /// <summary>
        /// Se consignará la cantidad de productos vendidos o servicios prestados en la operación. En el caso de retiro de bienes, 
        /// se consignará la cantidad de bienes transferidos a titulo gratuito.Cuando se trate de servicios o cualquier otra operación 
        /// no cuantificable se deberá consignar el valor uno(1)
        /// </summary>
        public decimal cantidad { get; set; }

        /// <summary>
        /// Se consignará el importe correspondiente al valor o monto unitario del bien vendido, cedido o servicio prestado, 
        /// indicado en una línea o ítem de la factura. Este importe no incluye los tributos (IGV, ISC y otros Tributos) ni los cargos globales.
        /// </summary>
        public decimal valorVentaUnitario { get; set; }

        /// <summary>
        /// Es el monto correspondiente al precio unitario facturado del bien vendido o servicio vendido. Este monto es la suma total que queda 
        /// obligado a pagar el adquirente o usuario por cada bien o servicio. Esto incluye los tributos (IGV, ISC y otros Tributos) y la deducción de descuentos por ítem.
        /// </summary>
        public decimal precioVentaUnitario { get; set; }

        /// <summary>
        /// Su propósito es permitir consignar en el comprobante de pago, un descuento a nivel de línea o ítem.
        /// </summary>
        public DescuentoCargoType descuento { get; set; }

        /// <summary>
        /// Este elemento es el producto de la cantidad por el valor unitario ( Q x Valor Unitario) y la deducción de los descuentos aplicados a dicho ítem 
        /// (de existir). Este importe no incluye los tributos (IGV, ISC y otros Tributos), los descuentos globales o cargos.
        /// (Sobre este valor se calcula el IGV, por lo que puede ser considerado como monto base para el calculo del IGV)
        /// </summary>
        public decimal valorVenta { get; set; }

        #region IGV

        /// <summary>
        /// Representa el monto sobre el cual se calcula el IGV
        /// </summary>
        public decimal montoBaseIGV { get; set; }

        /// <summary>
        /// El monto total del IGV(Este valor se calcula sobre el valorVenta + montoIsc)
        /// </summary>
        public decimal montoIGV { get; set; }

        /// <summary>
        /// Representa el % del IGV valor del 1 al 100
        /// </summary>
        public decimal tasaIGV { get; set; }

        /// <summary>
        /// Afectación al IGV - Catálogo No. 07
        /// </summary>
        public string codAfectacionIGV { get; set; }

        #endregion

        #region ISC

        /// <summary>
        /// Representa el monto sobre el cual se calcula el ISC
        /// </summary>
        public decimal montoBaseISC { get; set; }

        /// <summary>
        /// El monto total del ISC
        /// </summary>
        public decimal montoISC { get; set; }

        /// <summary>
        /// Representa el % del ISC valor del 1 al 100
        /// </summary>
        public decimal tasaISC { get; set; }

        /// <summary>
        /// Tipo de sistema de ISC - Catálogo No. 08
        /// </summary>
        public string codSistemaCalculoISC { get; set; }

        #endregion

        #region ICBPER

        /// <summary>
        /// El monto total del impuesto a las bolsas plasticas
        /// </summary>
        public decimal montoICBPER { get; set; }

        /// <summary>
        /// La aplicada a cada bolsa plastica
        /// </summary>
        public decimal tasaUnitariaICBPER { get; set; }

        #endregion

        #region OTH

        /// <summary>
        /// UBL 2.1
        /// Representa el monto sobre el cual se calcula el valor otros tributos
        /// </summary>
        public decimal montoBaseOTH { get; set; }

        /// <summary>
        /// El monto total de otros tributos
        /// </summary>
        public decimal montoOTH { get; set; }

        /// <summary>
        /// Representa el % de otros tributos (valor del 1 al 100)
        /// </summary>
        public decimal tasaOTH { get; set; }

        #endregion

        #region Otros Cargos/Descuentos que no afectan a la base imponible

        /// <summary>
        /// Colocar aqui otros cargos que no afectan a la base imponible
        /// </summary>
        public DescuentoCargoType otrosCargosNoAfectaBI { get; set; }

        /// <summary>
        /// El descuento aplicado a otros cargos que no afectan a la base imponible
        /// Si es mayor a 0 se registra un AllowanceCharge con AllowanceChargeReasonCode = '01' 
        /// </summary>
        public DescuentoCargoType descuentoNoAfectaBI { get; set; }

        #endregion

        /// <summary>
        /// UBL 2.1
        /// La suma de los valores de montoIgv, montoIsc, montoOth y montoICBPER
        /// </summary>
        public decimal sumatoriaImpuestos { get; set; }

        #region Campos Adiconales(No se mandan a SUNAT se usan para mandar informacion adicional en el pdf)

        public string adicional1 { get; set; }

        public string adicional2 { get; set; }

        public string adicional3 { get; set; }

        public string adicional4 { get; set; }

        public string adicional5 { get; set; }

        #endregion

        /// <summary>
        /// UBL 2.1
        /// Número de placa del vehículo(Información Adicional - Gastos art.37° Renta)
        /// Leer: http://www.sunat.gob.pe/legislacion/superin/2017/155-2017.pdf,
        /// Era obligatorio consignar el numero de placa en la venta de combustible por ejemplo
        /// </summary>
        public string numeroPlacaVehiculo { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Información Adicional - Beneficio de hospedaje
        /// </summary>
        public HospedajeType informacionHospedaje { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Información Adicional - Transporte terrestre de pasajeros
        /// </summary>
        public PasajeTransporteTerrestreType informacionPasajeTransporteTerrestre { get; set; }
    }
}
