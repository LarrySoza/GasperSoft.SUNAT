// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.GRE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    internal class GRERemitente2
    {
        /// <summary>
        /// Guia de remision Remitente con modalidad de transporte Privado y informacion de vehiculo y conductor
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

            var _vehiculoPrincipal = new InfoVehiculoType()
            {
                numeroPlaca = "ABC123"
            };

            var _conductorPrincipal = new InfoConductorType()
            {
                tipoDocumentoIdentificacion = "1",
                numeroDocumentoIdentificacion = "45288569",
                nombres = "JHON LARRY",
                apellidos = "VELEZMORO SOZA",
                licenciaConducir = "Q45288569"
            };

            var _gre = new GREType()
            {
                tipoGuia = "09",
                serie = "T001",
                numero = 1,
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
                    vehiculos = new List<InfoVehiculoType>()
                    {
                        _vehiculoPrincipal
                    },
                    conductores = new List<InfoConductorType>()
                    {
                        _conductorPrincipal
                    },
                    puntoLlegada = new InfoDireccionGREType()
                    {
                        ubigeo = "250101",
                        direccion = "KM 86 (PUNTO DE VENTA) CARRETERA FEDERICO BASADRE KM 86 MZ B LOTE 2 - UCAYALI - CORONEL PORTILLO - CALLERIA"
                    },
                    puntoPartida = new InfoDireccionGREType()
                    {
                        ubigeo = "150115",
                        direccion = "AV. 28 DE JULIO 1275 - 1281, LA VICTORIA - LIMA - LIMA - LIMA - LA VICTORIA",
                    }
                },
                detalles = _detalles
            };

            return _gre;
        }
    }
}
