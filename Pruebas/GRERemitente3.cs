// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.GRE;
using System;
using System.Collections.Generic;

namespace Pruebas
{
    internal class GRERemitente3
    {
        /// <summary>
        /// Guia de remision Remitente con modalidad de transporte Privado no incluir informacion de vehiculo ni conductor (M1 o L) 
        /// </summary>
        public static GREType GetDocumento()
        {
            //Al ser una guia de remision remitente usamos los datos del emisor
            var _remitente = new InfoRemitenteType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20606433094",
                nombre = "GASPERSOFT EIRL"
            };

            //Los datos del destinatario
            var _destinatario = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "1",
                numeroDocumentoIdentificacion = "40950090",
                nombre = "BERTA ATENCIA LLANOS"
            };

            //detalles a transportar
            var _detalles = new List<ItemGREType>()
            {
                new ItemGREType()
                {
                    nombre= "JUEGO DE MESA",
                    unidadMedida= "NIU",
                    cantidad = 2
                }
            };

            var _gre = new GREType()
            {
                tipoGuia = "09",
                serie = "T001",
                numero = 3,
                fechaEmision = DateTime.Now,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                remitente = _remitente,
                destinatario = _destinatario,
                datosEnvio = new DatosEnvioGREType()
                {
                    motivoTraslado = "01",//Catalogo N° 20
                    descripcionMotivoTraslado = "VENTA DE PRODUCTOS",
                    pesoBrutoTotalBienes = 1,
                    unidadMedidaPesoBruto = "KGM",//Catalogo N° 03
                    modalidadTraslado = "02",//Catalogo N° 18
                    fechaInicioTraslado = DateTime.Now.Date.AddDays(1),
                    puntoLlegada = new InfoDireccionGREType()
                    {
                        ubigeo = "250101",
                        direccion = "KM 86 (PUNTO DE VENTA) CARRETERA FEDERICO BASADRE KM 86 MZ B LOTE 2 - UCAYALI - CORONEL PORTILLO - CALLERIA"
                    },
                    puntoPartida = new InfoDireccionGREType()
                    {
                        ubigeo = "150115",
                        direccion = "AV. 28 DE JULIO 1275 - 1281, LA VICTORIA - LIMA - LIMA - LIMA - LA VICTORIA",
                    },
                    indicadoresGRERemitente = new IndicadoresGRERemitenteType()
                    {
                        indTrasladoVehiculoM1L = true
                    }
                },
                detalles = _detalles
            };

            return _gre;
        }
    }
}
