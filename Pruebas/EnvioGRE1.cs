// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

#if NET462_OR_GREATER || NET6_0_OR_GREATER

using GasperSoft.SUNAT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Pruebas
{
    /// <summary>
    /// Este es solo un ejemplo de cómo enviar una guía electrónica a SUNAT, para estas pruebas estoy usando un servidor de pruebas de NUBEFACT
    /// El código principal de envió está en el método "Enviar"
    /// El código principal de consulta de ticket está en el método "ObtenerEstado"
    /// </summary>
    internal class EnvioGRE1
    {
        //URL de produccion SUNAT
        //string _baseUrlServicio = "https://api-cpe.sunat.gob.pe";
        //string _baseUrlToken = "https://api-seguridad.sunat.gob.pe";

        //URL de pruebas NUBEFACT
        private static string _baseUrlServicio = "https://gre-test.nubefact.com";
        private static string _baseUrlToken = "https://gre-test.nubefact.com";

        private static string _ruc = App.GetEmisior.ruc;
        private static string _usuarioSol = "MODDATOS";
        private static string _claveSol = "MODDATOS";

        //Estos son datos de prueba proporcionados por NUBEFACT para produccion se deben generar usando la Clave sol revisar el archivo "Manual_Servicios_GRE.pdf" en la carpeta ManualesSunat
        private static string _clientID = "test-85e5b0ae-255c-4891-a595-0b98c65c9854";
        private static string _clientSecret = "test-Hty/M6QshYvPgItX2P0+Kw==";

        public static void Run()
        {
            var _pathStorage = App.GetPathStorage;
            Dictionary<string, string> _listaXML;

        ListarXML:

            #region Codigo para mostrar una lista de los XML de guias electronicas que existen en el storage

            Console.Clear();
            
            _listaXML = new Dictionary<string, string>();

            ConsoleEx.WriteLine("GUIAS Existentes");
            ConsoleEx.WriteLine("================");
            ConsoleEx.WriteLine();

            var _directoryInfo = new DirectoryInfo(_pathStorage);
            var _archivos = _directoryInfo.GetFiles("*.zip");

            int i = 0;

            foreach (var archivo in _archivos)
            {
                if (archivo.Name.StartsWith($"{_ruc}-09") || archivo.Name.StartsWith($"{_ruc}-31"))
                {
                    _listaXML.Add(i.ToString(), archivo.FullName);

                    ConsoleEx.Write($"{i}", ConsoleColor.Magenta);
                    ConsoleEx.WriteLine($": {archivo.Name}");
                    i++;
                }
            }
            
            #endregion

            if (_listaXML.Count > 0)
            {
                ConsoleEx.Write("X", ConsoleColor.Magenta);
                ConsoleEx.WriteLine(": SALIR");

                while (true)
                {
                    Console.WriteLine("Ingrese opción:");

                    var _input = ConsoleEx.ReadLine();

                    if (_input?.ToLower() == "x") break;

                    var _pathArchivoZipXml = string.Empty;

                    _listaXML.TryGetValue(_input ?? "", out _pathArchivoZipXml);

                    if (!string.IsNullOrEmpty(_pathArchivoZipXml))
                    {
                        var _pathTicketArchivoZipXml = $"{_pathArchivoZipXml}.ticket";

                        //verifico si existe el archivo .ticket para el documento

                        if (!File.Exists(_pathTicketArchivoZipXml))
                        {
                            //Aqui no existe un ticket para el documento entonces lo enviamos y almacenamos el ticket que devuelve
                            var _ticket = Enviar(File.ReadAllBytes(_pathArchivoZipXml), Path.GetFileName(_pathArchivoZipXml));

                            if (!string.IsNullOrEmpty(_ticket))
                            {
                                File.WriteAllText(_pathTicketArchivoZipXml, _ticket);

                                ConsoleEx.Write("Archivo enviado, se genero el ticket N°:");
                                ConsoleEx.WriteLine(_ticket, ConsoleColor.Blue);
                            }
                        }
                        else
                        {
                            //Ya existe un ticket entonces consultamos si existe su CDR

                            var _pathCDR = Path.Combine(_pathStorage, $"R-{Path.GetFileName(_pathArchivoZipXml)}");

                            if (!File.Exists(_pathCDR))
                            {
                                //No existe CDR consultamos su ticket
                                var _cdr = ObtenerEstado(File.ReadAllText(_pathTicketArchivoZipXml));

                                if (_cdr != null)
                                {
                                    File.WriteAllBytes(_pathCDR, _cdr);
                                    ConsoleEx.WriteLine("CDR descargado");
                                }
                            }
                            else
                            {
                                ConsoleEx.WriteLine("El documento ya cuenta con un CDR");
                            }
                        }

                        ConsoleEx.WriteLine("\nPresione una tecla para continuar.");
                        ConsoleEx.ReadKey();

                        goto ListarXML;
                    }
                    else
                    {
                        ConsoleEx.WriteLine("Opción no valida", ConsoleColor.Red);
                    }
                }
            }

            ConsoleEx.WriteLine("\nPresione una tecla para continuar.");
            ConsoleEx.ReadKey();
        }

        private static string Enviar(byte[] archivo, string nombreArchivo)
        {
            var _clientGRE = new ClientGRE(_baseUrlServicio, _baseUrlToken, _ruc, _usuarioSol, _claveSol, _clientID, _clientSecret);

            #region Codigo para leer/almacenar un token generado

            /*
             * Cuando se genera un token en SUNAT este tiene un tiempo de vida especifico, entonces se puede almacenar y usar para varias peticiones de envió y consulta,
             * en este ejemplo se usa un simple archivo de texto para almacenarlo
             * */

            var _pathToken = "Token.txt";

            if (File.Exists(_pathToken))
            {
                //Asigno el token al cliente, si aún está vigente se usa en la petición de envió o consulta, caso contrario generara otro
                //sin intervención alguna
                _clientGRE.SetToken = File.ReadAllText(_pathToken);
            }

            //Este evento se genera cuando se solicita un nuevo token, aqui podemos almacenarlo
            _clientGRE.EndGenerarToken += (token) =>
            {
                File.WriteAllText(_pathToken, token);
            };

            #endregion

            if (_clientGRE.EnviarDocumento(archivo, Path.GetFileNameWithoutExtension(nombreArchivo)))
            {
                return _clientGRE.GetTicketSunat;
            }
            else
            {
                //Muestro los mensajes del cliente
                ConsoleEx.WriteLine(_clientGRE.GetMensaje);
                return default;
            }
        }

        private static byte[] ObtenerEstado(string ticket)
        {
            var _clientGRE = new ClientGRE(_baseUrlServicio, _baseUrlToken, _ruc, _usuarioSol, _claveSol, _clientID, _clientSecret);

            #region Codigo para leer/almacenar un token generado

            /*
             * Cuando se genera un token en SUNAT este tiene un tiempo de vida especifico, entonces se puede almacenar y usar para varias peticiones de envió y consulta,
             * en este ejemplo se usa un simple archivo de texto para almacenarlo
             * */

            var _pathToken = "Token.txt";

            if (File.Exists(_pathToken))
            {
                //Asigno el token al cliente, si aún está vigente se usa en la petición de envió o consulta, caso contrario generara otro
                //sin intervención alguna
                _clientGRE.SetToken = File.ReadAllText(_pathToken);
            }

            //Este evento se genera cuando se solicita un nuevo token, aqui podemos almacenarlo
            _clientGRE.EndGenerarToken += (token) =>
            {
                File.WriteAllText(_pathToken, token);
            };

            #endregion

            if (_clientGRE.ObtenerEstado(ticket))
            {
                return _clientGRE.GetBytesCdr;
            }
            else
            {
                ConsoleEx.WriteLine("El Api devolvio");
                ConsoleEx.WriteLine("===============");

                if (!string.IsNullOrEmpty(_clientGRE.GetCodigoMensaje))
                {
                    ConsoleEx.Write($"{_clientGRE.GetCodigoMensaje}: ", ConsoleColor.Blue);
                }

                ConsoleEx.WriteLine(_clientGRE.GetMensaje);
                return default;
            }
        }
    }
}

#endif