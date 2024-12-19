// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System;

namespace GasperSoft.SUNAT
{
    /// <summary>
    /// Clase para el manejo de Códigos de Error en las validaciones
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Código del error
        /// </summary>
        public string codigo { get; set; }

        /// <summary>
        /// Detalle del error
        /// </summary>
        public string detalle { get; set; }

        /// <summary>
        /// requerido para la serializacion de respuestas en las API
        /// </summary>
        public Error()
        {

        }

        /// <summary>
        /// Los mensajes comienzan con la Letra V(Validacion) o S(SUNAT),
        /// </summary>
        /// <param name="mensaje">el mensaje a procesar</param>
        internal Error(string mensaje)
        {
            int _index = mensaje.IndexOf(":");

            if ((mensaje.StartsWith("V") || mensaje.StartsWith("S")) && (_index == 4 || _index == 5))
            {
                codigo = mensaje.Substring(0, _index);
                detalle = mensaje.Substring(_index + 1);
            }
            else
            {
                codigo = string.Empty;
                detalle = mensaje;
            }
        }

        internal Error(string mensaje, string observacion) : this(mensaje)
        {
            detalle = $"{detalle} - Obs: {observacion}";
        }

        internal Error(Exception ex) : this(ex.MessageExt()) { }
    }
}
