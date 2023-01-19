// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

namespace GasperSoft.SUNAT.DTO.Resumen
{
    public class ItemResumenDiarioV1Type
    {
        /// <summary>
        /// Número de orden del Ítem
        /// </summary>
        public int secuencia { get; set; }

        /// <summary>
        /// El tipo de documento
        /// </summary>
        public string tipoDocumento { get; set; }

        /// <summary>
        /// Numero de serie de los documentos
        /// </summary>
        public string serie { get; set; }

        /// <summary>
        /// Número correlativo del documento de inicio dentro de la serie
        /// </summary>
        public int numeroInicio { get; set; }

        /// <summary>
        /// Número correlativo del documento de fin dentro de la serie
        /// </summary>
        public int numeroFin { get; set; }

        /// <summary>
        /// El codigo de moneda Catalogo 02 Sunat
        /// </summary>
        public string codMoneda { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad totalOperacionesGravadas de cada boleta-nota credito/debito
        /// </summary>
        public decimal totalOperacionesGravadas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad totalOperacionesExoneradas de cada boleta-nota credito/debito
        /// </summary>
        public decimal totalOperacionesExoneradas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad totalOperacionesInafectas de cada boleta-nota credito/debito
        /// </summary>
        public decimal totalOperacionesInafectas { get; set; }

        /// <summary>
        /// Es la sumatoria de de la propiedad sumatoriaOtrosCargos de cada boleta-nota credito/debito
        /// </summary>
        public decimal sumatoriaOtrosCargos { get; set; }

        /// <summary>
        /// La sumatoria de la propiedad sumatoriaISC de cada boleta-nota credito/debito
        /// </summary>
        public decimal sumatoriaISC { get; set; }

        /// <summary>
        /// La sumatoria de la propiedad sumatoriaIGV de cada boleta-nota credito/debito
        /// </summary>
        public decimal sumatoriaIGV { get; set; }

        /// <summary>
        /// La suma total de la propiedad sumatoriaOTH de cada boleta-nota credito/debito
        /// </summary>
        public decimal sumatoriaOTH { get; set; }

        /// <summary>
        /// La suma toal de la propiedad importeTotal de cada boleta-nota credito/debito
        /// </summary>
        public decimal importeTotal { get; set; }
    }
}
