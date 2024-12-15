// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pruebas
{
    /// <summary>
    /// Queria tener una clase Console que permita escribir lineas con color
    /// </summary>
    internal static class ConsoleEx
    {
        public static void WriteLine(string mensaje, ConsoleColor ForegroundColor)
        {
            Console.ForegroundColor = ForegroundColor;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public static void Write(string mensaje, ConsoleColor ForegroundColor)
        {
            Console.ForegroundColor = ForegroundColor;
            Console.Write(mensaje);
            Console.ResetColor();
        }

        /// <summary>
        /// Hace lo mismo que Console.WriteLine, solo para tener todo en una sola clase
        /// </summary>
        public static void WriteLine(string mensaje)
        {
            Console.WriteLine(mensaje);
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        /// <summary>
        /// Hace lo mismo que Console.Write, solo para tener todo en una sola clase
        /// </summary>
        public static void Write(string mensaje)
        {
            Console.Write(mensaje);
        }

        /// <summary>
        /// Hace lo mismo que Console.ReadKey, solo para tener todo en una sola clase
        /// </summary>
        public static ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        /// <summary>
        /// Hace lo mismo que Console.Clear, solo para tener todo en una sola clase
        /// </summary>
        public static void Clear()
        {
            Console.Clear();
        }

        /// <summary>
        /// Hace lo mismo que Console.ReadLine, solo para tener todo en una sola clase
        /// </summary>
        public static string? ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
