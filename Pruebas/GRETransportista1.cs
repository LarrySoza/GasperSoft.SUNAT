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
    internal class GRETransportista1
    {
        /// <summary>
        /// Guia de remision Transportista 
        /// </summary>
        /// <returns>>GREType con informacion de una guia remision remitente</returns>
        public static GREType GetDocumento(EmisorType emisor)
        {
            //Los datos de la informacion del transportista son los mismo que del emisor
            var _transportista = new InfoTransportistaType()
            {
                ruc = emisor.ruc,
                razonSocial = emisor.razonSocial,
                registroMTC = "ABCDEFG12345" //de saber el registroMTC colocarlo aqui para evitar la observacion 4391
            };

            var _remitente = new InfoRemitenteType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = "20602712592",
                nombre = "CHOCANO CARGO S.A.C."
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

            var _documentoRelacionado = new DocumentoRelacionadoGREType()
            {
                emisor = new InfoPersonaType()
                {
                    tipoDocumentoIdentificacion = "6",
                    numeroDocumentoIdentificacion = "20602712592",
                    nombre = "CHOCANO CARGO S.A.C."
                },
                tipoDocumento = "01",
                numeroDocumento = "F001-1"
            };

            var _gre = new GREType()
            {
                tipoGuia = "31",
                serie = "V001",
                numero = 1,
                fechaEmision = DateTime.Now,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                remitente = _remitente,
                destinatario = _destinatario,
                transportista = _transportista,
                documentosRelacionados = new List<DocumentoRelacionadoGREType>()
                {
                    _documentoRelacionado
                },
                datosEnvio = new DatosEnvioGREType()
                {
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
