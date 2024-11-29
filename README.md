# GasperSoft.SUNAT
[![NuGet Version](https://img.shields.io/nuget/v/GasperSoft.SUNAT)](https://www.nuget.org/packages/GasperSoft.SUNAT)
[![GitHub License](https://img.shields.io/github/license/LarrySoza/GasperSoft.SUNAT)](/LICENSE.txt)

Librerías .NET que permite generar los XML de la **Facturación Electrónica** en Perú, estas mismas librerías son las que uso en mi plataforma de producción que ofrezco a mis clientes, por lo que están en constante actualización.

# Características #
- Generación de XML de los siguientes documentos electrónicos:
  - Facturas
  - Boletas
  - Notas de Crédito
  - Notas de Débito
  - Resumen Diario de Boletas
  - Comunicaciones de Baja
  - Retenciones
  - Guías de Remisión Remitente
  - Guías de Remisión Transportista

# Como Funciona
Utiliza código generado desde los **XSD oficiales** del estándar UBL/SUNAT **gracias a [UblXsdToCS]( https://github.com/LarrySoza/UblXsdToCS)**, estas clases contienen la estructura completa del estándar UBL/SUNAT, entonces implementar cualquier atributo adicional requerido parte de SUNAT es relativamente sencillo. Sin embargo dado la gran cantidad de propiedades que existen en el estándar UBL, en este proyecto se emplean objetos intermedios más sencillos que cumplen con todos los requerimientos de SUNAT, estas clases están definidas en **GasperSoft.SUNAT.DTO.dll** (CPEType, CREType, GREType, ResumenDiarioV2Type, ComunicacionBajaType), posteriormente después de asignar las propiedades correspondientes son convertidos usando la librería **GasperSoft.SUNAT.UBL.dll** a clases generadas desde los XSD (InvoiceType, DespatchAdviceType, SummaryDocumentsType, VoidedDocumentsType, RetentionType), finalmente son serializados a XML y firmado utilizando métodos definidos en **GasperSoft.SUNAT.dll**.

>[!NOTE] 
>El Proyecto **GasperSoft.SUNAT** es compatible con net462, net472, net481, netstandard2.0 y net8.0. **GasperSoft.SUNAT.DTO** y **GasperSoft.SUNAT.UBL** lo son con net35, net452, net462, net472, net481, netstandard2.0, net6.0, net7.0 y net8.0.
**GasperSoft.SUNAT.DTO.dll** y **GasperSoft.SUNAT.UBL.dll** no dependen de **GasperSoft.SUNAT.dll**, sin embargo es esta la librería que permite validar, serializar y firmar los XML, si necesita compatibilidad con net35 o un framework no soportado deberá implementar sus propios métodos.

# Como se usa
- En el proyecto pruebas encontrara ejemplos de código de como generar y firmar los XML, se necesita un certificado digital, puede generar uno para pruebas de manera gratuita en [NUBEFACT](https://llama.pe/certificado-digital-de-prueba-sunat)(sin valor legal), actualmente se tienen los siguientes ejemplos:

  - [BOLETA DE VENTA GRAVADA CON DOS ÍTEMS Y UNA BONIFICACIÓN](/Xml/20606433094-03-B001-1.xml) - [Pagina 60 Manual SUNAT](/ManualesSunat/BoletaDeVentaElectronica2.1.pdf)
  - [BOLETA CON ICBPER - COBRANDO BOLSA](/Xml/20606433094-03-B001-2.xml)
  - [BOLETA CON ICBPER - REGALANDO BOLSA](/Xml/20606433094-03-B001-3.xml)
  - [FACTURA CREDITO (CUOTAS)](/Xml/20606433094-01-F001-1.xml)
  - [FACTURA GRATUITA](/Xml/20606433094-01-F001-2.xml) - [Pagina 98 Manual SUNAT](/ManualesSunat/FacturaElectronica2.1.pdf)
  - [FACTURA CONTADO CON DETRACCION](/Xml/20606433094-01-F001-3.xml)
  - [FACTURA CON 4 ÍTEMS Y UNA BONIFICACIÓN](/Xml/20606433094-01-F001-4.xml) - [Pagina 77 Manual SUNAT](/ManualesSunat/FacturaElectronica2.1.pdf)
  <!--- [FACTURA CON 2 ÍTEMS E ISC](/Xml/20606433094-01-F001-5.xml) - [Pagina 88 Manual SUNAT](/ManualesSunat/FacturaElectronica2.1.pdf)-->
  - [FACTURA CON ANTICIPOS - CON MONTO PENDIENTE DE PAGO](/Xml/20606433094-01-F001-6.xml)
  - [FACTURA CON ANTICIPOS - MONTO TOTAL EN CERO](/Xml/20606433094-01-F001-7.xml)
  - [FACTURA CON RETENCION](/Xml/20606433094-01-F001-8.xml)
  - [FACTURA CON PERCEPCION](/Xml/20606433094-01-F001-9.xml)
  - [NOTA CREDITO MOTIVO 13](/Xml/20606433094-07-F001-1.xml)
  - [GUIA REMISION REMITENTE - Transporte Publico](/Xml/20606433094-09-T001-1.xml)
  - [GUIA REMISION REMITENTE - Transporte Privado (Vehiculo y Conductor)](/Xml/20606433094-09-T001-2.xml)
  - [GUIA REMISION REMITENTE - Transporte Privado (M1 o L)](/Xml/20606433094-09-T001-3.xml)
  - [GUIA REMISION TRANSPORTISTA](/Xml/20606433094-31-V001-1.xml)
  - [RESUMEN DIARIO DE BOLETAS - INFORMAR](/Xml/20606433094-RC-20241125-1.xml)
  - [RESUMEN DIARIO DE BOLETAS - DAR DE BAJA](/Xml/20606433094-RC-20241125-2.xml)
  - [COMUNICACION DE BAJA (SOLO FACTURAS)](/Xml/20606433094-RA-20241125-1.xml)
  - [RETENCION FACTURA SOLES](/Xml/20606433094-20-R001-1.xml)
  - [RETENCION FACTURA DOLARES - CON TIPO DE CAMBIO](/Xml/20606433094-20-R001-2.xml)
  - [REVERSION (BAJAS DE RETENCIONES)](/Xml/20606433094-RR-20241127-1.xml)

>[!NOTE] 
>El ejemplo **"FACTURA CON 4 ÍTEMS Y UNA BONIFICACIÓN - Pagina 77 Manual SUNAT"** es el ejemplo mas completo de todos porque combina el uso de productos grabados, exonerados, bonificaciones y descuentos por ítem y global, por lo que recomiendo darle una observación detalla. La clase **ValidadorCPE.cs** del proyecto **GasperSoft.SUNAT** debería poder ayudarte a corregir errores de cálculo, si no es el caso y pudiste generar el XML pero no paso las validaciones de SUNAT te agradecería que me mandes un ejemplo (como los que se implementan en el proyecto Pruebas) con los datos que llenas y el XML generado a [it@gaspersoft.com](mailto:it@gaspersoft.com), me ayudarías a colocar mas validaciones que no permitan generar XMLs con errores de calculo.

# Validar XML generado
- Se puede validar el XML generado en [NUBEFACT](https://probar-xml.nubefact.com), Sin embargo debe considerar que el solo hecho de copiar y pegar en esta página podría adulterar el contenido del XML y tener un mensaje de error 2335(Como en el ejemplo de "FACTURA CONTADO CON DETRACCION"), de ser ese el caso puede marcar la opcion Firmar.

![ValidarXml](https://github.com/user-attachments/assets/7f9edb32-7c83-4c02-9c8f-f47972ed8a49)

>[!NOTE] 
>A la fecha 24-11-2024 la pagina de nubefact no valida los XML de Guia Transportista

# Envio a SUNAT
- De momento este proyecto se enfoca exclusivamente en la generación de los XML, puede encontrar código de envió a SUNAT en [OpenInvoice](https://github.com/erickorlando/openinvoiceperu)

>[!NOTE] 
>La razón para no publicar código de envió es que busco la implementación usando un código compatible con NET Framework y NET 8, y que permita asignar la URL y credenciales, actualmente esto solo lo tengo implementado en NET Framework. En los próximos meses posiblemente el código de envío sea agregado al proyecto **GasperSoft.SUNAT**

## Asesoría y Soporte ##

Siéntase cómodo de escribir a [it@gaspersoft.com](mailto:it@gaspersoft.com) para cualquier característica requerida o bugs.
