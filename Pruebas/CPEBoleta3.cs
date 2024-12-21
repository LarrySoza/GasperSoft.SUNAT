// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class CPEBoleta3
    {
        /// <summary>
        /// BOLETA CON ICBPER - REGALANDO BOLSA
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
                new ItemCPEType()
                {
                    nombre = "ARTICULO DE PRUEBA",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 8.4745762712m,
                    precioVentaUnitario = 10,
                    valorVenta = 8.47m,
                    montoBaseIGV = 8.47m,
                    montoIGV = 1.53m,
                    tasaIGV = 18,
                    codAfectacionIGV = "10",//Catalogo N° 07
                    sumatoriaImpuestos = 1.53m
                },
                //Producto Gratuito con un precio referencial de S/ 1, de igual manera se cobra el ICBPER
                new ItemCPEType()
                {
                    nombre = "BOLSA GRANDE",
                    unidadMedida = "NIU",//Catalogo N° 03
                    cantidad = 1,
                    valorVentaUnitario= 0,
                    precioVentaUnitario = 1,
                    valorVenta = 1,
                    montoBaseIGV = 1,
                    montoIGV = 0,
                    tasaIGV = 0,
                    codAfectacionIGV = "31",//Catalogo N° 07
                    tasaUnitariaICBPER = 0.5m,
                    montoICBPER = 0.5m,//cantidad * tasaUnitariaICBPER
                    sumatoriaImpuestos = 0.5m//montoIGV + montoICBPER = 0.15 + 0.5
                },
            };

            var _cpe = new CPEType()
            {
                codigoTipoOperacion = "0101",//Catalogo N° 51
                codigoEstablecimiento = "0000",
                fechaEmision = DateTime.Now.Date,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                tipoDocumento = "03",//Catalogo N° 01
                serie = "B001",
                numero = 3,
                adquirente = _adquirente,
                detalles = _detalles,
                codMoneda = "PEN",//Catalogo N° 02
                totalOperacionesGravadas = 8.47m,
                totalOperacionesGratuitas = 1,
                sumatoriaIGV = 1.53m,
                sumatoriaICBPER = 0.5m,
                sumatoriaImpuestos = 2.03m,
                valorVenta = 8.47m,
                precioVenta = 10.5m,
                importeTotal = 10.5m
            };

            return _cpe;
        }
    }
}
