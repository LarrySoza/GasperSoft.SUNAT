// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class CPEBoleta4
    {
        /// <summary>
        /// Este es un ejemplo se emite una boleta de venta a un trabajador de la empresa por la entrega gratuita de un Televisor, 
        /// la empresa asumiera el IGV, el televisor tiene un precio de venta de S/ 1250
        /// </summary>
        public static CPEType GetDocumento()
        {
            //Informacion del Receptor
            var _adquirente = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "1",
                numeroDocumentoIdentificacion = "45288569",
                nombre = "VELEZMORO SOZA JHON LARRY"
            };

            //Tomar en cuentas que todo lo unitario soportan hasta 10 decimales para asegurar la presión de cálculo
            //según los manuales oficiales de SUNAT
            var _detalles = new List<ItemCPEType>()
            {
                /* Cuando un producto tiene un codigo de afectacion gratuito grabado(Comienzan con "1" pero no es "10"),
                 * se debe informar el IGV y la empresa asumira el pago de este tributo, para el ejemplo se usa el 
                 * codigo de afectacion "16"(RETIRO POR ENTREGA A TRABAJADORES)
                 * 
                */
                new ItemCPEType()
                {
                    nombre = "TELEVISOR PLASMA DE 42”, MARCA “RCA”",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 0,
                    precioVentaUnitario = 1059.3220338983m,
                    valorVenta = 1059.32m,
                    montoBaseIGV = 1059.32m,
                    montoIGV = 190.68m,
                    tasaIGV = 18,
                    codAfectacionIGV = "16",//Catalogo N° 07
                    sumatoriaImpuestos = 0m
                }
            };

            //La propiedad donde se colocar el IGV de las operaciones gratuitas grabadas es "sumatoriaIGVGratuitas"
            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "03",//Catalogo N° 01
                serie = "B001",
                numero = 4,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 0m,
                totalOperacionesGratuitas = 1059.32M,
                sumatoriaIGV = 0,
                sumatoriaIGVGratuitas = 190.68m,
                sumatoriaICBPER = 0,
                sumatoriaImpuestos = 0m,
                valorVenta = 0m,
                precioVenta = 0m,
                importeTotal = 0m
            };

            return _cpe;
        }
    }
}
