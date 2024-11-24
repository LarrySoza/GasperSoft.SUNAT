// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    internal class CPENotaCredito1
    {
        /// <summary>
        /// Nota de credito Motivo 13 para modificar las cuotas
        /// </summary>
        /// <returns>CPEType con informacion de factura</returns>
        public static CPEType GetDocumento()
        {
            //Informacion del Receptor
            var _adquirente = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20415932376",
                nombre = "COCA-COLA SERVICIOS DE PERU S.A",
                direccion = "AV. REPÚBLICA DE PANAMÁ NRO. 4050 URB. LIMATAMBO"
            };

            //Como ejemplo un unico de datalle de S / 100 + IGV
            var _detalles = new List<ItemCPEType>()
            {
                new ItemCPEType()
                {
                    codigoProducto = "00001",
                    nombre = "PRODUCTO DE PRUEBA",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario=0,
                    precioVentaUnitario = 0,
                    valorVenta = 0,
                    montoBaseIGV = 0,
                    montoIGV = 0,
                    tasaIGV = 18,//Obligatorio incluir una tasa valida en el documento XML
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                }
            };

            //Esta propiedad es la principal diferencia con una factura/boleta(estos documentos no se debe asignar la propiedad) 
            //Sunat solo acepta un unico documento de referencia sin embargo el estandar UBL soporta una lista de motivos
            var _motivosNota = new List<MotivoNotaType>()
            {
                new MotivoNotaType()
                {
                    fechaEmision = new DateTime(2024,11,19),
                    tipoDocumento = "01",
                    serie = "F001",
                    numero = 1,
                    tipoNota = "13",//Catalogo N° 09
                    sustento = "AMPLIACION DE LA FECHA DE PAGO"
                }
            };

            //Previamente se mando la Factura F001-1 con una cuota de 118 a 30 dias
            //Ahora se modificara 2 cuotas de 59 de 30 dias cada una
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Credito,
                montoPendientePago = 118,
                cuotas = new List<CuotaType>()
                {
                    new CuotaType()
                    {
                        fechaPago = new DateTime(2024,12,19),
                        monto = 59
                    },
                    new CuotaType()
                    {
                        fechaPago = new DateTime(2024,12,19),
                        monto = 59
                    }
                }
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoEstablecimiento = "0000",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "07",//Catalogo N° 01
                serie = "F001",
                numero = 1,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 0,
                sumatoriaIGV = 0,
                sumatoriaImpuestos = 0,
                valorVenta = 0,
                precioVenta = 0,
                importeTotal = 0,
                motivosNota = _motivosNota
            };

            return _cpe;
        }
    }
}
