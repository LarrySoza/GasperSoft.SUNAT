// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

#if NET462_OR_GREATER || NET6_0_OR_GREATER

using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Security.Cryptography;
using System.Text.Json;

namespace GasperSoft.SUNAT
{
    /// <summary>
    /// Clase Para el envió de Guías a SUNAT
    /// </summary>
    public class ClientGRE
    {
        private readonly string _baseUrlServicio;
        private readonly string _baseUrlToken;
        private readonly string _ruc;
        private readonly string _usuarioSol;
        private readonly string _claveSol;
        private readonly string _clientID;
        private readonly string _clientSecret;
        private string _ticketSunat;
        private string _token;
        private byte[] _bytesCdr;
        private string _mensaje;
        private string _codigoMensaje;

        /// <summary>
        /// Delegado para el Evento EndGenerarToken
        /// </summary>
        /// <param name="token">Token Generado</param>
        public delegate void EventEndGenerarToken(string token);

        #region DTO SUNAT

        private class oTokenDto
        {
            public string access_token { get; set; }

            public string token_type { get; set; }

            public int expires_in { get; set; }
        }

        private class oAccessTokenDTo
        {
            public int exp { get; set; }
            public string grantType { get; set; }
            public int iat { get; set; }
        }

        private class oErrorConsultarGre
        {
            public string numError { get; set; }
            public string desError { get; set; }
        }

        private class oArchivoDto
        {
            public string nomArchivo { get; set; }
            public byte[] arcGreZip { get; set; }
            public string hashZip { get; set; }
        }

        private class RequestEnvioGre
        {
            public oArchivoDto archivo { get; set; }
        }

        private class ResponseEnvioGre
        {
            public string numTicket { get; set; }
            public string fecRecepcion { get; set; }
        }

        private class ResponseConsultarGre
        {
            public string codRespuesta { get; set; }

            public oErrorConsultarGre error { get; set; }

            public string arcCdr { get; set; }

            public string indCdrGenerado { get; set; }
        }

        #endregion

        /// <summary>
        /// Clase para el Envió de guías de remisión a SUNAT
        /// </summary>
        /// <param name="baseUrlServicio">Url base para el envio de guias</param>
        /// <param name="baseUrlToken">Url base para generar el token</param>
        /// <param name="ruc">Ruc</param>
        /// <param name="usuarioSol">Usuario Sol</param>
        /// <param name="claveSol">Clave Sol</param>
        /// <param name="clientID">Client ID creado desde el portal de SUNAT</param>
        /// <param name="clientSecret">Client Secret creado desde el portal de SUNAT</param>
        public ClientGRE(string baseUrlServicio,
                              string baseUrlToken,
                              string ruc,
                              string usuarioSol,
                              string claveSol,
                              string clientID,
                              string clientSecret)
        {
            if (string.IsNullOrEmpty(baseUrlServicio)) throw new ArgumentNullException(nameof(baseUrlServicio));
            if (string.IsNullOrEmpty(baseUrlToken)) throw new ArgumentNullException(nameof(baseUrlToken));
            if (string.IsNullOrEmpty(ruc)) throw new ArgumentNullException(nameof(ruc));
            if (string.IsNullOrEmpty(usuarioSol)) throw new ArgumentNullException(nameof(usuarioSol));
            if (string.IsNullOrEmpty(claveSol)) throw new ArgumentNullException(nameof(claveSol));
            if (string.IsNullOrEmpty(clientID)) throw new ArgumentNullException(nameof(clientID));
            if (string.IsNullOrEmpty(clientSecret)) throw new ArgumentNullException(nameof(clientSecret));

            _baseUrlServicio = baseUrlServicio;
            _baseUrlToken = baseUrlToken;
            _ruc = ruc;
            _usuarioSol = usuarioSol;
            _claveSol = claveSol;
            _clientID = clientID;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Envía la guía de remisión a SUNAT, si todo es correcto devuelve Verdadero y se puede obtener el ticket invocando a la propiedad "GetTicketSunat",
        /// caso contrario devolverá false y puede leer la respuesta del servidor invocando a la propiedad GetMensaje
        /// </summary>
        /// <param name="archivo">Bytes del XML comprimido en formato zip</param>
        /// <param name="nombreArchivo">El nombre del archivo en formato "ruc-tipoGUia-serie-numero" ejemplo: 20606433094-09-T001-1</param>
        public bool EnviarDocumento(byte[] archivo, string nombreArchivo)
        {
            if (ToKenVencido())
            {
                GenerarToken();
            }

            var _sha256 = SHA256.Create();
            var _checksum = _sha256.ComputeHash(archivo);
            var _hash = BitConverter.ToString(_checksum).Replace("-", string.Empty).ToLower();

            var _body = new RequestEnvioGre()
            {
                archivo = new oArchivoDto()
                {
                    nomArchivo = $"{nombreArchivo}.zip",
                    arcGreZip = archivo,
                    hashZip = _hash
                }
            };

            var _request = new RestRequest($"/v1/contribuyente/gem/comprobantes/{nombreArchivo}")
            .AddJsonBody(_body);

            var _clientOptions = new RestClientOptions(_baseUrlServicio)
            {
                Authenticator = new JwtAuthenticator(_token),
                Expect100Continue = false
            };

            var _clientGre = new RestClient(_clientOptions);

            var _response = _clientGre.ExecutePost(_request);

            if (_response.IsSuccessful)
            {
                var _envioGre = JsonSerializer.Deserialize<ResponseEnvioGre>(_response.Content);
                _ticketSunat = _envioGre.numTicket;
                return true;
            }
            else
            {
                _mensaje = _response.Content;
                return false;
            }
        }

        /// <summary>
        /// Consulta el estado de envio de un ticket en SUNAT y devuelve true si existe el CDR que se puede leer invocando a "GetBytesCdr", 
        /// caso contrario devuelve false y se pueden leer el código y mensaje de error invocando a "GetCodigoMensaje" y "GetMensaje"
        /// </summary>
        /// <param name="ticket">Numero de ticket de la guía de remisión a consultar</param>
        public bool ObtenerEstado(string ticket)
        {
            if (ToKenVencido())
            {
                GenerarToken();
            }

            var _request = new RestRequest($"/v1/contribuyente/gem/comprobantes/envios/{ticket}");

            var _clientOptions = new RestClientOptions(_baseUrlServicio)
            {
                Authenticator = new JwtAuthenticator(_token),
                Expect100Continue = false
            };

            var _clientGre = new RestClient(_clientOptions);

            var _response = _clientGre.ExecuteGet(_request);

            if (_response.IsSuccessful)
            {
                var _consultarGre = JsonSerializer.Deserialize<ResponseConsultarGre>(_response.Content);

                switch (_consultarGre.codRespuesta)
                {
                    case "98":
                        _mensaje = "Aun en proceso";
                        return false;
                    case "0":
                        _bytesCdr = Convert.FromBase64String(_consultarGre.arcCdr);
                        return true;
                    case "99":
                        if (_consultarGre.error != null)
                        {
                            _codigoMensaje = _consultarGre.error.numError;
                            _mensaje = _consultarGre.error.desError;
                        }

                        if (_consultarGre.indCdrGenerado == "1")
                        {
                            _bytesCdr = Convert.FromBase64String(_consultarGre.arcCdr);
                            return true;
                        }

                        return false;
                    default:
                        goto case "98";
                }
            }
            else
            {
                throw new Exception(_response.Content);
            }
        }

        private bool ToKenVencido()
        {
            if (string.IsNullOrEmpty(_token)) return true;

            try
            {
                string[] _tokenSplit = _token.Split('.');

                //El token consta de 3 partes separada por el caracter "." es la segunda parte la que contiene
                //la carga util que es un JSON codificado en base64 la fecha de expiracion en formato UNIX

                var _payloadBase64 = _tokenSplit[1];

                int mod4 = _payloadBase64.Length % 4;
                if (mod4 > 0)
                {
                    _payloadBase64 += new string('=', 4 - mod4);
                }

                var base64EncodedBytes = System.Convert.FromBase64String(_payloadBase64);

                var _payload = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                var _accessTokenDto = JsonSerializer.Deserialize<oAccessTokenDTo>(_payload);

                //le quitare unos 5 minutos = 300 segundos para evitar problemas
                DateTime _fechaExpiracion = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                _fechaExpiracion = _fechaExpiracion.AddSeconds(_accessTokenDto.exp - 300).ToLocalTime();

                //Si la fecha actual es menor a la fecha de expiracion entonces ahun no vence el token
                if (DateTime.Now <= _fechaExpiracion)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //Cualquier error devolvemos true
                return true;
            }
        }

        private void GenerarToken()
        {
            var _request = new RestRequest($"/v1/clientessol/{_clientID}/oauth2/token/")
                .AddParameter("grant_type", "password")
                .AddParameter("scope", "https://api-cpe.sunat.gob.pe")
                .AddParameter("client_id", _clientID)
                .AddParameter("client_secret", _clientSecret)
                .AddParameter("username", string.Concat(_ruc, _usuarioSol))
                .AddParameter("password", _claveSol);

            //Esto garantiza que se mande correctamente el contentType application/json
            //_request.AddHeader("", ContentType.Json);

            var _clientOptions = new RestClientOptions(_baseUrlToken)
            {
                Expect100Continue = false,
                DisableCharset = true
            };

            //Tomar en cuenta que con esta línea estamos aceptando todos los certificados lo que implementa una brecha de seguridad muy grave
            //_clientOptions.RemoteCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            var _clientToken = new RestClient(_clientOptions);

            var _response = _clientToken.ExecutePost(_request);

            if (_response.IsSuccessful)
            {
                var _tokeDto = JsonSerializer.Deserialize<oTokenDto>(_response.Content);

                _token = _tokeDto.access_token;

                if (EndGenerarToken != null)
                {
                    EndGenerarToken(_token);
                }
            }
            else
            {
                throw new Exception(_response.Content);
            }
        }

        /// <summary>
        /// Se produce cuando se termina de generar un nuevo token de autentificación en SUNAT
        /// </summary>
        public EventEndGenerarToken EndGenerarToken;

        /// <summary>
        /// Devuelve el Ticket generado por SUNAT
        /// </summary>
        public string GetTicketSunat
        {
            get { return _ticketSunat; }
        }

        /// <summary>
        /// Devuelve los Bytes del CDR devuelto por SUNAT
        /// </summary>
        public byte[] GetBytesCdr
        {
            get { return _bytesCdr; }
        }

        /// <summary>
        /// Devuelve el mensaje de error generado por alguno de los métodos de envió/consulta SUNAT
        /// </summary>
        public string GetMensaje
        {
            get { return _mensaje; }
        }

        /// <summary>
        /// Devuelve el codigo del mensaje de error (de existir) generado por alguno de los métodos de envió/consulta SUNAT
        /// </summary>
        public string GetCodigoMensaje
        {
            get { return _codigoMensaje; }
        }

        /// <summary>
        /// Asigna un token de sesion para su uso en los métodos de envió/consulta SUNAT
        /// </summary>
        public string SetToken
        {
            set
            {
                _token = value;
            }
        }
    }
}

#endif