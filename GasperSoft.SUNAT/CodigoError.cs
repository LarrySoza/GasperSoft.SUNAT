// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT
{
    /// <summary>
    /// He tratado de usar los mismos códigos de error que generaría SUNAT estos códigos comienzan con 'S', validaciones propias de la librería comienzan con 'V'
    /// </summary>
    internal class CodigoError
    {
        //Validaciones SUNAT
        public const string S2017 = "S2017:El numero de documento de identidad del receptor debe ser  RUC";
        public const string S2329 = "S2329:La fecha de emision se encuentra fuera del limite permitido";
        public const string S2511 = "S2511:El tipo de documento no es aceptado, para 'Guías Remisión Remitente', el tipo de documento de identificación del remitente debe ser '6'(RUC)";
        public const string S2523 = "S2523:El valor del atributo es diferente 'KGM' y diferente de 'TNE'";
        public const string S2532 = "S2532:No existe información de modalidad de transporte";
        public const string S2566 = "S2566:El XML no contiene el tag o no existe informacion del Numero de placa del vehículo";
        public const string S2567 = "S2567:Placa debe ser un valor alfanumérico de 6 a 8 caracteres";
        public const string S2573 = "S2573:Licencia de conducir debe ser un valor alfanumérico de 9 a 10 caracteres (no se permite solamente ceros)";
        public const string S2642 = "S2642:Operaciones de exportación, deben consignar Tipo Afectación igual a 40(en todos los ítems)";
        public const string S2760 = "S2760:El valor ingresado como tipo de documento del destinatario es incorrecto";
        public const string S2769 = "S2769:El valor ingresado como numero de DAM no cumple con el estandar";
        public const string S2773 = "S2773:El valor ingresado como modalidad de transporte no es correcto";
        public const string S2776 = "S2776:Ubigeo debe ser un valor numérico de 6 dígitos";
        public const string S2800 = "S2800:El dato ingresado en el tipo de documento de identidad del receptor no esta permitido";
        public const string S2801 = "S2801:El DNI ingresado no cumple con el estandar";
        public const string S2802 = "S2802:El dato ingresado como numero de documento de identidad del receptor no cumple con el formato establecido";
        public const string S2993 = "S2993:El factor de afectación de IGV por linea debe ser diferente a 0.00";
        public const string S3030 = "S3030:No existe información del código de local anexo del emisor";
        public const string S3205 = "S3205:Debe consignar el tipo de operación";
        public const string S3208 = "S3208:La moneda del monto de la detracción debe ser PEN";
        public const string S3206 = "S3206:El dato ingresado como tipo de operación no corresponde a un valor esperado (catálogo nro. 51)";
        public const string S3265 = "S3265:El Monto neto pendiente de pago debe ser menor o igual al Importe total del comprobante";
        public const string S3267 = "S3267:Fecha del pago único o de las cuotas no puede ser anterior o igual a la fecha de emisión del comprobante";
        public const string S3319 = "S3319:La suma de las cuotas debe ser igual al Monto neto pendiente de pago";
        public const string S3343 = "S3343:La Fecha de inicio del traslado debe ser mayor o igual a la Fecha de emision del documento";
        public const string S3347 = "S3347:No debe consignar los datos del transportista para una operacion de Transporte Privado";
        public const string S3352 = "S3352:Si ingreso un documento relacionado tipo '49' (solo en caso de GRE-Remitente) u '80', debe existir al menos un item con Partida arancelaria";
        public const string S3355 = "S3355:El Numero de Constancia de Inscripcion Vehicular o Certificado de Habilitacion Vehicular o la TUC (fisica o electronica) no cumple con el formato establecido";
        public const string S3357 = "S3357:Debe consignar informacion del conductor principal";
        public const string S3377 = "S3377:La Partida arancelaria no cumple con el formato establecido";
        public const string S3414 = "S3414:El Numero de RUC  asociado al punto de partida/llegada debe ser el igual al Numero de RUC del remitente";
        public const string S3426 = "S3426:Si se trata de un bien normalizado por SUNAT, debe indicarse la Partida arancelaria";
        public const string S3430 = "S3430:La Numeracion de la DAM o DS no se encuentra consignado como documento relacionado";
        public const string S3431 = "S3431:El Numero de la serie en la DAM o DS no cumple con el formato establecido";
        public const string S3438 = "S3438:Hora de emisión no cumple con el formato requerido por Sunat (hh:mm:ss)";
        public const string S3440 = "S3440:No se ha ingresado el tipo Declaracion Aduanera de Mercancias (DAM) o el Declaracion Simplificada (DS)  para el motivo de traslado selecionado";
        public const string S3445 = "S3445:El tipo de documento relacionado no corresponde para motivo de traslado seleccionado";
        public const string S3452 = "S3452:No debe ingresar informacion adicional de vehiculos (registros y/o autorizaciones)";
        public const string S3453 = "S3453:No debe ingresar informacion de vehiculos secundarios";
        public const string S4392 = "S4392:El Numero de Registro MTC del transportista no cumple con el formato establecido";
        public const string S3034 = "S3034:No existe información en el nro de cuenta de detracción";
        public const string S3101 = "S3101:El factor de afectación de IGV por linea debe ser igual a 0.00 para Exoneradas, Inafectas, Exportación, Gratuitas de exoneradas o Gratuitas de inafectas";
        public const string S3127 = "S3127:El XML no contiene el tag o no existe información del Codigo de BBSS de detracción para el tipo de operación";
        public const string S3128 = "S3128:El XML contiene información de codigo de bien y servicio de detracción que no corresponde al tipo de operación";
        public const string S3203 = "S3203:El tipo de nota es un dato único (Notas de crédito y débito deben consignar un único elemento en la propiedad 'motivosNota')";
        public const string S3244 = "S3244:Debe consignar la informacion del tipo de transaccion del comprobante('informacionPago' es requerido para facturas y notas de crédito motivo 13)";
        public const string S3249 = "S3249:Si el tipo de transaccion es al Credito debe existir al menos información de una cuota de pago";
        public const string S3356 = "S3356:Solo debe consignar un Numero de autorizacion del vehiculo";
        public const string S3365 = "S3365:Código de establecimiento es obligatorio para el motivo de traslado seleccionado";
        public const string S3404 = "S3404:El XML no contiene el tag o no existe informacion del motivo de traslado";
        public const string S3405 = "S3405:El valor ingresado como motivo de traslado no es valido";
        public const string S3462 = "S3462:La tasa del IGV debe ser la misma en todas las líneas o ítems del documento y debe corresponder con una tasa vigente";
        public const string S4233 = "S4233:El dato ingresado en order de compra no cumple con el formato establecido";
        public const string S4399 = "S4399:No ha consignado el Numero de Constancia de Inscripcion Vehicular o Certificado de Habilitacion Vehicular o la TUC (fisica o electronica)";
        public const string S4403 = "S4403:Debe indicar la entidad autorizadora del vehiculo";
        public const string S4406 = "S4406:El Numero de autorizacion del vehiculo no cumple con el formato establecido";
        public const string S4407 = "S4407:El Codigo de entidad autorizadora del vehiculo no corresponde a un valor esperado(El valor no existe el catalogo N° D37)";

        //Validaciones(Les fui asignado un código correlativo)
        public const string V0001 = "V0001:Tipo de documento de identidad no es válido";
        public const string V0002 = "V0002:Ruc no valido";
        public const string V0003 = "V0003:El valor de la propiedad debe ser un valor alfanumérico de 1 a 500 caracteres, y no empiezan ni terminan con espacios en blanco";
        public const string V0004 = "V0004:El valor de la propiedad debe ser un valor alfanumérico de 3 a 500 caracteres, y no empiezan ni terminan con espacios en blanco";
        public const string V0005 = "V0005:El valor de la propiedad debe ser un valor alfanumérico de 3 a 250 caracteres, y no empiezan ni terminan con espacios en blanco";
        public const string V0006 = "V0006:El valor de la propiedad debe ser un valor alfanumérico de 3 a 1500 caracteres, y no empiezan ni terminan con espacios en blanco";
        public const string V0007 = "V0007:No existen detalles en el documento";
        public const string V0008 = "V0008:Boleta de Venta electrónica y sus notas de crédito y débito asociadas esta obligados a consignar el documento de identidad cuando la operación no es de exportación y el monto supera los 700 SOLES";
        public const string V0009 = "V0009:Serie no es válido para el tipo de documento";
        public const string V0010 = "V0010:El Numero de documento no puede exceder los 8 dígitos según validaciones por SUNAT";
        public const string V0011 = "V0011:El numero de decimales excede el valor máximo permitido";
        public const string V0012 = "V0012:El valor de la propiedad debe ser un numero positivo";
        public const string V0013 = "V0013:El valor de la propiedad debe ser un numero mayor a cero";
        public const string V0014 = "V0014:Solo se pueden emitir notas de crédito y débito para facturas y boletas";
        public const string V0015 = "V0015:El valor de la propiedad 'serie' no es válido para el tipo de documento en uno de los elementos de la propiedad 'motivosNota' del comprobante";
        public const string V0016 = "V0016:El valor de la propiedad 'serie' en todos los elementos de la propiedad 'motivosNota' deben comenzar con la misma letra que el valor de la propiedad 'serie' del documento";
        public const string V0017 = "V0017:No es posible emitir Notas de crédito con motivos 04(descuento global) 05(descuento por ítem) 08(bonificación)";
        public const string V0018 = "V0018:La propiedad 'indTransferenciaGratuita' solo puede ser verdadera cuando todos los detalles del comprobante tienen un código de afectación del IGV no oneroso";
        public const string V0019 = "V0019:El valor no existe el catalogo N° 09";
        public const string V0020 = "V0020:El valor no existe el catalogo N° 10";
        public const string V0021 = "V0021:Las notas de crédito motivo 13 (Ajustes – montos y/o fechas de pago), deben tener 'impoteTotal' igual a cero";
        public const string V0022 = "V0022:El valor de la propiedad debe ser cero, para notas de crédito motivo 13 (Ajustes – montos y/o fechas de pago)";
        public const string V0023 = "V0023:El valor de la propiedad 'tasaIsc' no es válido, si el ítem no está afecto al ISC asigne 0 a la propiedad 'montoIsc'";
        public const string V0024 = "V0024:El valor de la propiedad 'codSistemaCalculoIsc' no es válido, si el ítem no está afecto al ISC asigne 0 a la propiedad 'montoIsc'";
        public const string V0025 = "V0025:Documento de identidad del adquirente debe ser '1' o '6' cuando la operación no es de exportación";
        public const string V0026 = "V0026:Existe al menos un ítem con 'codAfectacionIgv'= 40 y la propiedad 'codigoTipoOperacion' no es de exportación";
        public const string V0027 = "V0027:Solo se permiten los valores: '01', '03', '07' y '08'";
        public const string V0028 = "V0028:El valor no existe el catalogo N° 02";
        public const string V0029 = "V0029:El valor no existe el catalogo N° 03";
        public const string V0030 = "V0030:El valor no existe el catalogo N° 07";
        public const string V0031 = "V0031:Solo se permite máximo 3 vehículos por GRE (un principal y dos secundarias)";
        public const string V0032 = "V0032:Solo se permite máximo 3 conductores por GRE (uno principal y dos secundarios)";
        public const string V0033 = "V0033:No debe ingresar información de conductores cuando existe el indicador de traslado en vehículos de categoría M1 o L, Cod. SUNAT 3455 y 3456";
        public const string V0034 = "V0034:Se debe consignar los datos del transportista para una operación de Transporte Publico cuando no existe el indicador de traslado en vehículos de categoría M1 o L, Cod. SUNAT 2561,2558 y 2563";
        public const string V0035 = "V0035:No debe ingresar informacion adicional de vehiculos (registros y/o autorizaciones), Cod. SUNAT 3452 y 3454";
        public const string V0036 = "V0036:Cantidad no puede ser cero (0)";
        public const string V0037 = "V0037:La propiedad no cumple con el formato requerido por SUNAT";
        public const string V0038 = "V0038:El codigo de moneda de ser PEN";

        //Mensajes genericos para validar una propiedad
        public const string V0101 = "V0101:El valor de la propiedad deber ser NULL";
        public const string V0102 = "V0102:El valor de la propiedad es requerido";

        /// <summary>
        /// Todo error de calculo usa este codigo
        /// </summary>
        public const string V2000 = "V2000:Error de cálculo";

        #region Codigos de la libreria que se puede ignorar(comienzan con V4XXX)

        public const string V4008 = "V4008:La propiedad no se aplica a Notas de crédito o débito";
        public const string V4018 = "V4018:La propiedad 'informacionPago' solo es requerido para facturas y notas de crédito motivo 13";

        #endregion
    }
}
