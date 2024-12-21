// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Text.RegularExpressions;

namespace GasperSoft.SUNAT
{
    /// <summary>
    /// Validaciones de SUNAT y Otras
    /// </summary>
    public class Validaciones
    {
        internal static bool IsNullOrWhiteSpace(string value)
        {
#if NET35 || NET40
            return Validaciones.IsNullOrWhiteSpace(value) || value.Trim().Length > 0;
#else
            return string.IsNullOrWhiteSpace(value);    
#endif
        }

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

        /// <summary>
        /// Valida si el input es un numero decimal 
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsDecimal(string input)
        {
            Regex regex = new Regex(@"^[0-9]{1,9}([\.\,][0-9]{1,3})?$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input es un entero
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsInteger(string input)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato HH:mm:ss
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
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

        /// <summary>
        /// Valida si el input tiene el formato de una placa segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidPlacaSunat(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{6,8})$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un numero de licencia de conducir segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidLicenciaConducirSunat(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{9,10})$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// valida si el input tiene el formato de un numero de declaracion de aduana segun SUNAT
        /// Se usa la siguiente expresion regular "^[0-9]{3}-[0-9]{4}-[0-9]{2}-[0-9]{1,6}$"
        /// donde: [0-9]{3}: Código de la Aduana, [0-9]{4}: Año, [0-9]{2} Régimen aduanero, [0-9]{1,6} Correlativo
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidNumeroDeclaracionAduana(string input)
        {
            Regex regex = new Regex(@"^[0-9]{3}-[0-9]{4}-[0-9]{2}-[0-9]{1,6}$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un numero de partida arancelaria segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidPartidaArancelaria(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9]{1,10})$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un numero de serie en declaracion aduanera segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidSerieEnDeclaracionAduana(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9]{1,4})$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un registro del MTC segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        public static bool IsValidRegistroMTC(string input)
        {
            Regex regex = new Regex(@"^(?!0+$)([0-9A-Z]{1,20})$");
            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un texto segun SUNAT
        /// </summary>
        /// <param name="input">Texto a evaluar</param>
        /// <param name="longitudMinima">Longitud minima del texto</param>
        /// <param name="longitudMaxima">Longitud maxima del texto</param>
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

        /// <summary>
        /// Validar si el input es un decimal con un maximo de x digitos
        /// </summary>
        /// <param name="input">Decimal a evaluar</param>
        /// <param name="digitos">Numero maximo de decimales</param>
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

        /// <summary>
        /// Valida si el input es un correo electrónico valido
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Validar si la serie es valida para el tipo de documento
        /// </summary>
        /// <param name="tipoDoc">Tipo de documento</param>
        /// <param name="serie">Serie a evaluar</param>
        public static bool IsValidSeries(string tipoDoc, string serie)
        {
            if (Validaciones.IsNullOrWhiteSpace(tipoDoc) || Validaciones.IsNullOrWhiteSpace(serie))
                return false;

            if (serie.Length != 4) return false;

            if (tipoDoc == "03" && serie.StartsWith("B")) return true;

            if (tipoDoc == "01" && serie.StartsWith("F")) return true;

            if ((tipoDoc == "07" || tipoDoc == "08") && (serie.StartsWith("F") || serie.StartsWith("B"))) return true;

            if ((tipoDoc == "09") && serie.StartsWith("T")) return true;

            if ((tipoDoc == "20") && serie.StartsWith("R")) return true;

            if ((tipoDoc == "31") && serie.StartsWith("V")) return true;

            return false;
        }

        /// <summary>
        /// Valida si el input es un numero de RUC valido
        /// </summary>
        /// <param name="input">Numero de ruc a evaluar</param>
        public static bool IsValidRuc(string input)
        {
            if (Validaciones.IsNullOrWhiteSpace(input) || !IsInteger(input) || !(input.Length == 11))
            {
                return false;
            }

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

            return false;
        }

        /// <summary>
        /// Valida si el input es un numero de DNI valido
        /// </summary>
        /// <param name="input">Numero de DNI a evaluar</param>
        public static bool IsValidDni(string input)
        {
            var regex = new Regex(@"^[\d]{8}$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si input es un tipo de documento de identidad valido segun SUNAT
        /// </summary>
        /// <param name="input">Tipo de documento a evaluar</param>
        public static bool IsValidTipoDocumentoIdentidad(string input)
        {
            var regex = new Regex(@"^[01467A\-]{1}$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si input tiene el formato de un UBIGEO valido
        /// </summary>
        /// <param name="input">Ubigeo a evaluar</param>
        public static bool IsValidUbigeo(string input)
        {
            var regex = new Regex(@"^[0-9]{6}$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si input es una url valida
        /// </summary>
        /// <param name="input">Url a evaluar</param>
        public static bool IsValidUrl(string input)
        {
            string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            Regex Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return Rgx.IsMatch(input);
        }

        /// <summary>
        /// Validar si el input tiene el formato de una orden de compra segun SUNAT
        /// </summary>
        /// <param name="input">Orden de compra a evaluar</param>
        public static bool IsValidOrdenCompra(string input)
        {
            var regex = new Regex(@"^[0-9a-zA-Z]{1,20}$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un numero TUC segun SUNAT
        /// </summary>
        /// <param name="input">Tuc a evaluar</param>
        public static bool IsValidTuc(string input)
        {
            var regex = new Regex(@"^(?!0+$)([0-9A-Z]{10,15})$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un numero de autorizacion especial segun SUNAT
        /// </summary>
        /// <param name="input">Autorizacion a evaluar</param>
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
            if (Validaciones.IsNullOrWhiteSpace(tipoDocumento) ||
                Validaciones.IsNullOrWhiteSpace(numeroDocumento))
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
        /// Valida si el input tiene el formato de un código de producto según SUNAT
        /// </summary>
        /// <param name="input">Codigo a evaluar</param>
        public static bool IsValidCodigoProducto(string input)
        {
            var regex = new Regex(@"^((?!\s*$)[\s\S]{0,29})$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Valida si el input tiene el formato de un codigo de producto GS1 segun SUNAT
        /// </summary>
        /// <param name="tipo">Tipo de codigo GS1</param>
        /// <param name="codigo">Codigo GS1 a evaluar</param>
        public static bool IsValidCodigoProductoGS1(string tipo, string codigo)
        {
            Regex regex;

            switch (tipo)
            {
                case "GTIN-8":
                    regex = new Regex(@"^((?!\s*$)[\s\S]{8})$");
                    break;
                case "GTIN-12":
                    regex = new Regex(@"^((?!\s*$)[\s\S]{12})$");
                    break;
                case "GTIN-13":
                    regex = new Regex(@"^((?!\s*$)[\s\S]{13})$");
                    break;
                case "GTIN-14":
                    regex = new Regex(@"^((?!\s*$)[\s\S]{14})$");
                    break;
                default:
                    return false;
            }

            return regex.IsMatch(codigo ?? "");
        }

        /// <summary>
        /// Valida si el input tiene tenga un formato alfanumerico de 2 a 30 caracteres
        /// </summary>
        /// <param name="input">La cadena a evaluar</param>
        public static bool IsValidCodigoInformacionAdicional(string input)
        {
            var regex = new Regex(@"^[0-9a-zA-Z]{2,30}$");

            return regex.IsMatch(input ?? "");
        }

        /// <summary>
        /// Un delegado que devuelve sin un determinado valor se encuentra en un determinado catálogo de SUNAT (Anexo 8)
        /// </summary>
        /// <param name="catalogo">El codigo del catalogo</param>
        /// <param name="valor">El valor a buscar</param>
        /// <returns></returns>
        public delegate bool ValidarCatalogoSunat(string catalogo, string valor);

        /// <summary>
        /// Se puede usar para validar con base de datos que codigoRegimenRetencion exista y que la tasa corresponda a este codigo
        /// </summary>
        /// <param name="codigoRegimenRetencion">El codigo de retencion (Catalogo 23)</param>
        /// <param name="tasa">Tasa de retencion (Catalogo 23)</param>
        public delegate bool ValidarTasaRetencion(string codigoRegimenRetencion, decimal tasa);
    }
}
