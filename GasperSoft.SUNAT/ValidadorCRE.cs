// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.CRE;
using System;
using System.Collections.Generic;
using System.Linq;
using static GasperSoft.SUNAT.Validaciones;

namespace GasperSoft.SUNAT
{
    public class ValidadorCRE
    {
        private readonly CREType _cre;
        private readonly List<Error> _mensajesError;
        private decimal _toleranciaCalculo = 0.2m;

        public ValidadorCRE(CREType cre)
        {
            _cre = cre;
            _mensajesError = new List<Error>();
        }

        public decimal ToleranciaCalculo
        {
            get
            {
                return _toleranciaCalculo;
            }
            set
            {
                _toleranciaCalculo = value;

                if (_toleranciaCalculo < 0.20m)
                {
                    _toleranciaCalculo = 0.20m;
                }

                if (_toleranciaCalculo > 1)
                {
                    _toleranciaCalculo = 1;
                }
            }
        }

        public List<Error> Errors
        {
            get
            {
                return _mensajesError;
            }
        }

        /// <summary>
        /// Valida que codigoRegimenRetencion exista y que la tasa corresponda a este codigo
        /// </summary>
        public event ValidarTasaRetencion OnValidarTasaRetencion;

        public bool Validar()
        {
            _mensajesError.Clear();

            int _idRecord;

            //No existe validacion segun la pagina oficial de SUNAT
            if (_cre.fechaEmision.Date > DateTime.Now.Date)
            {
                _mensajesError.AddMensaje(CodigoError.S2329);
                return false;
            }

            if (_cre.detalles?.Count == 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0007);
                return false;
            }

            if (_cre.proveedor == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "proveedor");
                return false;
            }


            if (OnValidarTasaRetencion != null)
            {
                if (!OnValidarTasaRetencion(_cre.codigoRegimenRetencion, _cre.tasaRetencion))
                {
                    _mensajesError.AddMensaje(CodigoError.V0039, $"codigoRegimenRetencion = '{_cre.codigoRegimenRetencion}', tasaRetencion = '{_cre.tasaRetencion}'");
                    return false;
                }
            }

            #region Validacion del Proveedor

            if (!Validaciones.IsValidTipoDocumentoIdentidad(_cre.proveedor.tipoDocumentoIdentificacion))
            {
                _mensajesError.AddMensaje(CodigoError.V0001, $"proveedor.tipoDocumentoIdentificacion = '{_cre.proveedor.tipoDocumentoIdentificacion}'");
                return false;
            }

            if (!Validaciones.IsValidDocumentoIdentidadSunat(_cre.proveedor.numeroDocumentoIdentificacion, _cre.proveedor.tipoDocumentoIdentificacion))
            {
                if (_cre.proveedor.tipoDocumentoIdentificacion == "6")
                {
                    _mensajesError.AddMensaje(CodigoError.V0002, $"proveedor.numeroDocumentoIdentificacion = '{_cre.proveedor.numeroDocumentoIdentificacion}'");
                    return false;
                }
                else
                {
                    _mensajesError.AddMensaje(CodigoError.V0037, $"proveedor.numeroDocumentoIdentificacion = '{_cre.proveedor.numeroDocumentoIdentificacion}'");
                    return false;
                }
            }

            if (!Validaciones.IsValidTextSunat(_cre.proveedor.nombre, 3, 1500))
            {
                _mensajesError.AddMensaje(CodigoError.V0006, $"proveedor.nombre = '{_cre.proveedor.nombre}'");
                return false;
            }

            if (_cre.proveedor.tipoDocumentoIdentificacion == "-")
            {
                _mensajesError.AddMensaje(CodigoError.V0037, "proveedor.tipoDocumentoIdentificacion");
                return false;
            }

            #endregion

            #region Validacion de Propiedades del comprobante

            if (_cre.fechaEmision == new DateTime())
            {
                _mensajesError.AddMensaje(CodigoError.V0102, "fechaEmision");
            }

            if (!_cre.serie.StartsWith("R"))
            {
                _mensajesError.AddMensaje(CodigoError.V0009);
            }

            if (_cre.numero.ToString().Length > 8)
            {
                _mensajesError.AddMensaje(CodigoError.V0010);
            }

            //Aqui la moneda es valida
            if (_cre.codMoneda != "PEN")
            {
                _mensajesError.AddMensaje(CodigoError.V0038, $"codMoneda = {_cre.codMoneda}");
            }

            #region Validar que se envíen valores positivos y con longitud decimal correcta

            if (_cre.tasaRetencion < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "tasaRetencion");
            }

            if (_cre.importeTotalRetenido < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "importeTotalRetenido");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cre.importeTotalRetenido, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "importeTotalRetenido");
            }

            if (_cre.importeTotalPagado < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "importeTotalPagado");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cre.importeTotalPagado, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "importeTotalPagado");
            }

            #endregion

            //Aqui si ya muchos erros no continuamos y notificamos al usuario
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            #region Detalles del comprobante

            _idRecord = 0;

            //Segun las validaciones de SUNAT solo estos documentos
            var _documentosPermitidos = new List<string> { "01", "12", "07", "08", "20" };

            foreach (var item in _cre.detalles)
            {
                if (!_documentosPermitidos.Contains(item.documentoRelacionadoTipoDocumento))
                {
                    _mensajesError.AddMensaje(CodigoError.S2692, $"detalle[{_idRecord}].documentoRelacionadoTipoDocumento");
                }

                bool _valorCalculoValidado = true;

                #region Validacion de longitud decimal

                if (item.pagoTotalSinRetencion < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].pagoTotalSinRetencion");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.pagoTotalSinRetencion, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].pagoTotalSinRetencion");
                    _valorCalculoValidado = false;
                }

                if (item.importeRetenido < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].importeRetenido");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.importeRetenido, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].importeRetenido");
                    _valorCalculoValidado = false;
                }

                if (item.importePagadoConRetencion < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].importePagadoConRetencion");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.importePagadoConRetencion, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].importePagadoConRetencion");
                    _valorCalculoValidado = false;
                }

                if (item.tipoCambio != null)
                {
                    if (item.tipoCambio.factorConversion < 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].tipoCambio.factorConversion");
                        _valorCalculoValidado = false;
                    }
                    else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.tipoCambio.factorConversion, 6))
                    {
                        _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].tipoCambio.factorConversion");
                        _valorCalculoValidado = false;
                    }
                }
                else
                {
                    if (item.documentoRelacionadoCodMoneda != "PEN")
                    {
                        _mensajesError.AddMensaje(CodigoError.V0102, $"detalle[{_idRecord}].tipoCambio");
                        _valorCalculoValidado = false;
                    }
                }

                #endregion

                if (_valorCalculoValidado)
                {
                    var _importeRetenidoCalculado = item.pagoTotalSinRetencion * _cre.tasaRetencion / 100;
                    var _importePagadoConRetencion = item.pagoTotalSinRetencion - _importeRetenidoCalculado;

                    if (item.documentoRelacionadoCodMoneda != "PEN")
                    {
                        _importeRetenidoCalculado = _importeRetenidoCalculado * item.tipoCambio.factorConversion;

                        if (!Validaciones.ValidarToleranciaCalculo(item.importeRetenido, decimal.Round(_importeRetenidoCalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"importeRetenido incorrecto en la linea {_idRecord}, Valor enviado: {item.importeRetenido} Valor calculado: {decimal.Round(_importeRetenidoCalculado, 2)}; Formula: importeRetenido = (pagoTotalSinRetencion * cre.tasaRetencion / 100) * tipoCambio.factorConversion");
                            continue;
                        }

                        _importePagadoConRetencion = (item.pagoTotalSinRetencion * item.tipoCambio.factorConversion) - _importeRetenidoCalculado;

                        if (!Validaciones.ValidarToleranciaCalculo(item.importePagadoConRetencion, decimal.Round(_importePagadoConRetencion, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"importePagadoConRetencion incorrecto en la linea {_idRecord}, Valor enviado: {item.importePagadoConRetencion} Valor calculado: {decimal.Round(_importePagadoConRetencion, 2)}; Formula: importePagadoConRetencion = (pagoTotalSinRetencion * tipoCambio.factorConversion) - importeRetenido");
                            continue;
                        }
                    }
                    else
                    {
                        if (!Validaciones.ValidarToleranciaCalculo(item.importeRetenido, decimal.Round(_importeRetenidoCalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"importeRetenido incorrecto en la linea {_idRecord}, Valor enviado: {item.importeRetenido} Valor calculado: {decimal.Round(_importeRetenidoCalculado, 2)}; Formula: importeRetenido = pagoTotalSinRetencion * cre.tasaRetencion / 100");
                            continue;
                        }

                        if (!Validaciones.ValidarToleranciaCalculo(item.importePagadoConRetencion, decimal.Round(_importePagadoConRetencion, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"importePagadoConRetencion incorrecto en la linea {_idRecord}, Valor enviado: {item.importePagadoConRetencion} Valor calculado: {decimal.Round(_importePagadoConRetencion, 2)}; Formula: importePagadoConRetencion = pagoTotalSinRetencion - importeRetenido");
                            continue;
                        }
                    }
                }

                _idRecord++;
            }

            //Existe algun error en los detalles entonces ya no continuamos
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            #region validaciones de totales

            if (Math.Abs(_cre.totalRedondeoImporteTotalPagado) > 1)
            {
                _mensajesError.AddMensaje(CodigoError.S3303, "ValorAbsoluto(totalRedondeoImporteTotalPagado) no puede ser mayor a 1");
                return false;
            }

            var _importeTotalPagadoCalculado = Math.Truncate((_cre.detalles.Where(x => x.documentoRelacionadoTipoDocumento != "07" && x.documentoRelacionadoTipoDocumento != "20").Sum(x => x.importePagadoConRetencion) + _cre.totalRedondeoImporteTotalPagado) * 100) / 100;

            if (_importeTotalPagadoCalculado != _cre.importeTotalPagado)
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"importeTotalPagado incorrecto Valor enviado: {_cre.importeTotalPagado} Valor calculado: {_importeTotalPagadoCalculado}; Formula: importeTotalPagado = Suma del 'importePagadoConRetencion' de cada detalle (sin considerar los tipos de documentos '07' y '20') + totalRedondeoImporteTotalPagado");
                return false;
            }

            var _importeTotalRetenidoCalculado = Math.Truncate(_cre.detalles.Where(x => x.documentoRelacionadoTipoDocumento != "07" && x.documentoRelacionadoTipoDocumento != "20").Sum(x => x.importeRetenido) * 100) / 100;

            if (_importeTotalRetenidoCalculado != _cre.importeTotalRetenido)
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"importeTotalRetenido incorrecto Valor enviado: {_cre.importeTotalRetenido} Valor calculado: {_importeTotalRetenidoCalculado}; Formula: importeTotalRetenido = Suma del 'importeRetenido' de cada detalle (sin considerar los tipos de documentos '07' y '20')");
                return false;
            }

            #endregion

            //Si no existen mensajes de Error entonces la validacion esta OK
            return !(_mensajesError.Count > 0);
        }
    }
}
