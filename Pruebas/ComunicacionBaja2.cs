// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.Resumen;

namespace Pruebas
{
    /// <summary>
    /// Comunicacion de baja de un documento de retencion es exactamente igual anular una factura solo
    /// cambia su ID
    /// </summary>
    internal class ComunicacionBaja2
    {
        public static ComunicacionBajaType GetDocumento()
        {
            var _baja = new ComunicacionBajaType()
            {
                fechaBaja = new DateTime(2024, 11, 27),
                fechaGeneracion = new DateTime(2024, 11, 27),
                detalles = new List<ItemComunicacionBajaType>()
                {
                    new ItemComunicacionBajaType()
                    {
                        tipoDocumento = "20",
                        serie = "R001",
                        numero = 1,
                        motivo = "ERROR DE DATOS" //Una breve descripcion del motivo de anulacion
                    },
                    new ItemComunicacionBajaType()
                    {
                        tipoDocumento = "20",
                        serie = "R001",
                        numero = 2,
                        motivo = "ERROR DE DATOS" //Una breve descripcion del motivo de anulacion
                    }
                }
            };

            //Cada baja puede contener 500 detalles por cada bloque del dia
            //se va asignado un correlativo 1,2,3.....etc
            int _correlativo = 1;

            _baja.id = $"RR-{_baja.fechaGeneracion:yyyyMMdd}-{_correlativo}";

            return _baja;
        }
    }
}
