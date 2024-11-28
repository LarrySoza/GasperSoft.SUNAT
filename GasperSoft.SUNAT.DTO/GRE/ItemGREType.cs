// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.GRE
{
    public class ItemGREType
    {
        /// <summary>
        /// El codigo del Articulo-Servicio o bien prestado
        /// </summary>
        public string codigoProducto { get; set; }

        /// <summary>
        /// solo se permite GTIN-8, GTIN-12, GTIN-13, GTIN-14
        /// </summary>
        public string tipoCodigoProductoGS1 { get; set; }

        /// <summary>
        /// UBL 2.1
        /// El codigo del Articulo-Servicio GS1 
        /// </summary>
        public string codigoProductoGS1 { get; set; }

        /// <summary>
        /// UBL 2.1
        /// Código del producto de acuerdo al estándar internacional de la ONU denominado:
        /// United Nations Standard Products and Services Code - Código de productos y servicios
        /// estándar de las Naciones Unidas - UNSPSC v14_0801, a que hace referencia el catálogo N° 25
        /// </summary>
        public string codigoProductoSunat { get; set; }

        /// <summary>
        /// Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando el nombre y las características, 
        /// tales como marca del bien vendido o cedido en uso.
        /// </summary>
        public string nombre { get; set; }

        /// <summary>
        /// Se consigna la unidad de medida de los bienes por ítem, para el caso peruano se usará el catalogo internacional UN/ECE rec 20- Unit Of Measure.
        /// No será necesario colocar la unidad de medida si ésta es “NIU”(unidad) 0 “ZZ”.
        /// </summary>
        public string unidadMedida { get; set; }

        /// <summary>
        /// Se consignará la cantidad de productos vendidos o servicios prestados en la operación. En el caso de retiro de bienes, 
        /// se consignará la cantidad de bienes transferidos a titulo gratuito.Cuando se trate de servicios o cualquier otra operación 
        /// no cuantificable se deberá consignar el valor uno(1)
        /// </summary>
        public decimal cantidad { get; set; }

        /// <summary>
        /// Indica si el item es un bien normalizado
        /// </summary>
        public bool esBienNormalizado { get; set; }

        /// <summary>
        /// Catalogo 62
        /// </summary>
        public string partidaArancelaria { get; set; }

        /// <summary>
        /// Numero de la Declaración Aduanera de Mercancías (DAM) o el formato de la  Declaración Simplificada(DS).
        /// El formato es {codigo aduana}-{año}-{tipo}-{numero}
        /// Ejemplo:
        /// 235-2022-10-081049
        /// </summary>
        public string numeroDeclaracionAduanera { get; set; }

        /// <summary>
        /// El numero de serie de la Declaración Aduanera en la que figura el Item 
        /// </summary>
        public string numeroSerieEnDeclaracionAduanera { get; set; }

        #region Campos Adiconales(no existen referencias, pero se usan al momento de mandar la lista de detalles(List<ItemCPEType>) al pdf)

        public string adicional1 { get; set; }

        public string adicional2 { get; set; }

        public string adicional3 { get; set; }

        public string adicional4 { get; set; }

        public string adicional5 { get; set; }

        #endregion
    }
}
