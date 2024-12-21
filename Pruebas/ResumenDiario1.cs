// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.Resumen;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class ResumenDiario1
    {
        /// <summary>
        /// Devuelve un Resumen diario con informacion de boletas (estadoItem de cada detalle es "1")
        /// para este ejemplo estamos mandando:
        ///     Una boleta B001-1 de 1000 soles+IGV
        ///     Una Nota de credito BN01-1 por 100+IGV que afecta a la boleta B001-1( se envia la referencia a B001-1 para no tener error 2513)
        /// 
        /// Nota:Montos menores a 700 soles no es requerido informacion del cliente se puede mandar:
        ///     tipoDocumentoIdentificacionAdquirente = "-"
        ///     numeroDocumentoIdentificacionAdquirente = "00000000"
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
                        estadoItem = "1",
                        tipoDocumentoIdentificacionAdquirente = "1",
                        numeroDocumentoIdentificacionAdquirente = "45288569",
                        codMoneda = "PEN",
                        totalOperacionesGravadas = 1000,
                        totalOperacionesExoneradas = 0,
                        totalOperacionesInafectas = 0,
                        totalOperacionesGratuitas = 0,
                        sumatoriaOtrosCargos = 0,
                        sumatoriaISC = 0,
                        sumatoriaIGV = 180,
                        sumatoriaICBPER = 0,
                        sumatoriaOTH = 0,
                        importeTotal = 1180
                    },
                    new ItemResumenDiarioV2Type()
                    {
                        secuencia = 2,
                        tipoDocumento = "07",
                        serie = "BN01",
                        numero = 1,
                        estadoItem = "1",
                        tipoDocumentoIdentificacionAdquirente = "1",
                        numeroDocumentoIdentificacionAdquirente = "45288569",
                        codMoneda = "PEN",
                        totalOperacionesGravadas = 100,
                        totalOperacionesExoneradas = 0,
                        totalOperacionesInafectas = 0,
                        totalOperacionesGratuitas = 0,
                        sumatoriaOtrosCargos = 0,
                        sumatoriaISC = 0,
                        sumatoriaIGV = 18,
                        sumatoriaICBPER = 0,
                        sumatoriaOTH = 0,
                        importeTotal = 118,
                        tipoDocumentoModifica = "03",//Solo Nota de credito/debito
                        serieModifica = "B001",//Solo Nota de credito/debito
                        numeroModifica = 1 //Solo Nota de credito/debito
                    }
                }
            };

            //Cada resumen puede contener 500 detalles(Boletas/notas) y por cada bloque del dia
            //se va asignado un correlativo 1,2,3.....etc
            int _correlativo = 1;

            _resumen.id = $"RC-{_resumen.fechaGeneracion:yyyyMMdd}-{_correlativo}";

            return _resumen;
        }
    }
}
