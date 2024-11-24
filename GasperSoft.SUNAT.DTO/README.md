# GasperSoft.SUNAT.DTO

Objetos (DTOs), usados por la librería GasperSoft.SUNAT.UBL.dll para la generación de los XML requeridos por SUNAT.

## EmisorType ##
Se usa para el llenado de los datos de emisor
```C#
var _emisor = new EmisorType()
{
    ruc = "20606433094",
    razonSocial = "GASPERSOFT EIRL",
    codigoUbigeo = "150125",
    direccion = "CAL. LOS LIRIOS INT. 20 LT. 1 MZ. B URB. MONTEGRANDE",
    departamento = "LIMA",
    provincia = "LIMA",
    distrito = "PUENTE PIEDRA"
};
```
## InfoPersonaType ##
Se usa para el llenado de los datos de una persona que podría ser un cliente, proveedor, comprador, pagador del flete etc. 
```C#
var _adquirente = new InfoPersonaType()
{
    tipoDocumentoIdentificacion = "6",
    numeroDocumentoIdentificacion = "20415932376",
    nombre = "COCA-COLA SERVICIOS DE PERU S.A",
    direccion = "AV. REPÚBLICA DE PANAMÁ NRO. 4050 URB. LIMATAMBO"
};
```
## InformacionPagoType ##
Se usa para el llenado de información de la forma de pago 
```C# 
//Forma de pago al contado
var _informacionPago = new InformacionPagoType()
{
    formaPago = FormaPagoType.Contado
};

//Forma de pago al credito en una cuota
var _informacionPago = new InformacionPagoType()
{
    formaPago = FormaPagoType.Credito,
    montoPendientePago = 118,
    cuotas = new List<CuotaType>()
    {
        new CuotaType()
        {
            fechaPago = DateTime.Now.AddDays(30).Date,
            monto = 118
        }
    }
};
```

## ItemCPEType ##
Se usa para el llenado del detalle de un CPE
```C#
var _item = new ItemCPEType()
{
    codigoProducto = "00001",
    nombre = "PRODUCTO DE PRUEBA",
    unidadMedida = "NIU",
    cantidad = 1,
    valorVentaUnitario=100,
    precioVentaUnitario = 118,
    valorVenta = 100,
    montoBaseIGV = 100,
    montoIGV = 18,
    tasaIGV = 18,
    codAfectacionIGV = "10",//Catalogo N° 7
    sumatoriaImpuestos = 18
};

//Posteriormente se agrega el detalle a una lista de detalles
var _detalles = new List<ItemCPEType>();
_detalles.Add(_item);
```

## ItemCPEType ##
Se usa para el llenado del detalle de un CPE
```C#
var _item = new ItemCPEType()
{
    codigoProducto = "00001",
    nombre = "PRODUCTO DE PRUEBA",
    unidadMedida = "NIU",
    cantidad = 1,
    valorVentaUnitario=100,
    precioVentaUnitario = 118,
    valorVenta = 100,
    montoBaseIGV = 100,
    montoIGV = 18,
    tasaIGV = 18,
    codAfectacionIGV = "10",//Catalogo N° 7
    sumatoriaImpuestos = 18
};

//Aqui podemos agregar el detalle a una lista de detalles
var _detalles = new List<ItemCPEType>();
_detalles.Add(_item);
```
## CPEType ##
Se usa para el llenado de los datos de un CPE
```C#
var _cpe = new CPEType()
{
    codigoTipoOperacion = "0101",
    codigoEstablecimiento = "0000",
    ordenCompra = "000055",
    informacionPago = _informacionPago,
    fechaEmision = DateTime.Now.Date,
    horaEmision = DateTime.Now.ToString("HH:mm:ss"),
    tipoDocumento = "01",
    serie = "F001",
    numero = 2,
    adquirente = _adquirente,
    detalles = _detalles,
    codMoneda = "PEN",
    totalOperacionesGravadas = 100,
    sumatoriaIGV = 18,
    tasaIGV = 18,
    sumatoriaImpuestos = 18,
    valorVenta = 100,
    precioVenta = 118,
    importeTotal = 118,
};
```




