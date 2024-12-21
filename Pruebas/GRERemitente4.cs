// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.GRE;
using System.Collections.Generic;

namespace Pruebas
{
    internal class GRERemitente4
    {
        /// <summary>
        /// GUIA REMITENTE CON DATOS ADICIONALES EN EL XML
        /// </summary>
        public static GREType GetDocumento()
        {
            //Voy usar la misma informacion de la GUIA T001-3 (archivo GRERemitente3.cs)
            var _gre = GRERemitente3.GetDocumento();

            //Solo le cambiare el numero
            _gre.numero = 4;

            //Agregare Informacion Adicional y que se agregue al XML
            var _informacionAdicional = new List<DatoAdicionalType>()
            {
                new DatoAdicionalType()
                {
                    codigo = "PrecioReferencialMercaderia",
                    valor = "S/ 3000.00"
                },
                new DatoAdicionalType()
                {
                    codigo = "TipoServicio",
                    valor = "Premium"
                },
                new DatoAdicionalType()
                {
                    codigo = "TarifaFlete",
                    valor = "S/ 200.00"
                }
            };

            _gre.informacionAdicional = _informacionAdicional;

            //Esta es la propiedad que hace que se incluya dicha informacion en el XML
            _gre.informacionAdicionalEnXml = true;

            return _gre;
        }
    }
}
