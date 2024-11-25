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
    internal class CPEFactura2
    {
        /// <summary>
        /// Este ejemplo genera el XML para una factura gratuita sacado de la pagina 98 del manual oficial de SUNAT
        /// URL: https://drive.google.com/file/d/15EdGzxHUC0vwclVxNjjYnPU0FyKR2x-a/view?usp=sharing
        /// la unicas diferencias son el emisor, receptor, codigo de establecimiento y serie-numero
        /// </summary>
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
                    codigoProductoSunat="52161505",
                    nombre = "TELEVISOR PLASMA DE 42”, MARCA “RCA”",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario = 0,
                    precioVentaUnitario = 1250,
                    valorVenta = 1250,
                    montoBaseIGV = 1250,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "35",//Catalogo N° 07
                    sumatoriaImpuestos = 0
                }
            };

            //Forma de pago al contado
            var _informacionPago = new InformacionPagoType()
            {
                formaPago = FormaPagoType.Contado
            };

            //Cuerpo de una factura
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                informacionPago = _informacionPago,
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "01",//Catalogo N° 01
                serie = "F001",
                numero = 2,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGratuitas = 1250,
                sumatoriaIGV = 0,
                sumatoriaImpuestos = 0,
                valorVenta = 0,
                precioVenta = 0,
                importeTotal = 0,
                indTransferenciaGratuita = true //Esto es opcional
            };

            return _cpe;
        }
    }
}
