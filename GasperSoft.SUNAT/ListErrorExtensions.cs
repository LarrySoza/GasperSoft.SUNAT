// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT
{
    internal static class ListErrorExtensions
    {
        public static void AddMensaje(this List<Error> lista, string mensaje)
        {
            lista.Add(new Error(mensaje));
        }

        public static void AddMensaje(this List<Error> lista, string mensaje, string observacion)
        {
            lista.Add(new Error(mensaje, observacion));
        }

        public static void AddMensaje(this List<Error> lista, Exception ex)
        {
            lista.Add(new Error(ex));
        }

        public static string MessageExt(this Exception ex)
        {
            string _msgException = ex.Message;

            if (ex.InnerException != null)
            {
                _msgException = ex.InnerException.Message;
            }

            return _msgException;
        }
    }
}
