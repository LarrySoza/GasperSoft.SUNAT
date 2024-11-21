// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static GasperSoft.SUNAT.DTO.Validar.Validaciones;

namespace GasperSoft.SUNAT.DTO.Validar
{
    public class ValidadorCPE
    {
        private decimal _montoMaximoClienteAnonimoBoleta = 700;
        private string _codigoMonedaNacional = "PEN";
        private readonly CPEType _cpe;
        private readonly List<Error> _mensajesError;
        private decimal _tipoCambioReferencial = 0;
        private decimal _toleranciaCalculo = 0.2m;

        /// <summary>
        /// Convierte un valor a moneda nacional
        /// </summary>
        /// <param name="codMoneda">La moneda en la que se encuenta el valor de la variable monto</param>
        /// <param name="monto">el monto a convertir</param>
        /// <param name="tipoCambio">el tipo de cambio a aplicar</param>
        /// <returns></returns>
        private decimal ConvertirMonedaNacional(string codMoneda, decimal monto, decimal tipoCambio)
        {
            if (codMoneda == "PEN")
            {
                return monto;
            }
            else if (codMoneda == "USD")
            {
                return monto * tipoCambio;
            }
            else
            {
                throw new Exception($"No esta implementado la conversion para el tipo de moneda '{codMoneda}'");
            }
        }


        /// <summary>
        /// Inicia una nueva instancia de la clave ValidadorCPE
        /// </summary>
        /// <param name="cpe">El Cpe a Validar</param>
        public ValidadorCPE(CPEType cpe)
        {
            _cpe = cpe;
            _mensajesError = new List<Error>();
        }

        public decimal TipoCambioReferencial
        {
            set
            {
                _tipoCambioReferencial = value;
            }
            get
            {
                return _tipoCambioReferencial;
            }
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
        /// Validar que un valor se encuentre en un determinado catalogo de SUNAT
        /// </summary>
        public event ValidarCatalogoSunat OnValidarCatalogoSunat;

        public bool Validar()
        {
            _mensajesError.Clear();
            
            if (OnValidarCatalogoSunat == null)
            {
                throw new Exception("Debe asociar el evento 'OnValidarCatalogoSunat' para la validación con los catálogos SUNAT");
            }

            //Aqui la moneda es valida
            if (_cpe.codMoneda != _codigoMonedaNacional)
            {
                if (_tipoCambioReferencial == 0)
                {
                    throw new Exception($"Debe asignar un valor mayor a '0' a la propiedad 'tipoCambioReferencial' cuando codMoneda es diferente de {_codigoMonedaNacional}");
                }
            }

            int _idRecord;

            //Segun la documentacion oficinal de SUNAT se enviar hasta 2 dias antes el CPE
            if (_cpe.fechaEmision.Date.Subtract(DateTime.Now.Date).TotalDays > 2)
            {
                _mensajesError.AddMensaje(CodigoError.S2329);
                return false;
            }

            if (!string.IsNullOrEmpty(_cpe.horaEmision))
            {
                if (!Validaciones.IsValidTimeSunat(_cpe.horaEmision))
                {
                    _mensajesError.AddMensaje(CodigoError.S3438, "horaEmision");
                }
            }

            if (_cpe.detalles?.Count == 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0007);
                return false;
            }

            if (_cpe.adquirente == null)
            {
                _mensajesError.AddMensaje(CodigoError.V0103, "adquirente");
                return false;
            }

            var _esExportacion = false;

            #region Validacion del Adquirente

            if (!Validaciones.IsValidTipoDocumentoIdentidad(_cpe.adquirente.tipoDocumentoIdentificacion))
            {
                _mensajesError.AddMensaje(CodigoError.S2800, "adquirente.tipoDocumentoIdentificacion");
                return false;
            }

            if (!Validaciones.IsValidDocumentoIdentidadSunat(_cpe.adquirente.numeroDocumentoIdentificacion, _cpe.adquirente.tipoDocumentoIdentificacion))
            {
                if (_cpe.adquirente.tipoDocumentoIdentificacion == "6")
                {
                    _mensajesError.AddMensaje(CodigoError.S2017, "adquirente.numeroDocumentoIdentificacion");
                    return false;
                }
                else if (_cpe.adquirente.tipoDocumentoIdentificacion == "1")
                {
                    _mensajesError.AddMensaje(CodigoError.S2801, "adquirente.numeroDocumentoIdentificacion");
                    return false;
                }
                else
                {
                    _mensajesError.AddMensaje(CodigoError.S2802, "adquirente.numeroDocumentoIdentificacion");
                    return false;
                }
            }

            if (!Validaciones.IsValidTextSunat(_cpe.adquirente.nombre, 3, 1500))
            {
                _mensajesError.AddMensaje(CodigoError.V0006, "adquirente.nombre");
                return false;
            }

            #endregion

            #region Validacion de Propiedades del comprobante

            if (string.IsNullOrEmpty(_cpe.codigoEstablecimiento))
            {
                _mensajesError.AddMensaje(CodigoError.S3030, "codigoEstablecimiento");
                return false;
            }

            if (_cpe.tipoDocumento == "01" || _cpe.tipoDocumento == "03")
            {
                if (string.IsNullOrEmpty(_cpe.codigoTipoOperacion))
                {
                    _mensajesError.AddMensaje(CodigoError.S3205, "codigoTipoOperacion");
                    return false;
                }
                else
                {
                    //Validar que codigoTipoOperacion se encuentre en el catálogo N° 51
                    if (!OnValidarCatalogoSunat("51", _cpe.codigoTipoOperacion))
                    {
                        _mensajesError.AddMensaje(CodigoError.S3206, "codigoTipoOperacion");
                        return false;
                    }

                    //Si comienza con 02 entonces es una exportacion
                    if (_cpe.codigoTipoOperacion.StartsWith("02"))
                    {
                        _esExportacion = true;
                    }
                }
            }
            else
            {
                //Es una Nota de credito o debito si algunos de sus items es de exportacion entonces es un comprobante de exportacion
                var itemExportacion = _cpe.detalles.Where(x => x.codAfectacionIGV == "40").FirstOrDefault();

                if (itemExportacion != null)
                {
                    _esExportacion = true;
                }
            }

            if (_cpe.fechaEmision == new DateTime())
            {
                _mensajesError.AddMensaje(CodigoError.V0103, "fechaEmision");
            }

            var documentosPermitidos = new List<string> { "01", "03", "07", "08" };

            if (!documentosPermitidos.Contains(_cpe.tipoDocumento))
            {
                _mensajesError.AddMensaje(CodigoError.V0027, "tipoDocumento");
                return false;
            }

            //Si no es entero entonces es una serie de documento electronico y debemos validarlo
            if (!Validaciones.IsInteger(_cpe.serie.Substring(0, 1)))
            {
                if (!Validaciones.IsValidSeries(_cpe.tipoDocumento, _cpe.serie))
                {
                    _mensajesError.AddMensaje(CodigoError.V0009);
                }
            }

            if (_cpe.numero.ToString().Length > 8)
            {
                _mensajesError.AddMensaje(CodigoError.V0010);
            }

            if (!string.IsNullOrEmpty(_cpe.ordenCompra))
            {
                if (!Validaciones.IsValidOrdenCompra(_cpe.ordenCompra))
                {
                    _mensajesError.AddMensaje(CodigoError.S4233);
                }
            }

            //Validar que codMoneda se encuentre en el catálogo N° 02
            if (!OnValidarCatalogoSunat("02", _cpe.codMoneda))
            {
                _mensajesError.AddMensaje(CodigoError.V0028, "codMoneda");
            }

            #region Validar que se envíen valores positivos y con longitud decimal correcta

            if (_cpe.tasaDescuentoGlobal < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "tasaDescuentoGlobal");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.tasaDescuentoGlobal, 5))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "tasaDescuentoGlobal");
            }

            if (_cpe.descuentoGlobalAfectaBI != null)
            {
                if (_cpe.descuentoGlobalAfectaBI.montoBase < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "descuentoGlobalAfectaBI.montoBase");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.descuentoGlobalAfectaBI.montoBase, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "descuentoGlobalAfectaBI.montoBase");
                }

                if (_cpe.descuentoGlobalAfectaBI.importe < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "descuentoGlobalAfectaBI.importe");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.descuentoGlobalAfectaBI.importe, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "descuentoGlobalAfectaBI.importe");
                }
            }
            if (_cpe.totalOperacionesGravadas < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalOperacionesGravadas");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalOperacionesGravadas, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalOperacionesGravadas");
            }

            if (_cpe.totalOperacionesExoneradas < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalOperacionesExoneradas");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalOperacionesExoneradas, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalOperacionesExoneradas");
            }

            if (_cpe.totalOperacionesInafectas < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalOperacionesInafectas");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalOperacionesInafectas, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalOperacionesInafectas");
            }

            if (_cpe.totalOperacionesExportacion < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalOperacionesExportacion");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalOperacionesExportacion, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalOperacionesExportacion");
            }

            if (_cpe.totalOperacionesGratuitas < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalOperacionesGratuitas");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalOperacionesGratuitas, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalOperacionesGratuitas");
            }

            if (_cpe.totalDescuentosNoAfectaBI < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "totalDescuentosNoAfectaBI");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.totalDescuentosNoAfectaBI, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "totalDescuentosNoAfectaBI");
            }

            if (_cpe.sumatoriaISC < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "sumatoriaISC");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.sumatoriaISC, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "sumatoriaISC");
            }

            if (_cpe.sumatoriaIGV < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "sumatoriaIGV");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.sumatoriaIGV, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "sumatoriaIGV");
            }

            if (_cpe.sumatoriaOTH < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "sumatoriaOTH");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.sumatoriaOTH, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "sumatoriaOTH");
            }

            if (_cpe.sumatoriaOtrosCargosNoAfectaBI < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "sumatoriaOtrosCargosNoAfectaBI");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.sumatoriaOtrosCargosNoAfectaBI, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "sumatoriaOtrosCargosNoAfectaBI");
            }

            if (_cpe.importeTotal < 0)
            {
                _mensajesError.AddMensaje(CodigoError.V0012, "importeTotal");
            }
            else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.importeTotal, 2))
            {
                _mensajesError.AddMensaje(CodigoError.V0011, "importeTotal");
            }

            if (_cpe.percepcion != null)
            {
                if (_cpe.percepcion.tasa < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "percepcion.tasa");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.percepcion.tasa, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "percepcion.tasa");
                }

                if (_cpe.percepcion.importe < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "percepcion.importe");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.percepcion.importe, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "percepcion.importe");
                }

                if (_cpe.percepcion.montoBase < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "percepcion.montoBase");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.percepcion.montoBase, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "percepcion.montoBase");
                }
            }

            if (_cpe.retencion != null)
            {
                if (_cpe.retencion.tasa < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "retencion.tasa");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.retencion.tasa, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "retencion.tasa");
                }

                if (_cpe.retencion.importe < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "retencion.importe");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.retencion.importe, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "retencion.importe");
                }

                if (_cpe.retencion.montoBase < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "retencion.montoBase");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.retencion.montoBase, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "retencion.montoBase");
                }
            }

            if (_cpe.detraccion != null)
            {
                if (string.IsNullOrEmpty(_cpe.detraccion.numeroCuentaBancoNacion))
                {
                    _mensajesError.AddMensaje(CodigoError.S3034, "detraccion.numeroCuentaBancoNacion");
                }

                if (_cpe.detraccion.codMoneda != "PEN")
                {
                    _mensajesError.AddMensaje(CodigoError.S3208);
                }

                if (_cpe.detraccion.porcentaje < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "detraccion.porcentaje");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.detraccion.porcentaje, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "detraccion.porcentaje");
                }

                if (_cpe.detraccion.importe < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, "detraccion.importe");
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(_cpe.detraccion.importe, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, "detraccion.importe");
                }
            }

            #endregion

            //Aqui si ya muchos erros no continuamos y notificamos al usuario
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #region Obligatorio informar datos del cliente cuando la factura supera los 700 soles

            if (_cpe.serie.StartsWith("F"))
            {
                //El codigo 0401 en tipo de operacion deberia poder emitir facturas con sujetos no domiciliado
                if (!_esExportacion && _cpe.codigoTipoOperacion != "0401" && _cpe.adquirente.tipoDocumentoIdentificacion != "6" && _cpe.adquirente.tipoDocumentoIdentificacion != "1")
                {
                    _mensajesError.AddMensaje(CodigoError.V0025, "adquirente.tipoDocumentoIdentificacion");
                    return false;
                }
            }
            else
            {
                if (ConvertirMonedaNacional(_cpe.codMoneda, _cpe.importeTotal, _tipoCambioReferencial) >= _montoMaximoClienteAnonimoBoleta)
                {
                    if (_cpe.adquirente.tipoDocumentoIdentificacion == "-" || _cpe.adquirente.numeroDocumentoIdentificacion == "-")
                    {
                        _mensajesError.AddMensaje(CodigoError.V0008);
                        return false;
                    }
                }
            }

            #endregion

            #endregion

            #region Detalles del comprobante

            //Un comprobante se considera oneroso cuando cual menos uno de sus items tiene el valor de "codAfectacionIgv"= '10' o '20' o '30' o '40'
            var _codigosOperacionOnerosa = new List<string>() { "10", "20", "30", "40" };
            var _comprobanteOneroso = false;
            var _detalleGratuito = false;
            var _existeItemExportacion = false;
            int _totalItemsExportacion = 0;
            var _esNotaCreditoMotivo13 = false;
            var _esNotaCreditoDebito = false;

            _idRecord = 0;

            #region Validaciones Nota de credito debito

            if (_cpe.tipoDocumento == "07" || _cpe.tipoDocumento == "08")
            {
                _esNotaCreditoDebito = true;

                if (_cpe.motivosNota?.Count != 1)
                {
                    _mensajesError.AddMensaje(CodigoError.S3203);
                }
                else
                {
                    int indexMotivoNota = 0;

                    foreach (var item in _cpe.motivosNota)
                    {
                        #region Validaciones Nota de Credito 

                        if (_cpe.tipoDocumento == "07")
                        {
                            //Validar el motivo de la nota de credito con el catalogo N° 09
                            if (!OnValidarCatalogoSunat("09", item.tipoNota))
                            {
                                _mensajesError.AddMensaje(CodigoError.V0019, $"motivosNota[{indexMotivoNota}].tipoNota");
                            }
                            else
                            {
                                if (item.tipoDocumento != "01" && item.tipoDocumento != "03" && item.tipoDocumento != "12")
                                {
                                    _mensajesError.AddMensaje(CodigoError.V0014, $"motivosNota[{indexMotivoNota}].tipoDocumento");
                                }
                                else
                                {
                                    if (item.tipoDocumento == "03")
                                    {
                                        //No es posible emitir Notas de crédito con motivos 04(descuento global) 05(descuento por ítem) 08(bonificación).
                                        if (item.tipoNota == "04" || item.tipoNota == "05" || item.tipoNota == "08")
                                        {
                                            _mensajesError.AddMensaje(CodigoError.V0017, $"motivosNota[{indexMotivoNota}].tipoNota");
                                        }
                                    }
                                }
                            }

                            if (item.tipoNota == "13")
                            {
                                if (_cpe.importeTotal > 0)
                                {
                                    _mensajesError.AddMensaje(CodigoError.V0021);
                                    return false;
                                }

                                _esNotaCreditoMotivo13 = true;
                            }
                        }

                        #endregion

                        #region Validaciones Nota de Debito

                        if (_cpe.tipoDocumento == "08")
                        {
                            //Validamos que el tipo de nota de debito sea valido
                            if (!OnValidarCatalogoSunat("10", item.tipoNota))
                            {
                                _mensajesError.AddMensaje(CodigoError.V0020, $"motivosNota[{indexMotivoNota}].tipoNota");
                            }
                            else
                            {
                                //Si no se trata de una nota de debito por penalidad entonces debe haber un tipo de documento de referencia valido
                                if (item.tipoNota != "03")
                                {
                                    if (item.tipoDocumento != "01" && item.tipoDocumento != "03" && item.tipoDocumento != "12")
                                    {
                                        _mensajesError.AddMensaje(CodigoError.V0014, $"motivosNota[{indexMotivoNota}].tipoDocumento");
                                    }
                                }
                            }
                        }

                        #endregion

                        //Si o si se debe tener un sustento
                        if (!Validaciones.IsValidTextSunat(item.sustento, 1, 500))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0003, $"motivosNota[{indexMotivoNota}].sustento");
                        }

                        indexMotivoNota++;
                    }
                }

                if (_cpe.tasaDescuentoGlobal > 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "tasaDescuentoGlobal");
                }

                if (_cpe.anticipos?.Count > 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "anticipos");
                }

                if (_cpe.descuentoGlobalAfectaBI != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "descuentoGlobalAfectaBI");
                }

                if (_cpe.descuentoGlobalNoAfectaBI != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "descuentoGlobalNoAfectaBI");
                }

                if (_cpe.recargoFISE != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "recargoFISE");
                }

                if (_cpe.recargoAlConsumo != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "recargoAlConsumo");
                }

                if (_cpe.percepcion != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "percepcion");
                }

                if (_cpe.otrosCargosGlobalNoAfectaBI != null)
                {
                    _mensajesError.AddMensaje(CodigoError.V4008, "otrosCargosGlobalNoAfectaBI");
                }
            }

            //No continuamos si existen errores
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            #endregion

            foreach (var item in _cpe.detalles)
            {
                bool _valorCalculoValidado = true;

                if (_codigosOperacionOnerosa.Contains(item.codAfectacionIGV))
                {
                    _comprobanteOneroso = true;
                    _detalleGratuito = false;
                }
                else
                {
                    _detalleGratuito = true;
                }

                #region Unidad de medida

                if (!OnValidarCatalogoSunat("03", item.unidadMedida))
                {
                    _mensajesError.AddMensaje(CodigoError.V0029, $"detalle[{_idRecord}].unidadMedida");
                }

                #endregion

                #region cantidad

                if (item.cantidad <= 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0013, $"detalle[{_idRecord}].cantidad");
                    _valorCalculoValidado = false;
                }

                #endregion

                #region descripcion

                if (!Validaciones.IsValidTextSunat(item.nombre, 1, 500))
                {
                    _mensajesError.AddMensaje(CodigoError.V0003, $"detalle[{_idRecord}].nombre");
                }

                #endregion

                #region Codigo Afectacion IGV

                //Validamos el codigo de afectacion del IGV
                if (!OnValidarCatalogoSunat("07", item.codAfectacionIGV))
                {
                    _mensajesError.AddMensaje(CodigoError.V0030, $"detalle[{_idRecord}].codAfectacionIGV");
                }
                else
                {
                    if (item.codAfectacionIGV == "40")
                    {
                        _totalItemsExportacion++;
                        _existeItemExportacion = true;
                    }
                    else
                    {
                        if (_esExportacion)
                        {
                            _mensajesError.AddMensaje(CodigoError.S2642, $"detalle[{_idRecord}].codAfectacionIGV");
                        }
                    }
                }

                #endregion

                #region TasaIGV

                if (item.tasaIGV < 0 || item.tasaIGV >= 100)
                {
                    _mensajesError.AddMensaje(CodigoError.V0100, $"detalle[{_idRecord}].tasaIGV");
                }

                #endregion

                #region montoISC

                if (item.montoISC > 0)
                {
                    if (!OnValidarCatalogoSunat("08", item.codSistemaCalculoISC))
                    {
                        _mensajesError.AddMensaje(CodigoError.V0024, $"detalle[{_idRecord}].codSistemaCalculoISC");
                    }
                }

                #endregion

                if (_cpe.tipoDocumento == "07" || _cpe.tipoDocumento == "08")
                {
                    if (item.descuento != null)
                    {
                        _mensajesError.AddMensaje(CodigoError.V4008, $"detalle[{_idRecord}].descuento");
                        continue;
                    }

                    if (item.otrosCargosNoAfectaBI != null)
                    {
                        _mensajesError.AddMensaje(CodigoError.V4008, $"detalle[{_idRecord}].descuento");
                        continue;
                    }

                    if (item.descuentoNoAfectaBI != null)
                    {
                        _mensajesError.AddMensaje(CodigoError.V4008, $"detalle[{_idRecord}].descuento");
                        continue;
                    }
                }

                #region Validacion de longitud decimal

                if (item.cantidad < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].cantidad");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.cantidad, 10))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].cantidad");
                    _valorCalculoValidado = false;
                }

                if (item.valorVentaUnitario < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].valorVentaUnitario");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.valorVentaUnitario, 10))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].valorVentaUnitario");
                    _valorCalculoValidado = false;
                }

                if (item.precioVentaUnitario < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].precioVentaUnitario");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.precioVentaUnitario, 10))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].precioVentaUnitario");
                    _valorCalculoValidado = false;
                }

                if (item.descuento != null)
                {
                    if (_esNotaCreditoDebito)
                    {
                        _mensajesError.AddMensaje(CodigoError.V4008, $"detalle[{_idRecord}].descuento");
                    }
                    else
                    {
                        if (item.descuento.tasa < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuento.tasa");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuento.tasa, 5))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuento.tasa");
                            _valorCalculoValidado = false;
                        }

                        if (item.descuento.montoBase < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuento.montoBase");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuento.montoBase, 2))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuento.montoBase");
                            _valorCalculoValidado = false;
                        }

                        if (item.descuento.importe < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuento.importe");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuento.importe, 2))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuento.importe");
                            _valorCalculoValidado = false;
                        }
                    }
                }

                if (item.descuentoNoAfectaBI != null)
                {
                    if (_esNotaCreditoDebito)
                    {
                        _mensajesError.AddMensaje(CodigoError.V4008, $"detalle[{_idRecord}].descuentoNoAfectaBI");
                    }
                    else
                    {
                        if (item.descuentoNoAfectaBI.tasa < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuentoNoAfectaBI.tasa");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuentoNoAfectaBI.tasa, 5))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuentoNoAfectaBI.tasa");
                            _valorCalculoValidado = false;
                        }

                        if (item.descuentoNoAfectaBI.montoBase < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuentoNoAfectaBI.montoBase");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuentoNoAfectaBI.montoBase, 2))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuentoNoAfectaBI.montoBase");
                            _valorCalculoValidado = false;
                        }

                        if (item.descuentoNoAfectaBI.importe < 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}].descuentoNoAfectaBI.importe");
                            _valorCalculoValidado = false;
                        }
                        else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.descuentoNoAfectaBI.importe, 2))
                        {
                            _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}].descuentoNoAfectaBI.importe");
                            _valorCalculoValidado = false;
                        }
                    }
                }

                if (item.valorVenta < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}] 'valorVenta'");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.valorVenta, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}] 'valorVenta'");
                    _valorCalculoValidado = false;
                }

                if (item.montoIGV < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}] 'montoIGV'");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.montoIGV, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}] 'montoIGV'");
                    _valorCalculoValidado = false;
                }

                if (item.tasaIGV < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}] 'tasaIGV'");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.tasaIGV, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}] 'tasaIGV'");
                    _valorCalculoValidado = false;
                }

                if (item.montoISC < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}] 'montoISC'");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.montoISC, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}] 'montoISC'");
                    _valorCalculoValidado = false;
                }

                if (item.tasaISC < 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V0012, $"detalle[{_idRecord}] 'montoISC'");
                    _valorCalculoValidado = false;
                }
                else if (!Validaciones.IsValidCantidadDecimalesMaximos(item.tasaISC, 2))
                {
                    _mensajesError.AddMensaje(CodigoError.V0011, $"detalle[{_idRecord}] 'montoISC'");
                    _valorCalculoValidado = false;
                }

                #endregion

                if (_valorCalculoValidado && !_esNotaCreditoMotivo13)
                {
                    if (item.precioVentaUnitario == 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"precioVentaUnitario incorrecto en la linea {_idRecord}, debe ser mayor a 0.00, Valor enviado: {item.precioVentaUnitario}");
                        continue;
                    }

                    //Si es un item gravado Codigo de validacion 3111
                    if (item.codAfectacionIGV.StartsWith("1"))
                    {
                        if (item.montoIGV == 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"montoIGV incorrecto en la linea {_idRecord}, debe ser mayor a 0.00 para Gravadas y Gratuitas de gravadas, Valor enviado: {item.montoIGV}");
                            continue;
                        }

                        if (item.tasaIGV == 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"tasaIGV incorrecto en la linea {_idRecord}, debe ser mayor a 0.00 para Gravadas y Gratuitas de gravadas, Valor enviado: {item.tasaIGV}");
                            continue;
                        }
                    }
                    else
                    {
                        if (item.montoIGV > 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"montoIGV incorrecto en la linea {_idRecord}, debe ser 0.00 para Exoneradas, Inafectas, Exportación, Gratuitas de exoneradas o Gratuitas de inafectas, Valor enviado: {item.montoIGV}");
                            continue;
                        }

                        if (item.tasaIGV > 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"tasaIGV incorrecto en la linea {_idRecord}, debe ser 0.00 para Exoneradas, Inafectas, Exportación, Gratuitas de exoneradas o Gratuitas de inafectas, Valor enviado: {item.tasaIGV}");
                            continue;
                        }
                    }

                    if (item.descuento != null)
                    {
                        //Si hay descuento validamos
                        if (item.descuento.tasa > 0 || item.descuento.importe > 0)
                        {
                            if (!_detalleGratuito)
                            {

                                var _montoBaseDescuentoCalculado = item.valorVentaUnitario * item.cantidad;

                                if (!Validaciones.ValidarToleranciaCalculo(item.descuento.montoBase, decimal.Round(_montoBaseDescuentoCalculado, 2), _toleranciaCalculo))
                                {
                                    _mensajesError.AddMensaje(CodigoError.V2000, $"descuento.montoBase incorrecto en la linea {_idRecord}, Valor enviado: {item.descuento.montoBase} Valor calculado: {decimal.Round(_montoBaseDescuentoCalculado, 2)}; Formula: descuento.montoBase = valorVentaUnitario * cantidad");
                                    continue;
                                }
                            }
                            else
                            {
                                var _montoBaseDescuentoCalculado = item.precioVentaUnitario * item.cantidad;

                                if (!Validaciones.ValidarToleranciaCalculo(item.descuento.montoBase, decimal.Round(_montoBaseDescuentoCalculado, 2), _toleranciaCalculo))
                                {
                                    _mensajesError.AddMensaje(CodigoError.V2000, $"descuento.montoBase incorrecto en la linea {_idRecord}, Valor enviado: {item.descuento.montoBase} Valor calculado: {decimal.Round(_montoBaseDescuentoCalculado, 2)}; Formula(Gratuitas): descuento.montoBase = precioVentaUnitario * cantidad");
                                    continue;
                                }
                            }

                            var _descuentoCalculado = item.descuento.montoBase * item.descuento.tasa;

                            if (!Validaciones.ValidarToleranciaCalculo(item.descuento.importe, decimal.Round(_descuentoCalculado, 2), _toleranciaCalculo))
                            {
                                _mensajesError.AddMensaje(CodigoError.V2000, $"descuento.importe incorrecto en la linea {_idRecord}, Valor enviado: {item.descuento.importe} Valor calculado: {decimal.Round(_descuentoCalculado, 2)}; Formula: descuento.importe = descuento.montoBase * descuento.tasa");
                                continue;
                            }
                        }
                    }

                    if (!_detalleGratuito)
                    {
                        //Codigo de validacion 3271
                        var _valorVentaItemCalculado = item.valorVentaUnitario * item.cantidad;

                        if (item.descuento != null)
                        {
                            _valorVentaItemCalculado -= item.descuento.importe;
                        }

                        if (!Validaciones.ValidarToleranciaCalculo(item.valorVenta, decimal.Round(_valorVentaItemCalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"valorVenta incorrecto en la linea {_idRecord}, Valor enviado: {item.valorVenta} Valor calculado: {decimal.Round(_valorVentaItemCalculado, 2)}; Formula: valorVenta = valorVentaUnitario * cantidad - descuento.importe");
                            continue;
                        }

                        if (item.otrosCargosNoAfectaBI != null)
                        {
                            decimal _montoOtrosCargosNoAfectaBICalculado = 0;
                            _montoOtrosCargosNoAfectaBICalculado = item.otrosCargosNoAfectaBI.montoBase * item.otrosCargosNoAfectaBI.tasa;

                            if (!Validaciones.ValidarToleranciaCalculo(item.otrosCargosNoAfectaBI.importe, decimal.Round(_montoOtrosCargosNoAfectaBICalculado, 2), _toleranciaCalculo))
                            {
                                _mensajesError.AddMensaje(CodigoError.V2000, $"otrosCargosNoAfectaBI.importe incorrecto en la linea {_idRecord}, Valor enviado: {item.otrosCargosNoAfectaBI.importe} Valor calculado: {decimal.Round(_montoOtrosCargosNoAfectaBICalculado, 2)}; Formula: otrosCargosNoAfectaBI.importe = otrosCargosNoAfectaBI.montoBase * otrosCargosNoAfectaBI.tasa");
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (item.valorVentaUnitario > 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"valorVentaUnitario incorrecto en la linea {_idRecord}, debe ser 0.00 cuando el codigo de afectacion es Gratuito, Valor enviado: {item.valorVentaUnitario}");
                            continue;
                        }

                        var _valorVentaItemCalculado = item.precioVentaUnitario * item.cantidad;

                        if (item.descuento != null)
                        {
                            _valorVentaItemCalculado -= item.descuento.importe;
                        }

                        if (!Validaciones.ValidarToleranciaCalculo(item.valorVenta, decimal.Round(_valorVentaItemCalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"valorVenta incorrecto en la linea {_idRecord}, Valor enviado: {item.valorVenta} Valor calculado: {decimal.Round(_valorVentaItemCalculado, 2)}; Formula(Gratuitas): valorVenta = precioVentaUnitario * cantidad - descuento.importe");
                            continue;
                        }
                    }

                    var _montoBaseIGVCalculado = item.valorVenta + item.montoISC;

                    if (!Validaciones.ValidarToleranciaCalculo(item.montoBaseIGV, decimal.Round(_montoBaseIGVCalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"montoBaseIGV incorrecto en la linea {_idRecord}, Valor enviado: {item.montoBaseIGV} Valor calculado: {decimal.Round(_montoBaseIGVCalculado, 2)}; Formula: montoBaseIGV = valorVenta + montoISC");
                        continue;
                    }

                    //Validamos el calculo del IGV
                    if (item.codAfectacionIGV.StartsWith("1"))
                    {
                        var _montoIGVCalculado = item.montoBaseIGV * item.tasaIGV / 100;

                        if (!Validaciones.ValidarToleranciaCalculo(item.montoIGV, decimal.Round(_montoIGVCalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"montoIGV incorrecto en la linea {_idRecord}, Valor enviado: {item.montoIGV} Valor calculado: {decimal.Round(_montoIGVCalculado, 2)}; Formula: montoIGV = montoBaseIGV * tasaIGV / 100");
                            continue;
                        }
                    }

                    var _montoICBPERCalculado = (int)item.cantidad * item.tasaUnitariaICBPER;

                    if (!Validaciones.ValidarToleranciaCalculo(item.montoICBPER, decimal.Round(_montoICBPERCalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"montoICBPER incorrecto en la linea {_idRecord}, Valor enviado: {item.montoICBPER} Valor calculado: {decimal.Round(_montoICBPERCalculado, 2)}; Formula: montoICBPER = cantidad * tasaUnitariaICBPER");
                        continue;
                    }

                    var _sumatoriaImpuestosItemCalculado = item.montoISC + item.montoOTH + item.montoICBPER;

                    if (item.codAfectacionIGV == "10")
                    {
                        _sumatoriaImpuestosItemCalculado += item.montoIGV;
                    }

                    if (!Validaciones.ValidarToleranciaCalculo(item.sumatoriaImpuestos, decimal.Round(_sumatoriaImpuestosItemCalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"sumatoriaImpuestos incorrecto en la linea {_idRecord}, Valor enviado: {item.sumatoriaImpuestos} Valor calculado: {decimal.Round(_sumatoriaImpuestosItemCalculado, 2)}; Formula: sumatoriaImpuestos = montoIGV(cuando codAfectacionIGV='10') + montoISC + montoOTH + montoICBPER");
                        continue;
                    }

                    if (!_detalleGratuito)
                    {
                        var _precioVentaItemCalculado = item.valorVenta + item.sumatoriaImpuestos;

                        if (item.otrosCargosNoAfectaBI != null)
                        {
                            _precioVentaItemCalculado += item.otrosCargosNoAfectaBI.importe;
                        }

                        if (item.descuentoNoAfectaBI != null)
                        {
                            _precioVentaItemCalculado -= item.descuentoNoAfectaBI.importe;
                        }

                        //Codigo de validacion 3270
                        var _precioVentaUnitarioCalculado = _precioVentaItemCalculado / item.cantidad;

                        if (!Validaciones.ValidarToleranciaCalculo(item.precioVentaUnitario, decimal.Round(_precioVentaUnitarioCalculado, 10), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"precioVentaUnitario incorrecto en la linea {_idRecord}, Valor enviado: {item.precioVentaUnitario} Valor calculado: {decimal.Round(_precioVentaUnitarioCalculado, 10)}; Formula: precioVentaUnitario = (valorVenta + sumatoriaImpuestos + otrosCargosNoAfectaBI.importe - descuentoNoAfectaBI.importe) / cantidad");
                        }
                    }
                }

                if (_esNotaCreditoMotivo13)
                {
                    if (item.valorVentaUnitario > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'valorVentaUnitario'");
                    }

                    if (item.precioVentaUnitario > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'precioVentaUnitario'");
                    }

                    if (item.valorVenta > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'valorVenta'");
                    }

                    if (item.montoBaseIGV > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoBaseIGV'");
                    }

                    if (item.montoIGV > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoIGV'");
                    }

                    //if (item.tasaIGV > 0)
                    //{
                    //    _mensajes.AddMensaje(CodigoError.A062, $"detalle[{_idRecord}] 'tasaIGV'");
                    //}

                    if (item.montoBaseISC > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoBaseISC'");
                    }

                    if (item.montoISC > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoISC'");
                    }

                    if (item.tasaISC > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'tasaISC'");
                    }

                    if (item.montoICBPER > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoICBPER'");
                    }

                    if (item.tasaUnitariaICBPER > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'tasaUnitariaICBPER'");
                    }

                    if (item.montoBaseOTH > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoBaseOTH'");
                    }

                    if (item.montoOTH > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'montoOTH'");
                    }

                    if (item.tasaOTH > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'tasaOTH'");
                    }

                    if (item.sumatoriaImpuestos > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0022, $"detalle[{_idRecord}] 'sumatoriaImpuestos'");
                    }
                }

                _idRecord++;
            }

            //Existe algun error en los detalles entonces ya no continuamos
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            if (_cpe.tipoDocumento == "01" || _cpe.tipoDocumento == "03")
            {
                if (_existeItemExportacion)
                {
                    if (!_esExportacion)
                    {
                        _mensajesError.AddMensaje(CodigoError.V0026);
                        return false;
                    }
                }
            }

            #endregion

            #region Validacion de Indicadores

            //Evitamos la advertencia 4025 de Sunat
            if (_cpe.indTransferenciaGratuita && _comprobanteOneroso)
            {
                _mensajesError.AddMensaje(CodigoError.V0018);
            }

            //Agregamos la leyenda 1002 si no es un comprobante oneroso
            if (!_comprobanteOneroso)
            {
                _cpe.indTransferenciaGratuita = true;
            }


            #endregion

            //#region Validacion de detraccion

            //if (_CPE.detraccion != null)
            //{
            //    if (_CPE.detraccion.datosAdiconales == null)
            //    {
            //        _mensajes.AddMensaje(new MensajeType("A043");
            //    }
            //    else
            //    {
            //        var _adicionalesDetraccion = dCatalogoSunatDato.Listar("15").Where(r => (r.codigo.StartsWith("30"))).ToList();
            //        var _codigosExistentes = new List<string>();

            //        foreach (var item in _CPE.detraccion.datosAdiconales)
            //        {
            //            //Verificamos que la leyenda exista
            //            if (_adicionalesDetraccion.Where(r => (r.codigo == item.codigo)).ToList().Count == 0)
            //            {
            //                _mensajes.AddMensaje(new MensajeType("A044", item.codigo));
            //            }
            //            else
            //            {
            //                if (string.IsNullOrEmpty(item.valor))
            //                {
            //                    //Prevenimos error 2066 de SUNAT
            //                    _mensajes.AddMensaje(new MensajeType("A045", item.codigo));
            //                }
            //                else
            //                {
            //                    if (_codigosExistentes.Contains(item.codigo))
            //                    {
            //                        //Prevenimos error 2407 de SUNAT
            //                        _mensajes.AddMensaje(new MensajeType("A046", item.codigo));
            //                    }
            //                    else
            //                    {
            //                        _codigosExistentes.Add(item.codigo);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //#endregion

            #region validaciones de totales

            decimal _descuentosGlobalesNOAfectaBI = Convert.ToDecimal(_cpe.descuentoGlobalNoAfectaBI?.importe);
            decimal _descuentosxLineaNOAfectaBI = Convert.ToDecimal(_cpe.detalles.Sum(x => x.descuentoNoAfectaBI?.importe));
            decimal _totalAnticiposGravados = Convert.ToDecimal(_cpe.anticipos?.Sum(x => x.totalOperacionesGravadas));
            decimal _totalAnticiposExonerados = Convert.ToDecimal(_cpe.anticipos?.Sum(x => x.totalOperacionesExoneradas));
            decimal _totalAnticiposInafectos = Convert.ToDecimal(_cpe.anticipos?.Sum(x => x.totalOperacionesInafectas));
            decimal _totalDescuentosNOAfectaBICalculado = _descuentosxLineaNOAfectaBI + _descuentosGlobalesNOAfectaBI;

            decimal _operacionesGravadasxLinea = _cpe.detalles.Where(x => x.codAfectacionIGV == "10").Sum(x => x.valorVenta);
            decimal _descuentoGlobalAfectaBICalculado = decimal.Round(_operacionesGravadasxLinea * _cpe.tasaDescuentoGlobal, 2);
            decimal _totalOperacionesGravadasCalculado = _operacionesGravadasxLinea - _descuentoGlobalAfectaBICalculado - _totalAnticiposGravados;

            decimal _operacionesExoneradasxLinea = _cpe.detalles.Where(x => x.codAfectacionIGV == "20").Sum(x => x.valorVenta);
            decimal _totalOperacionesExoneradasCalculado = _operacionesExoneradasxLinea - _totalAnticiposExonerados;

            decimal _operacionesInafectasxLinea = _cpe.detalles.Where(x => x.codAfectacionIGV == "30").Sum(x => x.valorVenta);
            decimal _totalOperacionesInafectasCalculado = _operacionesInafectasxLinea - _totalAnticiposInafectos;

            decimal _operacionesExportacionxLinea = _cpe.detalles.Where(x => x.codAfectacionIGV == "40").Sum(x => x.valorVenta);
            decimal _totalOperacionesExportacionCalculado = _operacionesExportacionxLinea - _totalAnticiposInafectos;

            decimal _totalOperacionesGratuitasCalculado = _cpe.detalles.Where(x => x.codAfectacionIGV != "10" && x.codAfectacionIGV != "20" && x.codAfectacionIGV != "30" && x.codAfectacionIGV != "40").Sum(x => x.valorVenta);
            decimal _totalIGVGratuitasCalculado = _cpe.detalles.Where(x => x.codAfectacionIGV.StartsWith("1") && x.codAfectacionIGV != "10").Sum(x => x.montoIGV);

            decimal _valorVentaxLinea = _cpe.detalles.Where(x => x.codAfectacionIGV == "10" || x.codAfectacionIGV == "20" || x.codAfectacionIGV == "30" || x.codAfectacionIGV == "40").Sum(x => x.valorVenta);
            decimal _valorVentaCalculado = _valorVentaxLinea - _descuentoGlobalAfectaBICalculado;
            decimal _sumatoriaISCCalculado = _cpe.detalles.Sum(x => x.montoISC);
            decimal _sumatoriaICBPERCalculado = _cpe.detalles.Sum(x => x.montoICBPER);
            decimal _sumatoriaOTHCalculado = _cpe.detalles.Sum(x => x.montoOTH);

            //No implementado
            decimal _anticiposISC = 0;

            //Codigo de validacion 3291
            decimal _sumatoriaIGVCalculado = _totalOperacionesGravadasCalculado * _cpe.tasaIGV / 100;

            //Codigo de validacion 3294
            decimal _sumatoriaImpuestosCalculado = _sumatoriaIGVCalculado + _sumatoriaISCCalculado + _sumatoriaOTHCalculado + _sumatoriaICBPERCalculado;

            decimal _sumatoriaOtrosCargosxLineaNoAfectaBI = Convert.ToDecimal(_cpe.detalles.Sum(x => x.otrosCargosNoAfectaBI?.importe));

            decimal _cargosGlobaleNoAfectaBI = Convert.ToDecimal(_cpe.otrosCargosGlobalNoAfectaBI?.importe) + Convert.ToDecimal(_cpe.recargoAlConsumo?.importe) + Convert.ToDecimal(_cpe.recargoFISE?.importe);

            decimal _sumatoriaOtrosCargosNoAfectaBICalculado = _cargosGlobaleNoAfectaBI + _sumatoriaOtrosCargosxLineaNoAfectaBI;

            //las validaciones de SUNAT para NC/ND no realizan calculo para este valor
            if (_cpe.tipoDocumento == "07" || _cpe.tipoDocumento == "08")
            {
                _sumatoriaOtrosCargosNoAfectaBICalculado = _cpe.sumatoriaOtrosCargosNoAfectaBI;
            }

            //Codigo de validacion 3279
            decimal _precioVentaCalculado = _valorVentaCalculado + _sumatoriaICBPERCalculado + _sumatoriaISCCalculado + _anticiposISC + _sumatoriaOTHCalculado + ((_operacionesGravadasxLinea - _descuentoGlobalAfectaBICalculado) * _cpe.tasaIGV / 100);
            decimal _totalAnticiposCalculado = Convert.ToDecimal(_cpe.anticipos?.Sum(x => x.importeTotal));
            decimal _totalRedondeo = _cpe.totalRedondeo;
            decimal _importeTotalCalculado = _precioVentaCalculado + _sumatoriaOtrosCargosNoAfectaBICalculado - _totalDescuentosNOAfectaBICalculado - _totalAnticiposCalculado + _totalRedondeo;

            //Se usa para validar los totales de operaciones Gravadas,Exoneradas,Inafectas y Exportacion
            bool _validacionOk;

            #region Validar Operaciones Gravadas

            _validacionOk = true;

            if (_cpe.tasaDescuentoGlobal > 0)
            {
                if (_cpe.descuentoGlobalAfectaBI != null)
                {
                    if (!Validaciones.ValidarToleranciaCalculo(_cpe.descuentoGlobalAfectaBI.montoBase, decimal.Round(_operacionesGravadasxLinea, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalAfectaBI.montoBase incorrecto Valor enviado: {_cpe.descuentoGlobalAfectaBI.montoBase} Valor calculado: {decimal.Round(_operacionesGravadasxLinea, 2)}; Formula: descuentoGlobalAfectaBI.montoBase = Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '10'");
                        _validacionOk = false;
                    }
                    else if (!Validaciones.ValidarToleranciaCalculo(_cpe.descuentoGlobalAfectaBI.importe, decimal.Round(_descuentoGlobalAfectaBICalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalAfectaBI.importe incorrecto Valor enviado: {_cpe.descuentoGlobalAfectaBI.importe} Valor calculado: {decimal.Round(_descuentoGlobalAfectaBICalculado, 2)}; Formula: descuentoGlobalAfectaBI.importe = descuentoGlobalAfectaBI.montoBase * tasaDescuentoGlobal");
                        _validacionOk = false;
                    }
                }
                else
                {
                    if (_operacionesGravadasxLinea > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalAfectaBI es obligatorio cuando tasaDescuentoGlobal > 0 y existan detalles con 'codAfectacionIGV' = '10'");
                        _validacionOk = false;
                    }
                }
            }

            if (_validacionOk)
            {
                if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalOperacionesGravadas, decimal.Round(_totalOperacionesGravadasCalculado, 2), _toleranciaCalculo))
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"totalOperacionesGravadas incorrecto Valor enviado: {_cpe.totalOperacionesGravadas} Valor calculado: {decimal.Round(_totalOperacionesGravadasCalculado, 2)}; Formula: totalOperacionesGravadas = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '10') - descuentoGlobalAfectaBI.importe - (Suma de 'totalOperacionesGravadas' de cada anticipo)");
                }
            }

            #endregion

            #region Validar Operaciones Exoneradas

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalOperacionesExoneradas, decimal.Round(_totalOperacionesExoneradasCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"totalOperacionesExoneradas incorrecto Valor enviado: {_cpe.totalOperacionesExoneradas} Valor calculado: {decimal.Round(_totalOperacionesExoneradasCalculado, 2)}; Formula: totalOperacionesExoneradas = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '20') - (Suma de 'totalOperacionesExoneradas' de cada anticipo)");
            }

            #endregion

            #region Validar Operaciones Inafectas

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalOperacionesInafectas, decimal.Round(_totalOperacionesInafectasCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"totalOperacionesInafectas incorrecto Valor enviado: {_cpe.totalOperacionesInafectas} Valor calculado: {decimal.Round(_totalOperacionesInafectasCalculado, 2)}; Formula: totalOperacionesInafectas = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '30') - (Suma de 'totalOperacionesInafectas' de cada anticipo)");
            }

            #endregion

            #region Validar Operaciones Exportacion

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalOperacionesExportacion, decimal.Round(_totalOperacionesExportacionCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"totalOperacionesExportacion incorrecto Valor enviado: {_cpe.totalOperacionesExportacion} Valor calculado: {decimal.Round(_totalOperacionesExportacionCalculado, 2)}; Formula: totalOperacionesExportacion = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '40') - (Suma de 'totalOperacionesInafectas' de cada anticipo)");
            }

            #endregion

            #region Validar Descuentos no afecta a la base imponible

            if (_cpe.tasaDescuentoGlobal > 0)
            {
                decimal _montoBaseDescuentoGlobalNoAfectaBICalculado = _operacionesExportacionxLinea + _operacionesInafectasxLinea + _operacionesExoneradasxLinea;

                _montoBaseDescuentoGlobalNoAfectaBICalculado += Convert.ToDecimal(_cpe.detalles.Sum(x => x.otrosCargosNoAfectaBI?.importe));

                if (_cpe.descuentoGlobalNoAfectaBI != null)
                {
                    var _descuentoGlobalNoAfectaBICalculado = _montoBaseDescuentoGlobalNoAfectaBICalculado * _cpe.tasaDescuentoGlobal;

                    if (!Validaciones.ValidarToleranciaCalculo(_cpe.descuentoGlobalNoAfectaBI.montoBase, decimal.Round(_montoBaseDescuentoGlobalNoAfectaBICalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalNoAfectaBI.montoBase incorrecto Valor enviado: {_cpe.descuentoGlobalNoAfectaBI.montoBase} Valor calculado: {decimal.Round(_montoBaseDescuentoGlobalNoAfectaBICalculado, 2)}; Formula: descuentoGlobalNoAfectaBI.montoBase = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '20','30'´ ó '40') + (Suma del 'montoOtrosCargosNoAfectaBI' de cada detalle)");
                    }
                    else
                    {
                        if (!Validaciones.ValidarToleranciaCalculo(_cpe.descuentoGlobalNoAfectaBI.importe, decimal.Round(_descuentoGlobalNoAfectaBICalculado, 2), _toleranciaCalculo))
                        {
                            _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalNoAfectaBI.importe incorrecto Valor enviado: {_cpe.descuentoGlobalNoAfectaBI.importe} Valor calculado: {decimal.Round(_descuentoGlobalNoAfectaBICalculado, 2)}; Formula: descuentoGlobalNoAfectaBI.importe = descuentoGlobalNoAfectaBI.montoBase * tasaDescuentoGlobal");
                        }
                    }
                }
                else
                {
                    if (_montoBaseDescuentoGlobalNoAfectaBICalculado > 0)
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"descuentoGlobalNoAfectaBI es obligatorio cuando tasaDescuentoGlobal > 0 y existan detalles con 'codAfectacionIGV' = '20','30'´ ó '40' y/o detalles con otrosCargosNoAfectaBI");
                        _validacionOk = false;
                    }
                }
            }

            #endregion

            #region Validar Operaciones Gratuitas

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalOperacionesGratuitas, decimal.Round(_totalOperacionesGratuitasCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"totalOperacionesGratuitas incorrecto Valor enviado: {_cpe.totalOperacionesGratuitas} Valor calculado: {decimal.Round(_totalOperacionesGratuitasCalculado, 2)}; Formula: totalOperacionesGratuitas = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' != '10','20','30' ó '40')");
            }
            else
            {
                if (!Validaciones.ValidarToleranciaCalculo(_cpe.sumatoriaIGVGratuitas, decimal.Round(_totalIGVGratuitasCalculado, 2), _toleranciaCalculo))
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"sumatoriaIGVGratuitas incorrecto Valor enviado: {_cpe.sumatoriaIGVGratuitas} Valor calculado: {decimal.Round(_totalIGVGratuitasCalculado, 2)}; Formula: sumatoriaIGVGratuitas = (Suma del 'montoIGV' de cada detalle que tenga 'codAfectacionIGV' comience con '1' y no sea '10')");
                }
            }

            #endregion

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.sumatoriaIGV, decimal.Round(_sumatoriaIGVCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"sumatoriaIGV incorrecto Valor enviado: {_cpe.sumatoriaIGV} Valor calculado: {decimal.Round(_sumatoriaIGVCalculado, 2)}; Formula: sumatoriaIGV = totalOperacionesGravadas * tasaIGV / 100");
                return false;
            }

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.sumatoriaImpuestos, decimal.Round(_sumatoriaImpuestosCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"sumatoriaImpuestos incorrecto Valor enviado: {_cpe.sumatoriaImpuestos} Valor calculado: {decimal.Round(_sumatoriaImpuestosCalculado, 2)}; Formula: sumatoriaImpuestos = sumatoriaIGV + sumatoriaISC + sumatoriaOTH + sumatoriaICBPER");
                return false;
            }

            if (_cpe.recargoAlConsumo != null)
            {
                if (_cpe.recargoAlConsumo.importe == 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"si envia informacion en la propiedad recargoAlConsumo, recargoAlConsumo.importe debe ser mayor a 0.00");
                }
                else
                {
                    decimal _montoRecargoAlConsumoCalculado = _cpe.recargoAlConsumo.montoBase * _cpe.recargoAlConsumo.tasa;

                    if (!Validaciones.ValidarToleranciaCalculo(_cpe.recargoAlConsumo.importe, decimal.Round(_montoRecargoAlConsumoCalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"recargoAlConsumo.importe incorrecto Valor enviado: {_cpe.recargoAlConsumo.importe} Valor calculado: {decimal.Round(_montoRecargoAlConsumoCalculado, 2)}; Formula: recargoAlConsumo.importe = recargoAlConsumo.montoBase * recargoAlConsumo.tasa");
                    }
                }
            }

            if (_cpe.recargoFISE != null)
            {
                if (_cpe.recargoFISE.importe == 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"si envia informacion en la propiedad recargoFISE, recargoFISE.importe debe ser mayor a 0.00");
                }
                else
                {
                    decimal _recargoFISECalculado = _cpe.recargoFISE.montoBase * _cpe.recargoFISE.tasa;

                    if (!Validaciones.ValidarToleranciaCalculo(_cpe.recargoFISE.importe, decimal.Round(_recargoFISECalculado, 2), _toleranciaCalculo))
                    {
                        _mensajesError.AddMensaje(CodigoError.V2000, $"recargoFISE.importe incorrecto Valor enviado: {_cpe.recargoFISE.importe} Valor calculado: {decimal.Round(_recargoFISECalculado, 2)}; Formula: recargoFISE.importe = recargoFISE.montoBase * recargoFISE.tasa");
                    }
                }
            }

            if (_cpe.otrosCargosGlobalNoAfectaBI != null)
            {
                if (_cpe.otrosCargosGlobalNoAfectaBI.importe == 0)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"si envia informacion en la propiedad otrosCargosGlobalNoAfectaBI, otrosCargosGlobalNoAfectaBI.importe debe ser mayor a 0.00");
                }
            }

            //Hay error en los montos ya no continuamos
            if (_mensajesError.Count > 0)
            {
                return false;
            }

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.totalDescuentosNoAfectaBI, decimal.Round(_totalDescuentosNOAfectaBICalculado, 2), _toleranciaCalculo))
            {
                if (_esExportacion)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"totalDescuentosNoAfectaBI incorrecto Valor enviado: {_cpe.totalDescuentosNoAfectaBI} Valor calculado: {decimal.Round(_totalDescuentosNOAfectaBICalculado, 2)}; Formula: totalDescuentosNoAfectaBI = (Suma del 'descuentoNoAfectaBI' de cada detalle) + descuentoOperacionesExportacion + descuentoGlobalNoAfectaBI");
                }
                else
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"totalDescuentosNoAfectaBI incorrecto Valor enviado: {_cpe.totalDescuentosNoAfectaBI} Valor calculado: {decimal.Round(_totalDescuentosNOAfectaBICalculado, 2)}; Formula: totalDescuentosNoAfectaBI = (Suma del 'descuentoNoAfectaBI' de cada detalle) + descuentoOperacionesExoneradas + descuentoOperacionesInafectas + descuentoGlobalNoAfectaBI");
                }

                return false;
            }

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.valorVenta, decimal.Round(_valorVentaCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"valorVenta incorrecto Valor enviado: {_cpe.valorVenta} Valor calculado: {decimal.Round(_valorVentaCalculado, 2)}; Formula: valorVenta = (Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '10','20','30' ó '40') - descuentoGlobalAfectaBI.importe");
                return false;
            }

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.precioVenta, decimal.Round(_precioVentaCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"precioVenta incorrecto Valor enviado: {_cpe.precioVenta} Valor calculado: {decimal.Round(_precioVentaCalculado, 2)}; Formula: precioVenta = valorVenta + sumatoriaICBPER + sumatoriaISC + sumatoriaOTH + (((Suma del 'valorVenta' de cada detalle que tenga 'codAfectacionIGV' = '10') - descuentoGlobalAfectaBI.importe) * tasaIGV / 100)");
                return false;
            }

            if (_cpe.tipoDocumento == "01" || _cpe.tipoDocumento == "03")
            {
                if (!Validaciones.ValidarToleranciaCalculo(_cpe.sumatoriaOtrosCargosNoAfectaBI, decimal.Round(_sumatoriaOtrosCargosNoAfectaBICalculado, 2), _toleranciaCalculo))
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"sumatoriaOtrosCargosNoAfectaBI incorrecto Valor enviado: {_cpe.sumatoriaOtrosCargosNoAfectaBI} Valor calculado: {decimal.Round(_sumatoriaOtrosCargosNoAfectaBICalculado, 2)}; Formula: sumatoriaOtrosCargosNoAfectaBI =  (Suma del 'otrosCargosNoAfectaBI.importe' de cada detalle) + montoRecargoAlConsumo + montoFISE + otrosCargosGlobalNoAfectaBI.importe");
                    return false;
                }
            }

            if (!Validaciones.ValidarToleranciaCalculo(_cpe.importeTotal, decimal.Round(_importeTotalCalculado, 2), _toleranciaCalculo))
            {
                _mensajesError.AddMensaje(CodigoError.V2000, $"importeTotal incorrecto Valor enviado: {_cpe.importeTotal} Valor calculado: {decimal.Round(_importeTotalCalculado, 2)}; Formula: importeTotal = precioVenta + sumatoriaOtrosCargosNoAfectaBI - totalDescuentosNoAfectaBI - totalAnticipos + totalRedondeo");
                return false;
            }

            if (_cpe.retencion != null)
            {
                if (_cpe.retencion.montoBase != _cpe.importeTotal)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"retencion.montoBase debe ser igual al importeTotal Valor enviado: {_cpe.retencion.montoBase} Valor importeTotal: {_cpe.importeTotal}");
                    return false;
                }

                if (_cpe.retencion.tasa >= 1)
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"retencion.tasa debe ser un valor decimal, por ejemplo para 3% se debe mandar 0.03");
                    return false;
                }

                var _rentencionCalculado = _cpe.retencion.montoBase * _cpe.retencion.tasa;

                if (!Validaciones.ValidarToleranciaCalculo(_cpe.retencion.importe, decimal.Round(_rentencionCalculado, 2), _toleranciaCalculo))
                {
                    _mensajesError.AddMensaje(CodigoError.V2000, $"retencion.importe incorrecto Valor enviado: {_cpe.retencion.importe} Valor calculado: {decimal.Round(_rentencionCalculado, 2)}; Formula: retencion.importe = retencion.montoBase * retencion.tasa");
                    return false;
                }
            }

            #endregion

            #region Validacion informacion Pago

            if (_cpe.informacionPago != null)
            {
                bool _validarInformacionPago = true;

                //Solo permitir informacionPago para Facturas y Notas de Credito Motivo 13
                if (_cpe.tipoDocumento != "01" && !_esNotaCreditoMotivo13)
                {
                    _mensajesError.AddMensaje(CodigoError.V4018);
                    _validarInformacionPago = false;
                }

                if (_validarInformacionPago)
                {
                    if (_cpe.informacionPago.formaPago == FormaPagoType.Credito)
                    {
                        if (_cpe.informacionPago.cuotas?.Count == 0)
                        {
                            _mensajesError.AddMensaje(CodigoError.S3249);
                        }
                        else
                        {
                            if (_cpe.informacionPago.montoPendientePago > _cpe.importeTotal && !_esNotaCreditoMotivo13)
                            {
                                _mensajesError.AddMensaje(CodigoError.S3265, $"cpe.informacionPago.montoPendientePago: {_cpe.informacionPago.montoPendientePago}, cpe.importeTotal: {_cpe.importeTotal}");
                            }
                            else
                            {
                                var _montoPendientePagoCalculado = _cpe.informacionPago.cuotas.Sum(x => x.monto);

                                if (_cpe.informacionPago.montoPendientePago != _montoPendientePagoCalculado)
                                {
                                    _mensajesError.AddMensaje(CodigoError.S3319, $"cpe.informacionPago.montoPendientePago: {_cpe.informacionPago.montoPendientePago}, Monto pendiente calculado: {_montoPendientePagoCalculado}");
                                }

                                if (_cpe.tipoDocumento == "01")
                                {
                                    //Validamos que la fecha de pago de la cuota sea mayor a la fecha de emision
                                    _idRecord = 0;
                                    foreach (var item in _cpe.informacionPago.cuotas)
                                    {
                                        if (item.fechaPago.Date <= _cpe.fechaEmision.Date)
                                        {
                                            _mensajesError.AddMensaje(CodigoError.S3267, $"cpe.informacionPago.cuotas[{_idRecord}].fechaPago");
                                        }
                                        _idRecord++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //Forma de pago es obligatorio para las facturas y notas de credito Motivo 13
                if (_cpe.tipoDocumento == "01" || _esNotaCreditoMotivo13)
                {
                    _mensajesError.AddMensaje(CodigoError.S3244);
                }
            }

            #endregion

            //Si no existen mensajes de Error entonces la validacion esta OK
            return !(_mensajesError.Count > 0);
        }
    }
}
