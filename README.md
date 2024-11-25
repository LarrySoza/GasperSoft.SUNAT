# GasperSoft.SUNAT

Un conjunto de librerías para .NET que permite la fácil generación de los XML de documentos electrónicos, estas mismas librerías son las que uso en mi plataforma de producción que ofrezco a mis clientes, por lo que están en constante actualización.

# Como Funciona
Podría afirmar que existe 4 principales formas de generar XML en .Net: 
-	**Primer Método:** Usar plantillas o escribir el XML con StringBuilder usando variables que después son remplazadas por datos, este enfoque es el más sencillo, pero podría generar errores.
-	**Segundo Método:** Generar clases clases (Invoice, CreditNote, DebitNote, DespatchAdvice etc) con sus respectivas propiedades heredando la interface IXmlSerializable y escribiendo el método WriteXml que finalmente dará como resultado el XML, este enfoque es incluso mucho más complejo y difícil de mantener.
-	**Tercer Método:** Generar el XML usando XmlDocument que debido a la complejidad de la estructura es mucho más difícil que los dos anteriores métodos. 
-	**Cuarto Método:** Generar código desde los XSD oficiales del estándar UBL/SUNAT, usar estas clases generadas asignarle los datos requeridos y después ser serializados usando XmlSerializer. **Este es el método usando en este proyecto gracias a [UblXsdToCS]( https://github.com/LarrySoza/UblXsdToCS)**, por lo que implementar cualquier atributo requerido parte del estándar UBL/SUNAT es relativamente sencillo, para ello se utiliza el proyecto **GasperSoft.SUNAT.DTO** que contiene clases (CPEType, GREType, ResumenDiarioV2Type etc) que encapsula los requerimientos de SUNAT y después son convertidos a objetos generados desde los XSD usando la librería **GasperSoft.SUNAT.UBL**, finalmente este objetos son serializados usando XmlSerializer.

>[!NOTE] 
>El Proyecto **GasperSoft.SUNAT** contiene código para validar y Serializar los objetos GasperSoft.SUNAT.DTO, podrían referenciarlo directamente en su proyecto o hacer su propia implementación de la serialización y validación.
>Pueden encontrar los paquetes publicados en [Nuget]( https://www.nuget.org/packages/GasperSoft.SUNAT.UBL) e instalarla directamente la librería desde ahí, pero tome en cuenta que los ajustes de validaciones y cambios primero se actualizan en este repositorio.

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

# Proyecto "Pruebas"
- Se necesita un certificado de prueba que se puede generar de manera gratuita en https://llama.pe/certificado-digital-de-prueba-sunat), actualmente se tienen los siguientes ejemplos:

  - [FACTURA CREDITO](/Xml/20606433094-01-F001-1.xml)
  - [FACTURA GRATUITA](/Xml/20606433094-01-F001-2.xml)
  - [FACTURA CONTADO CON DETRACCION](/Xml/20606433094-01-F001-3.xml)
  - [NOTA CREDITO MOTIVO 13](/Xml/20606433094-07-F001-1.xml)
  - [GUIA REMISION REMITENTE - Transporte Publico](/Xml/20606433094-09-T001-1.xml)
  - [GUIA REMISION REMITENTE - Transporte Privado (Vehiculo y Conductor)](/Xml/20606433094-09-T001-2.xml)
  - [GUIA REMISION REMITENTE - Transporte Privado (M1 o L)](/Xml/20606433094-09-T001-3.xml)
  - [GUIA REMISION TRANSPORTISTA](/Xml/20606433094-31-V001-1.xml)
  - [RESUMEN DIARIO DE BOLETAS - INFORMAR](/Xml/20606433094-RC-20241125-1.xml)
  - [RESUMEN DIARIO DE BOLETAS - DAR DE BAJA](/Xml/20606433094-RC-20241125-2.xml)
  - [COMUNICACION DE BAJA (SOLO FACTURAS)](/Xml/20606433094-RA-20241125-1)

# Validar XML generado
- Se puede validar el XML generado en https://probar-xml.nubefact.com, Sin embargo debe considerar que el solo hecho de copiar y pegar en esta página podría adulterar el contenido del XML y tener un mensaje de error 2335(Como en el ejemplo de "FACTURA CONTADO CON DETRACCION"), de ser ese el caso puede marcar la opcion Firmar.

![ValidarXml](https://github.com/user-attachments/assets/7f9edb32-7c83-4c02-9c8f-f47972ed8a49)

>[!NOTE] 
>A la fecha 24-11-2024 la pagina de nubefact no valida los XML de Guia Transportista

# Envio a SUNAT
- De momento este proyecto se enfoca exclusivamente en la generación de los XML, puede encontrar código de envió a SUNAT en OpenInvoice https://github.com/erickorlando/openinvoiceperu

## Asesoría y Soporte ##

Siéntase cómodo de escribir a it@gaspersoft.com para cualquier característica requerida o bugs.
