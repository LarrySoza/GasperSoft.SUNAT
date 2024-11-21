// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System.Text.RegularExpressions;
using System;

namespace GasperSoft.SUNAT.DTO.Validar
{
    public class Validaciones
    {
        #region Private
        private static bool Valruc(string input)
        {
            input = input.Trim();
            if (IsInteger(input))
            {
                if (input.Length == 8)
                {
                    var suma = 0;
                    for (int i = 0; i < input.Length - 1; i++)
                    {
                        var digito = input[i] - '0';
                        if (i == 0) suma += (digito * 2);
                        else suma += (digito * (input.Length - i));
                    }
                    var resto = suma % 11;
                    if (resto == 1) resto = 11;
                    if (resto + (input[input.Length - 1] - '0') == 11)
                    {
                        return true;
                    }
                }
                else if (input.Length == 11)
                {
                    var suma = 0;
                    var x = 6;
                    for (int i = 0; i < input.Length - 1; i++)
                    {
                        if (i == 4) x = 8;
                        var digito = input[i] - '0';
                        x--;
                        if (i == 0) suma += (digito * x);
                        else suma += (digito * x);
                    }
                    var resto = suma % 11;
                    resto = 11 - resto;

                    if (resto >= 10) resto -= 10;
                    if (resto == input[input.Length - 1] - '0')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        internal static bool ValidarToleranciaCalculo(decimal valorEnviado, decimal valorCalculado, decimal toleranciaCalculo)
        {
            if (toleranciaCalculo < 0.20m)
            {
                toleranciaCalculo = 0.20m;
            }

            if (toleranciaCalculo > 1)
            {
                toleranciaCalculo = 1;
            }

            if (valorEnviado + toleranciaCalculo < valorCalculado || valorEnviado - toleranciaCalculo > valorCalculado)
            {
                return false;
            }

            return true;
        }

        public static bool IsDecimal(string input)
        {
            Regex regex = new Regex(@"^[0-9]{1,9}([\.\,][0-9]{1,3})?$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsInteger(string input)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidTimeSunat(string input)
        {
            Regex regex = new Regex(@"^[0-9]{2}:[0-9]{2}:[0-9]{2}?$");

            //Validar la extructura
            if (!regex.IsMatch(input ?? ""))
            {
                return false;
            }

            //Validar que sea una hora menor a 23:59:59
            var _splitHoraStr = input.Split(':');
            int _hora = Convert.ToInt32(_splitHoraStr[0]);
            int _minutos = Convert.ToInt32(_splitHoraStr[1]);
            int _segundos = Convert.ToInt32(_splitHoraStr[2]);

            if (_hora > 23 || _minutos > 59 || _segundos > 59)
            {
                return false;
            }

            return true;
        }

        public static bool IsValidPlacaSunat(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{6,8})$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidLicenciaConducirSunat(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{9,10})$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidNumeroDeclaracionAduana(string input)
        {
            Regex regex = new Regex(@"^[0-9]{3}-[0-9]{4}-[0-9]{2}-[0-9]{1,6}$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidPartidaArancelaria(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9]{1,10})$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidSerieEnDeclaracionAduana(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9]{1,4})$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidRegistroMTC(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{1,20})$");
            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidTextSunat(string input, int longitudMinima, int longitudMaxima)
        {
            if (input.StartsWith(" ") || input.EndsWith(" "))
            {
                return false;
            }

            if ((input ?? "").Length < longitudMinima || (input ?? "").Length > longitudMaxima)
            {
                return false;
            }

            Regex regex = new Regex(@"^(?!\s*$)[\s\S].{0,}");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidCantidadDecimalesMaximos(decimal input, int digitos)
        {
            input = Math.Abs(input);

            if ((int)input == input)
            {
                return true;
            }
            else
            {
                var parteDecimal = input - (int)input;

                var totalDecimales = parteDecimal.ToString().Substring(2).Length;

                return !(totalDecimales > digitos);
            }
        }

        public static bool IsValidEmail(string input)
        {
            if (input == null)
                return false;

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,6}|[0-9]{1,3})(\]?)$";

            Regex re = new Regex(strRegex);
            return re.IsMatch(input);
        }

        public static bool IsValidSeries(string tipoDoc, string serie)
        {
            if (string.IsNullOrEmpty(tipoDoc) || string.IsNullOrEmpty(serie))
                return false;

            if (serie.Length != 4)
                return false;

            if (tipoDoc == "03" && serie.StartsWith("B"))
                return true;

            if (tipoDoc == "01" && serie.StartsWith("F"))
                return true;

            if ((tipoDoc == "07" || tipoDoc == "08") && (serie.StartsWith("F") || serie.StartsWith("B")))
                return true;

            if ((tipoDoc == "09") && serie.StartsWith("T"))
                return true;

            if ((tipoDoc == "20") && serie.StartsWith("R"))
                return true;

            if ((tipoDoc == "31") && serie.StartsWith("V"))
                return true;

            return false;
        }

        public static bool IsValidRuc(string input)
        {
            return (!(string.IsNullOrEmpty(input) || !IsInteger(input) || !(input.Length == 11) || !Valruc(input)));
        }

        public static bool IsValidDni(string input)
        {
            var regex = new Regex(@"^[\d]{8}$");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidTipoDocumentoIdentidad(string input)
        {
            var regex = new Regex(@"^[01467A\-]{1}$");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidUbigeo(string input)
        {
            var regex = new Regex(@"^[0-9]{6}$");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidUrl(string url)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(url);
        }

        public static bool IsValidOrdenCompra(string input)
        {
            var regex = new Regex(@"^[0-9a-zA-Z]{1,20}$");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidTuc(string input)
        {
            var regex = new Regex(@"^(?!0+$)([0-9A-Z]{10,15})$");

            return regex.IsMatch(input ?? "");
        }

        public static bool IsValidAutorizacionEspecial(string input)
        {
            var regex = new Regex(@"^[^\s\n\t\r]{3,50}$");
            
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Previamente debe invocarse al metodo "IsValidTipoDocumentoIdentidad(string tipoDocumento)"
        /// Segun SUNAT cualquier otro tipo de documento alfanumerico entre 2 y 15 digitos es valido
        /// </summary>
        /// <param name="numeroDocumento">El numero a validar</param>
        /// <param name="tipoDocumento">El tipo de documento</param>
        /// <returns></returns>
        public static bool IsValidDocumentoIdentidadSunat(string numeroDocumento, string tipoDocumento)
        {
            if (string.IsNullOrEmpty(tipoDocumento) ||
                string.IsNullOrEmpty(numeroDocumento))
                return false;

            var regex = new Regex(@"^(?!\s*$)[^\s]{1,15}$");

            switch (tipoDocumento)
            {
                case "1":
                    return IsValidDni(numeroDocumento);
                case "6":
                    return IsValidRuc(numeroDocumento);
                case "4":
                case "7":
                    if (numeroDocumento.Length > 12)
                        return false;
                    goto default;
                default:
                    if (regex.IsMatch(numeroDocumento ?? ""))
                    {
                        return true;
                    }
                    return false;
            }
        }

        /// <summary>
        /// Un delegado que devuelve sin un determinado valor se encuentra en un determinado catálogo de SUNAT (Anexo 8)
        /// </summary>
        /// <param name="catalogo">El codigo del catalogo</param>
        /// <param name="valor">El valor a buscar</param>
        /// <returns></returns>
        public delegate bool ValidarCatalogoSunat(string catalogo, string valor);
    }
}
