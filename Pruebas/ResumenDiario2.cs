// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.Resumen;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class ResumenDiario2
    {
        /// <summary>
        /// Devuelve un resumen diario para dar de baja boletas(estadoItem de cada detalle es "1")
        /// para este ejemplo estamos mandando los datos del ejemplo "RESUMEN DIARIO DE BOLETAS - INFORMAR" con montos en 0
        ///     Una boleta B001-1
        ///     Una Nota de credito BN01-1
        /// Nótese que no fue necesario enviar información del cliente ni documentos de referencia pero si muy importante la Moneda
        /// </summary>
        public static ResumenDiarioV2Type GetDocumento()
        {
            var _resumen = new ResumenDiarioV2Type()
            {
                fechaEmisionDocumentos = DateTime.Now,
                fechaGeneracion = DateTime.Now,
                detalles = new List<ItemResumenDiarioV2Type>()
                {
                    new ItemResumenDiarioV2Type()
                    {
                        secuencia = 1,
                        tipoDocumento = "03",
                        serie = "B001",
                        numero = 1,
                        estadoItem = "3",
                        tipoDocumentoIdentificacionAdquirente = "-",
                        numeroDocumentoIdentificacionAdquirente = "0",
                        codMoneda = "PEN",
                        totalOperacionesGravadas = 0,
                        totalOperacionesExoneradas = 0,
                        totalOperacionesInafectas = 0,
                        totalOperacionesGratuitas = 0,
                        sumatoriaOtrosCargos = 0,
                        sumatoriaISC = 0,
                        sumatoriaIGV = 0,
                        sumatoriaICBPER = 0,
                        sumatoriaOTH = 0,
                        importeTotal = 0
                    },
                    new ItemResumenDiarioV2Type()
                    {
                        secuencia = 2,
                        tipoDocumento = "07",
                        serie = "BN01",
                        numero = 1,
                        estadoItem = "3",
                        tipoDocumentoIdentificacionAdquirente = "-",
                        numeroDocumentoIdentificacionAdquirente = "0",
                        codMoneda = "PEN",
                        totalOperacionesGravadas = 0,
                        totalOperacionesExoneradas = 0,
                        totalOperacionesInafectas = 0,
                        totalOperacionesGratuitas = 0,
                        sumatoriaOtrosCargos = 0,
                        sumatoriaISC = 0,
                        sumatoriaIGV = 0,
                        sumatoriaICBPER = 0,
                        sumatoriaOTH = 0,
                        importeTotal = 0
                    }
                }
            };

            //Cada resumen puede contener 500 detalles(Boletas/notas) y por cada bloque del dia
            //se va asignado un correlativo 1,2,3.....etc
            int _correlativo = 2;

            _resumen.id = $"RC-{_resumen.fechaGeneracion:yyyyMMdd}-{_correlativo}";

            return _resumen;
        }
    }
}
