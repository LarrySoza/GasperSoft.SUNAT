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
    internal class GRERemitente1
    {
        /// <summary>
        /// Guia de remision Remitente con modalidad de transporte Publico
        /// </summary>
        /// <returns>>GREType con informacion de una guia remision remitente</returns>
        public static GREType GetDocumento(EmisorType emisor)
        {
            //Al ser una guia de remision remitente usamos los datos del emisor
            var _remitente = new InfoRemitenteType()
            {
                tipoDocumentoIdentificacion = "6",
                numeroDocumentoIdentificacion = emisor.ruc,
                nombre = emisor.razonSocial
            };

            //Los datos del destinatario
            var _destinatario = new InfoPersonaType()
            {
                tipoDocumentoIdentificacion = "1",
                numeroDocumentoIdentificacion = "40950090",
                nombre = "BERTA ATENCIA LLANOS"
            };

            //Informacion del transportista(Empresa publica que realiza el transporte de la mercaderia)
            var _transportista = new InfoTransportistaType()
            {
                ruc = "20602712592",
                razonSocial = "CHOCANO CARGO S.A.C.",
                registroMTC = null //de saber el registroMTC colocarlo aqui para evitar la observacion 4391
            };

            //detalles a transportar
            var _detalles = new List<ItemGREType>()
            {
                new ItemGREType()
                {
                    nombre= "DC COCINA Y MESA CN REPISA AUT DON WERRIN MURRIETA PSJ A KM 86  AST 54",
                    unidadMedida= "NIU",
                    cantidad = 2
                }
            };

            var _gre = new GREType()
            {
                tipoGuia = "09",
                serie = "T001",
                numero = 1,
                fechaEmision = DateTime.Now,
                horaEmision = DateTime.Now.ToString("HH:mm:ss"),
                remitente = _remitente,
                transportista = _transportista,
                destinatario = _destinatario,
                datosEnvio = new DatosEnvioGREType()
                {
                    motivoTraslado = "01",//Catalogo N° 20
                    descripcionMotivoTraslado = "VENTA DE PRODUCTOS",
                    pesoBrutoTotalBienes = 1,
                    unidadMedidaPesoBruto = "KGM",//Catalogo N° 03
                    modalidadTraslado = "01",//Catalogo N° 18
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
                    }
                },
                detalles = _detalles
            };

            return _gre;
        }
    }
}
