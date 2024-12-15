// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;

namespace Pruebas
{
    internal class CPEFactura10
    {
        /// <summary>
        /// FACTURA CON DATOS ADICIONALES EN EL XML
        /// </summary>
        public static CPEType GetDocumento()
        {
            //Voy usar la misma informacion de la Factura F001-1 (archivo CPEFactura1.cs)
            var _cpe = CPEFactura1.GetDocumento();

            //Solo le cambiare el numero
            _cpe.numero = 10;

            //Agregare Informacion Adicional y que se agregue al XML
            var _informacionAdicional = new List<DatoAdicionalType>()
            {
                new DatoAdicionalType()
                {
                    codigo = "TipoServicio",
                    valor = "Premium"
                },
                new DatoAdicionalType()
                {
                    codigo = "UsuarioSistema",
                    valor = "Fact001"
                },
                new DatoAdicionalType()
                {
                    codigo = "CodigoInterno",
                    valor = "0501002017012000125"
                }
            };

            _cpe.informacionAdicional = _informacionAdicional;

            //Esta es la propiedad que hace que se incluya dicha informacion en el XML
            _cpe.informacionAdicionalEnXml = true;

            return _cpe;
        }
    }
}
