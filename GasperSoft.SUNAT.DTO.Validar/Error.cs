// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GasperSoft.SUNAT.DTO.Validar
{
    public class Error
    {
        public string codigo { get; set; }

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
        public Error(string mensaje)
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

        public Error(string mensaje, string observacion) : this(mensaje)
        {
            detalle = $"{detalle} - Obs: {observacion}";
        }

        public Error(Exception ex) : this(ex.MessageExt()) { }
    }
}
