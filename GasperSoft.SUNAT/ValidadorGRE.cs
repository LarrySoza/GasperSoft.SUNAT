// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.GRE;
using System;
using System.Collections.Generic;
using System.Linq;
using static GasperSoft.SUNAT.Validaciones;

namespace GasperSoft.SUNAT
{
    public class ValidadorGRE
    {
        private readonly GREType _gre;
        private readonly List<Error> _mensajesError;

        public ValidadorGRE(GREType gre)
        {
            _gre = gre;
            _mensajesError = new List<Error>();
        }

        public List<Error> Errors
        {
            get
            {
                return _mensajesError;
            }
        }

        /// <summary>
        /// Validar que un valor se encuentre en un determinado catalogo de SUNAT
        /// </summary>
        public event ValidarCatalogoSunat OnValidarCatalogoSunat;

        public bool Validar()
        {
            if (OnValidarCatalogoSunat == null)
            {
                throw new Exception("Debe asociar el evento 'OnValidarCatalogoSunat' para la validación con los catálogos SUNAT");
            }

            _mensajesError.Clear();

            switch (_gre.tipoGuia)
            {
                case "09":
                    return ValidarGuiaRemitente();
                case "31":
                    return ValidarGuiaTransportista();
                default:
                    _mensajesError.AddMensaje($"No esta implementado la validacion para tipoGuia='{_gre.tipoGuia}'");
                    return false;
            }
        }

        private void ValidacionGeneral(out bool procesarMasValidaciones)
        {
            procesarMasValidaciones = false;

            //Se usa para saber el numero de item en una interaccion Foreach
            int _idRecord;

            #region Datos Envio

            if (_gre.datosEnvio == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, $"datosEnvio");
                return;
            }

            #endregion

            if ((_gre.detalles?.Count ?? 0) == 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0007, $"detalles");
            }

            procesarMasValidaciones = true;

            //Aqui comienzan mas validaciones generales
            #region Validacion del Destinatario

            if (_gre.destinatario == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "destinatario");
            }
            else
            {
                //Validamos el tipo de documento de identidad
                if (!Validaciones.IsValidTipoDocumentoIdentidad(_gre.destinatario.tipoDocumentoIdentificacion))
                {
                    _mensajesError.AddMensaje(CodigoError.S2760, "destinatario.tipoDocumentoIdentificacion");
                }
                else
                {
                    if (!Validaciones.IsValidDocumentoIdentidadSunat(_gre.destinatario.numeroDocumentoIdentificacion, _gre.destinatario.tipoDocumentoIdentificacion))
                    {
                        if (_gre.destinatario.tipoDocumentoIdentificacion == "6")
                        {
                            _mensajesError.AddMensaje(CodigoError.V0002, "destinatario.numeroDocumentoIdentificacion");
                        }
                        else
                        {
                            _mensajesError.AddMensaje(CodigoError.V0037, "destinatario.numeroDocumentoIdentificacion");
                        }
                    }

                    if (_gre.destinatario.tipoDocumentoIdentificacion != "-" && !Validaciones.IsValidTextSunat(_gre.destinatario.nombre, 3, 250))
                    {
                        _mensajesError.AddMensaje(CodigoError.V0005, "destinatario.nombre");
                    }
                }
            }

            #endregion

            #region Hora Emision

            if (!Validaciones.IsValidTimeSunat(_gre.horaEmision))
            {
                _mensajesError.AddMensaje(CodigoError.S3438, "horaEmision");
            }

            #endregion

            #region fechaEmision

            if (_gre.fechaEmision == new DateTime())
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "fechaEmision");
            }

            #endregion

            #region Serie

            if (!Validaciones.IsValidSeries(_gre.tipoGuia, _gre.serie))
            {
                _mensajesError.AddMensaje(CodigoError.V0009);
            }

            #endregion

            #region Numero

            if (_gre.numero.ToString().Length > 8)
            {
                _mensajesError.AddMensaje(CodigoError.V0010);
            }

            #endregion

            #region pesoBruto

            if (_gre.datosEnvio.pesoBrutoTotalBienes <= 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0037, "datosEnvio.pesoBrutoTotalBienes debe ser mayor a 0");
            }
            else
            {
                if (!Validaciones.IsValidCantidadDecimalesMaximos(_gre.datosEnvio.pesoBrutoTotalBienes, 10))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "datosEnvio.pesoBrutoTotalBienes");
                }
            }

            if (_gre.datosEnvio.unidadMedidaPesoBruto != "KGM" && _gre.datosEnvio.unidadMedidaPesoBruto != "TNE")
            {
                _mensajesError.AddMensaje(CodigoError.S2523, "datosEnvio.unidadMedidaPesoBruto");
            }

            #endregion

            #region Observaciones

            _idRecord = 0;

            if (_gre.observaciones != null)
            {
                foreach (var item in _gre.observaciones)
                {
                    if (!Validaciones.IsValidTextSunat(item, 3, 250))
                    {
                        _mensajesError.AddMensaje(CodigoError.V0005, $"observaciones[{_idRecord}]");
                    }
                    _idRecord++;
                }
            }

            #endregion

            #region Punto Partida

            if (_gre.datosEnvio.puntoPartida == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "datosEnvio.puntoPartida");
            }
            else
            {
                if (!Validaciones.IsValidUbigeo(_gre.datosEnvio.puntoPartida.ubigeo))
                {
                    _mensajesError.AddMensaje(CodigoError.S2776, "datosEnvio.puntoPartida.ubigeo");
                }
            }

            #endregion
        }

        private bool ValidarGuiaTransportista()
        {
            bool procesarMasValidaciones;

            #region Validaciones Generales

            ValidacionGeneral(out procesarMasValidaciones);

            if (!procesarMasValidaciones && _mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            if (_gre.datosEnvio.indicadoresGRERemitente != null)
            {
                _mensajesError.AddMensaje(CodigoError.V0101, "datosEnvio.indicadoresGRERemitente");
                return false;
            }

            if (_gre.transportista == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "transportista");
                return false;
            }
            else
            {
                if (!Validaciones.IsValidRuc(_gre.transportista.ruc))
                {
                    _mensajesError.AddMensaje(CodigoError.V0002, "transportista.ruc");
                }

                if (!Validaciones.IsValidTextSunat(_gre.transportista.razonSocial, 3, 250))
                {
                    _mensajesError.AddMensaje(CodigoError.V0005, "transportista.razonSocial");
                }
            }

            if (_gre.remitente == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "remitente");
                return false;
            }
            else
            {
                if (!Validaciones.IsValidDocumentoIdentidadSunat(_gre.remitente.numeroDocumentoIdentificacion, _gre.remitente.tipoDocumentoIdentificacion))
                {
                    _mensajesError.AddMensaje(CodigoError.V0002, "remitente.numeroDocumentoIdentificacion");
                }

                if (!Validaciones.IsValidTextSunat(_gre.remitente.nombre, 3, 250))
                {
                    _mensajesError.AddMensaje(CodigoError.V0005, "transportista.razonSocial");
                }
            }



            //Si no existen mensajes de Error entonces la validacion esta OK
            return !(_mensajesError.Count > 0);
        }

        private bool ValidarGuiaRemitente()
        {
            bool procesarMasValidaciones;

            //Se usa para saber el numero de item en una interaccion Foreach
            int _idRecord;

            #region Validaciones Generales

            ValidacionGeneral(out procesarMasValidaciones);

            if (!procesarMasValidaciones && _mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            var _mensajes = new List<Error>();

            //No permitir indicadores Transportista
            if (_gre.datosEnvio.indicadoresGRETransportista != null)
            {
                _mensajesError.AddMensaje(CodigoError.V0101, "datosEnvio.indicadoresGRETransportista");
                return false;
            }

            //Validar el remitente
            if (_gre.remitente == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "remitente");
                return false;
            }
            else
            {
                if (_gre.remitente.tipoDocumentoIdentificacion != "6")
                {
                    _mensajesError.AddMensaje(CodigoError.S2511, "remitente.tipoDocumentoIdentificacion");
                    return false;
                }

                if (!Validaciones.IsValidRuc(_gre.remitente.numeroDocumentoIdentificacion))
                {
                    _mensajesError.AddMensaje(CodigoError.V0002);
                    return false;
                }

                if (!Validaciones.IsValidTextSunat(_gre.remitente.nombre, 3, 250))
                {
                    _mensajesError.AddMensaje(CodigoError.V0005, "remitente.razonSocial");
                }
            }

            if (string.IsNullOrEmpty(_gre.datosEnvio.motivoTraslado))
            {
                _mensajesError.AddMensaje(CodigoError.S3404, $"datosEnvio.motivoTraslado = '{_gre.datosEnvio.modalidadTraslado}'");
                return false;
            }
            else if (!OnValidarCatalogoSunat("20", _gre.datosEnvio.motivoTraslado))
            {
                _mensajesError.AddMensaje(CodigoError.S3405, $"datosEnvio.motivoTraslado = '{_gre.datosEnvio.modalidadTraslado}'");
                return false;
            }

            if (string.IsNullOrEmpty(_gre.datosEnvio.modalidadTraslado))
            {
                _mensajesError.AddMensaje(CodigoError.S2532, $"datosEnvio.modalidadTraslado = '{_gre.datosEnvio.modalidadTraslado}'");
                return false;
            }
            else if (!OnValidarCatalogoSunat("18", _gre.datosEnvio.modalidadTraslado))
            {
                _mensajesError.AddMensaje(CodigoError.S2773, $"datosEnvio.modalidadTraslado = '{_gre.datosEnvio.modalidadTraslado}'");
                return false;
            }

            #region  Traslado en vehículos de categoría M1 o L

            if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoVehiculoM1L ?? false) == true)
            {
                if (_gre.datosEnvio.conductores?.Count > 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0033, "datosEnvio.conductores");
                }

                //Si esta asignado la propiedad "vehiculos" se ignora la propiedad "placasVehiculo" en la generacion del XML
                if (_gre.datosEnvio.vehiculos?.Count > 0)
                {
                    //Solo se debe ingresar 1 placa la del vehiculo M1 o L(opcional)
                    if (_gre.datosEnvio.vehiculos.Count > 1)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3453, "datosEnvio.placasVehiculo");
                    }
                }
                else
                {
                    //Solo se debe ingresar 1 placa la del vehiculo M1 o L(opcional)
                    if (_gre.datosEnvio.placasVehiculo?.Count > 1)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3453, "datosEnvio.placasVehiculo");
                    }
                }

                if (_mensajesError.Count > 0)
                {
                    return false;
                }
            }

            #endregion

            #region Transportista

            if (_gre.transportista != null)
            {
                //Si es transporte privado
                if (_gre.datosEnvio.modalidadTraslado == "02")
                {
                    _mensajesError.AddMensaje(CodigoError.S3347, "datosEnvio.transportista");
                    return false;
                }

                if (!Validaciones.IsValidRuc(_gre.transportista.ruc))
                {
                    _mensajesError.AddMensaje(CodigoError.V0002, "transportista.ruc");
                }

                if (!Validaciones.IsValidTextSunat(_gre.transportista.razonSocial, 3, 250))
                {
                    _mensajesError.AddMensaje(CodigoError.V0005, "transportista.razonSocial");
                }

                if (!Validaciones.IsValidRegistroMTC(_gre.transportista.registroMTC))
                {
                    _mensajesError.AddMensaje(CodigoError.S4392, "transportista.registroMTC");
                }

                if (_mensajesError.Count > 0)
                {
                    return false;
                }
            }
            else
            {
                //Si es transporte publico
                if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoVehiculoM1L ?? false) == false && _gre.datosEnvio.modalidadTraslado == "01")
                {
                    _mensajesError.AddMensaje(CodigoError.V0034, "datosEnvio.transportista");
                    return false;
                }
            }

            #endregion

            #region Fecha inicio translado

            if (_gre.datosEnvio.fechaInicioTraslado.Date < _gre.fechaEmision.Date)
            {
                _mensajesError.AddMensaje(CodigoError.S3343);
                return false;
            }

            #endregion

            #region Punto de Partida

            if (!Validaciones.IsValidTextSunat(_gre.datosEnvio.puntoPartida.direccion, 3, 500))
            {
                _mensajesError.AddMensaje(CodigoError.V0004, "datosEnvio.puntoPartida.direccion");
            }

            if (_gre.datosEnvio.motivoTraslado == "04")
            {
                if (_gre.datosEnvio.puntoPartida.rucAsociado != _gre.remitente.numeroDocumentoIdentificacion)
                {
                    _mensajesError.AddMensaje(CodigoError.S3414, "gre.datosEnvio.puntoPartida.rucAsociado");
                }

                //Codigo de establecimiento es obligatorio
                if (string.IsNullOrEmpty(_gre.datosEnvio.puntoPartida.codigoEstablecimiento))
                {
                    _mensajesError.AddMensaje(CodigoError.S3365, "datosEnvio.puntoPartida.codigoEstablecimiento");
                }
            }

            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            #region Punto de Llegada

            if (_gre.datosEnvio.puntoLlegada == null && _gre.datosEnvio.motivoTraslado != "18")
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "datosEnvio.puntoLlegada");
                return false;
            }

            if (_gre.datosEnvio.puntoLlegada != null)
            {
                if (!Validaciones.IsValidUbigeo(_gre.datosEnvio.puntoLlegada.ubigeo))
                {
                    _mensajesError.AddMensaje(CodigoError.S2776, "datosEnvio.puntoLlegada.ubigeo");
                }

                if (!Validaciones.IsValidTextSunat(_gre.datosEnvio.puntoLlegada.direccion, 3, 500))
                {
                    _mensajesError.AddMensaje(CodigoError.V0004, "datosEnvio.puntoLlegada.direccion");
                }

                if (_gre.datosEnvio.motivoTraslado == "04")
                {
                    if (_gre.datosEnvio.puntoLlegada.rucAsociado != _gre.remitente.numeroDocumentoIdentificacion)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3414, "gre.datosEnvio.puntoLlegada.rucAsociado");
                    }

                    //Codigo de establecimiento es obligatorio
                    if (string.IsNullOrEmpty(_gre.datosEnvio.puntoLlegada.codigoEstablecimiento))
                    {
                        _mensajesError.AddMensaje(CodigoError.S3365, "datosEnvio.puntoLlegada.codigoEstablecimiento");
                    }
                }
            }

            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            #region Vehiculo principal/secundarios

            //Si esta asignado la propiedad "vehiculos" se ignora la propiedad "placasVehiculo" en la generacion del XML
            if (_gre.datosEnvio.vehiculos?.Count > 0)
            {
                //Maximos 3 placas 1 principal y 2 secundarias
                if (_gre.datosEnvio.vehiculos.Count > 3)
                {
                    _mensajesError.AddMensaje(CodigoError.V0031);
                }
                else
                {
                    _idRecord = 0;

                    foreach (var item in _gre.datosEnvio.vehiculos)
                    {
                        if (!Validaciones.IsValidPlacaSunat(item.numeroPlaca ?? ""))
                        {
                            _mensajesError.AddMensaje(CodigoError.S2567, $"datosEnvio.vehiculos[{_idRecord}].numeroPlaca = {item}");
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.numeroTarjeta))
                            {
                                //Si existe el Indicador de registro de vehículos y conductores del transportista es falso no se debe registrar el Tuc
                                if ((_gre.datosEnvio.indicadoresGRERemitente?.indVehiculoConductoresTransp ?? false) == false)
                                {
                                    //No se debe ingresar el Tuc
                                    _mensajes.AddMensaje(CodigoError.V0035, $"datosEnvio.vehiculos[{_idRecord}].numeroTarjeta");
                                }
                                else
                                {
                                    //Validamos el formato del Tuc
                                    if (!Validaciones.IsValidTuc(item.numeroTarjeta))
                                    {
                                        _mensajesError.AddMensaje(CodigoError.S3355, $"datosEnvio.vehiculos[{_idRecord}].numeroTarjeta = {item}");
                                    }
                                }
                            }
                            else
                            {
                                if (_gre.datosEnvio.modalidadTraslado == "01"
                                    && (_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoVehiculoM1L ?? false) == false
                                    && (_gre.datosEnvio.indicadoresGRERemitente?.indVehiculoConductoresTransp ?? false) == true)
                                {
                                    _mensajesError.AddMensaje(CodigoError.S4399, $"datosEnvio.vehiculos[{_idRecord}].numeroTarjeta");
                                }
                            }

                            if (item.autorizacionesEspeciales?.Count > 0)
                            {
                                if (item.autorizacionesEspeciales.Count > 1)
                                {
                                    _mensajesError.AddMensaje(CodigoError.S3356, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales.Count > 1");
                                    continue;
                                }

                                if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoVehiculoM1L ?? false) == true)
                                {
                                    //No se debe ingresar el Tuc
                                    _mensajes.AddMensaje(CodigoError.V0035, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales");
                                    continue;
                                }

                                if (_gre.datosEnvio.modalidadTraslado == "01"
                                    && (_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoVehiculoM1L ?? false) == false
                                    && (_gre.datosEnvio.indicadoresGRERemitente?.indVehiculoConductoresTransp ?? false) == false)
                                {
                                    _mensajesError.AddMensaje(CodigoError.V0035, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales");
                                    continue;
                                }

                                //En un futuro SUNAT podria permitir mas de una Autorizacion(estandar UBL lo permite) por eso el bloque foreach
                                var _idRecordAutorizacion = 0;

                                foreach (var autorizacion in item.autorizacionesEspeciales)
                                {
                                    if (!string.IsNullOrEmpty(autorizacion.codigoEntidadAutorizadora))
                                    {
                                        if (!OnValidarCatalogoSunat("D37", autorizacion.codigoEntidadAutorizadora))
                                        {
                                            _mensajes.AddMensaje(CodigoError.S4407, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales[{_idRecordAutorizacion}].codigoEntidadAutorizadora = '{autorizacion.codigoEntidadAutorizadora}'");
                                        }
                                    }
                                    else
                                    {
                                        _mensajes.AddMensaje(CodigoError.S4403, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales[{_idRecordAutorizacion}].codigoEntidadAutorizadora = '{autorizacion.codigoEntidadAutorizadora}'");
                                    }

                                    if (!Validaciones.IsValidAutorizacionEspecial(autorizacion.numeroAutorizacion))
                                    {
                                        _mensajes.AddMensaje(CodigoError.S4406, $"datosEnvio.vehiculos[{_idRecord}].autorizacionesEspeciales[{_idRecordAutorizacion}].numeroAutorizacion = '{autorizacion.codigoEntidadAutorizadora}'");
                                    }

                                    _idRecordAutorizacion++;
                                }
                            }
                        }

                        _idRecord++;
                    }
                }
            }
            else
            {
                //La propiedad "placasVehiculo" se usaba para llenar las placas de los vehiculos esta validacion esta validacion es solo por compatibilidad
                //con usuarios antiguos de la DLL
                if (_gre.datosEnvio.placasVehiculo?.Count > 0)
                {
                    //Maximos 3 placas 1 principal y 2 secundarias
                    if (_gre.datosEnvio.placasVehiculo.Count > 3)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0031);
                    }
                    else
                    {
                        _idRecord = 0;

                        foreach (var item in _gre.datosEnvio.placasVehiculo)
                        {
                            if (!Validaciones.IsValidPlacaSunat(item ?? ""))
                            {
                                _mensajesError.AddMensaje(CodigoError.S2567, $"datosEnvio.placasVehiculo[{_idRecord}] = {item}");
                            }
                            _idRecord++;
                        }
                    }
                }
            }

            if (_mensajesError.Count > 0)
            {
                return false;
            }


            #endregion

            #region Conductores

            if (_gre.datosEnvio.conductores?.Count > 0)
            {
                //Maximos 3 conductores 1 principal y 2 secundarias
                if (_gre.datosEnvio.conductores.Count > 3)
                {
                    _mensajesError.AddMensaje(CodigoError.V0032);
                }
                else
                {
                    _idRecord = 0;

                    foreach (var item in _gre.datosEnvio.conductores)
                    {
                        if (item.tipoDocumentoIdentificacion != "1" &&
                            item.tipoDocumentoIdentificacion != "4" &&
                            item.tipoDocumentoIdentificacion != "7" &&
                            item.tipoDocumentoIdentificacion != "A")
                        {
                            _mensajesError.AddMensaje(CodigoError.V0001, $"datosEnvio.conductores[{_idRecord}].tipoDocumentoIdentificacion");
                            continue;
                        }

                        if (!Validaciones.IsValidDocumentoIdentidadSunat(item.numeroDocumentoIdentificacion, item.tipoDocumentoIdentificacion))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0037, $"datosEnvio.conductores[{_idRecord}].numeroDocumentoIdentificacion");
                        }

                        if (!Validaciones.IsValidTextSunat(item.nombres, 3, 250))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0005, $"datosEnvio.conductores[{_idRecord}].nombres");
                        }

                        if (!Validaciones.IsValidTextSunat(item.apellidos, 3, 250))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0005, $"datosEnvio.conductores[{_idRecord}].apellidos");
                        }

                        if (!Validaciones.IsValidLicenciaConducirSunat(item.licenciaConducir ?? ""))
                        {
                            _mensajesError.AddMensaje(CodigoError.S2573, $"datosEnvio.conductores[{_idRecord}].licenciaConducir");
                        }

                        _idRecord++;
                    }
                }

                if (_mensajesError.Count > 0)
                {
                    return false;
                }
            }

            #endregion

            if (_gre.datosEnvio.motivoTraslado == "08" || _gre.datosEnvio.motivoTraslado == "09")
            {
                //El tipo de operacion es importacion/ exportacion y se requieren documentos relacionados
                if (_gre.documentosRelacionados?.Count == 0)
                {
                    _mensajesError.AddMensaje(CodigoError.S3440, "documentosRelacionados");
                    return false;
                }
                else
                {
                    _idRecord = 0;
                    bool _existeDAMoDS = false;
                    var _docRelacionadosPermitidos = new List<string>() { "09", "49", "50", "52", "80" };

                    foreach (var item in _gre.documentosRelacionados)
                    {
                        if (item.tipoDocumento == "50" || item.tipoDocumento == "52")
                        {
                            if (!Validaciones.IsValidNumeroDeclaracionAduana(item.numeroDocumento))
                            {
                                _mensajesError.AddMensaje(CodigoError.S2769, $"documentosRelacionados[{_idRecord}].numeroDocumento");
                            }
                            else
                            {
                                _existeDAMoDS = true;
                            }
                            _idRecord++;
                            continue;
                        }

                        if (!_docRelacionadosPermitidos.Contains(item.tipoDocumento))
                        {
                            _mensajesError.AddMensaje(CodigoError.S3445, $"documentosRelacionados[{_idRecord}].tipoDocumento");
                        }

                        _idRecord++;
                    }

                    if (!_existeDAMoDS && _mensajesError.Count == 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3440, "documentosRelacionados");
                    }
                }

                if (_mensajesError.Count > 0)
                {
                    return false;
                }
            }
            else
            {
                //El tipo de operacion no es importacion/ exportacion y los documentos relacionados son opcionales
                _idRecord = 0;
                bool _debeExistirItemDAMoDS = false;
                var _docRelacionadosNoPermitidos = new List<string>() { "50", "52" };

                if (_gre.documentosRelacionados?.Count > 0)
                {
                    foreach (var item in _gre.documentosRelacionados)
                    {
                        //En este punto el motivo de traslado ya es diferente de "08" y "09"
                        if (_gre.datosEnvio.motivoTraslado != "13")
                        {
                            if (_docRelacionadosNoPermitidos.Contains(item.tipoDocumento))
                            {
                                _mensajesError.AddMensaje(CodigoError.S3445, $"documentosRelacionados[{_idRecord}].tipoDocumento");
                                _idRecord++;
                                continue;
                            }
                        }

                        if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoTotalDAMoDS ?? false) == false && (item.tipoDocumento == "49" || item.tipoDocumento == "80"))
                        {
                            _debeExistirItemDAMoDS = true;
                        }

                        _idRecord++;
                    }
                }

                if (_debeExistirItemDAMoDS && _mensajesError.Count == 0)
                {
                    var _totalItemsConDAMoDS = _gre.detalles?.Select(x => !string.IsNullOrEmpty(x.partidaArancelaria)).Count();

                    if ((_totalItemsConDAMoDS ?? 0) == 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3352);
                    }
                }

                if (_mensajesError.Count > 0)
                {
                    return false;
                }
            }

            #region Detalles

            _idRecord = 0;

            foreach (var item in _gre.detalles)
            {
                if (!OnValidarCatalogoSunat("03", item.unidadMedida))
                {
                    _mensajesError.AddMensaje(CodigoError.V0029, $"detalle[{_idRecord}].unidadMedida");
                }

                if (item.cantidad == 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0036, $"detalle[{_idRecord}].cantidad");
                }

                if (!Validaciones.IsValidTextSunat(item.nombre, 3, 500))
                {
                    _mensajesError.AddMensaje(CodigoError.V0005, $"detalle[{_idRecord}].nombre");
                }

                if (!string.IsNullOrEmpty(item.partidaArancelaria))
                {
                    if (!Validaciones.IsValidPartidaArancelaria(item.partidaArancelaria))
                    {
                        _mensajesError.AddMensaje(CodigoError.S3377, $"detalle[{_idRecord}].partidaArancelaria");
                    }
                    else
                    {
                        //Validar error 3429: La Partida arancelaria no esta en el listado de bienes normalizados(Catalogo 62)

                    }
                }
                else
                {
                    if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoTotalDAMoDS ?? false) == false && item.esBienNormalizado)
                    {
                        _mensajesError.AddMensaje(CodigoError.S3426, $"detalle[{_idRecord}].partidaArancelaria");
                    }
                }

                if (!string.IsNullOrEmpty(item.numeroDeclaracionAduanera))
                {
                    if (_gre.datosEnvio.motivoTraslado != "08" && _gre.datosEnvio.motivoTraslado != "09")
                    {
                        _mensajesError.AddMensaje(CodigoError.V0101, $"detalle[{_idRecord}].numeroDeclaracionAduanera");
                    }
                    else
                    {
                        if (!Validaciones.IsValidNumeroDeclaracionAduana(item.numeroDeclaracionAduanera))
                        {
                            _mensajesError.AddMensaje(CodigoError.S2769, $"detalle[{_idRecord}].numeroDeclaracionAduanera");
                        }
                        else
                        {
                            if ((_gre.datosEnvio.indicadoresGRERemitente?.indTrasladoTotalDAMoDS ?? false) == false && (_gre.datosEnvio.motivoTraslado == "08" || _gre.datosEnvio.motivoTraslado != "09"))
                            {
                                var _docRelacionadosConDAMoDS = _gre.documentosRelacionados?.Select(x => (x.tipoDocumento == "50" || x.tipoDocumento == "52") && x.numeroDocumento == item.numeroDeclaracionAduanera).Count();

                                if ((_docRelacionadosConDAMoDS ?? 0) == 0)
                                {
                                    _mensajesError.AddMensaje(CodigoError.S3430, $"detalle[{_idRecord}].numeroDeclaracionAduanera");
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(item.numeroSerieEnDeclaracionAduanera))
                {
                    if (_gre.datosEnvio.motivoTraslado != "08" && _gre.datosEnvio.motivoTraslado != "09")
                    {
                        _mensajesError.AddMensaje(CodigoError.V0101, $"detalle[{_idRecord}].numeroSerieEnDeclaracionAduanera");
                    }
                    else
                    {
                        if (!Validaciones.IsValidSerieEnDeclaracionAduana(item.numeroSerieEnDeclaracionAduanera))
                        {
                            _mensajesError.AddMensaje(CodigoError.S3431, $"detalle[{_idRecord}].numeroSerieEnDeclaracionAduanera");
                        }
                    }
                }

                _idRecord++;
            }

            #endregion

            //Si no existen mensajes de Error entonces la validacion esta OK
            return !(_mensajesError.Count > 0);
        }
    }
}
