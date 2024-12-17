// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CRE;
using System;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V1
{
    /// <remarks/>
    public static class Retencion
    {
        internal static PartyNameType[] GetNombreComercialEmisor(EmisorType emisor)
        {
            if (!string.IsNullOrEmpty(emisor.nombreComercial))
            {
                return new PartyNameType[]
                {
                    new PartyNameType()
                    {
                        Name = new NameType1() { Value = emisor.nombreComercial }
                    }
                };
            }

            return null;
        }

        internal static CitySubdivisionNameType GetUrbanizacionEmisor(EmisorType emisor)
        {
            if (!string.IsNullOrEmpty(emisor.urbanizacion))
            {
                return new CitySubdivisionNameType()
                {
                    //Urbanización
                    Value = emisor.urbanizacion
                };
            }

            return null;
        }

        internal static PartyType GetEmisor(EmisorType emisor)
        {
            return new PartyType()
            {
                PartyIdentification = new PartyIdentificationType[]
                {
                    //Número de RUC
                    new PartyIdentificationType()
                    {
                        ID = new IDType()
                        {
                            schemeID = emisor.tipoDocumentoIdentificacion,
                            schemeName = "Documento de Identidad",
                            schemeAgencyName = "PE:SUNAT",
                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06",
                            Value = emisor.ruc
                        },
                    }
                },

                //Nombre Comercial (an..1500 C)
                PartyName = GetNombreComercialEmisor(emisor),

                PartyLegalEntity = new PartyLegalEntityType[]
                {
                    new PartyLegalEntityType()
                    {
                        //Apellidos y nombres, denominación o razón social (an..1500 M)
                        RegistrationName = new RegistrationNameType()
                        {
                            Value = emisor.razonSocial
                        },

                        //Domicilio Fiscal 
                        RegistrationAddress = new AddressType()
                        {
                            AddressLine = new AddressLineType[]
                            {
                                new AddressLineType()
                                {
                                    Line = new LineType()
                                    {
                                        //Dirección completa y detallada
                                        Value = emisor.direccion
                                    }
                                }
                            },

                            CitySubdivisionName = GetUrbanizacionEmisor(emisor),

                            CityName = new CityNameType()
                            {
                                //Provincia
                                Value =  emisor.provincia
                            },

                            //Ubigeo (Catálogo No. 13) 
                            ID = new IDType()
                            {
                                schemeAgencyName = "PE:INEI",
                                schemeName = "Ubigeos",
                                Value = emisor.codigoUbigeo
                            },

                            //Departamento
                            CountrySubentity = new CountrySubentityType()
                            {
                                Value = emisor.departamento
                            },

                            //Distrito
                            District = new DistrictType(){
                                Value = emisor.distrito
                            },

                            //Código de país (Catálogo No. 04)
                            Country = new CountryType()
                            {
                                IdentificationCode = new IdentificationCodeType()
                                {
                                    listID = "ISO 3166-1",
                                    listAgencyName = "United Nations Economic Commission for Europe",
                                    listName = "Country",
                                    Value = emisor.codigoPais
                                }
                            }

                        }
                    }
                }
            };
        }

        internal static NoteType GetObservaciones(string observaciones)
        {
            if (string.IsNullOrEmpty(observaciones))
                return null;

            return new NoteType()
            {
                Value = observaciones
            };
        }

        internal static PartyType GetProveedor(InfoPersonaType proveedor)
        {
            return new PartyType()
            {
                //Tipo y Número de documento de identidad del adquirente o usuario
                PartyIdentification = new PartyIdentificationType[]
                {
                    new PartyIdentificationType()
                    {
                        ID = new IDType()
                        {
                            //Catálogo No. 06
                            schemeID = proveedor.tipoDocumentoIdentificacion,
                            schemeName = "Documento de Identidad",
                            schemeAgencyName = "PE:SUNAT",
                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06",
                            Value = proveedor.numeroDocumentoIdentificacion
                        }
                    }
                },

                PartyLegalEntity = new PartyLegalEntityType[]
                {
                    new PartyLegalEntityType()
                    {
                        //Apellidos y nombres, denominación o razón social del adquirente o usuario (an..100 M)
                        RegistrationName = new RegistrationNameType() { Value = proveedor.nombre }
                    }
                }
            };
        }

        internal static CurrencyCodeContentType GetCurrencyID(string codigoMoneda)
        {
            if (Enum.IsDefined(typeof(CurrencyCodeContentType), codigoMoneda))
            {
                return (CurrencyCodeContentType)Enum.Parse(typeof(CurrencyCodeContentType), codigoMoneda, true);
            }
            return CurrencyCodeContentType.PEN;
        }

        internal static SUNATRetentionDocumentReferenceType[] GetItems(List<ItemCREType> items, string codMonedaRetencion)
        {
            var _documentosReferencia = new List<SUNATRetentionDocumentReferenceType>();

            foreach (var item in items)
            {
                var _documentoReferencia = new SUNATRetentionDocumentReferenceType()
                {
                    //Serie y número del documento relacionado
                    ID = new IDType()
                    {
                        schemeID = item.documentoRelacionadoTipoDocumento,
                        Value = $"{item.documentoRelacionadoSerie}-{item.documentoRelacionadoNumero}"
                    },

                    //Fecha de emisión documento relacionado
                    IssueDate = new IssueDateType()
                    {
                        Value = item.documentoRelacionadoFechaEmision
                    },

                    //Importe total del documento relacionado
                    TotalInvoiceAmount = new TotalInvoiceAmountType()
                    {
                        Value = item.documentoRelacionadoImporteTotal,
                        currencyID = GetCurrencyID(item.documentoRelacionadoCodMoneda)
                    },

                    //Datos del pago
                    Payment = new PaymentType()
                    {
                        //Fecha de pago
                        PaidDate = new PaidDateType()
                        {
                            Value = item.fechaPago
                        },

                        //Número de pago
                        ID = new IDType()
                        {
                            Value = item.numeroPago.ToString()
                        },

                        //Importe del pago sin retención
                        PaidAmount = new PaidAmountType()
                        {
                            Value = item.pagoTotalSinRetencion,
                            currencyID = GetCurrencyID(item.documentoRelacionadoCodMoneda)
                        }
                    },

                    //Datos de la retención
                    SUNATRetentionInformation = new SUNATRetentionInformationType()
                    {
                        //Importe retenido
                        SUNATRetentionAmount = new AmountType1()
                        {
                            Value = item.importeRetenido,
                            currencyID = GetCurrencyID(codMonedaRetencion)
                        },

                        //Fecha de retención
                        SUNATRetentionDate = new SUNATRetentionDateType()
                        {
                            Value = item.importeRetenidoFecha
                        },

                        //Importe total a pagar (neto)
                        SUNATNetTotalPaid = new AmountType1()
                        {
                            Value = item.importePagadoConRetencion,
                            currencyID = GetCurrencyID(codMonedaRetencion)
                        }
                    }
                };

                if (item.tipoCambio != null)
                {
                    _documentoReferencia.SUNATRetentionInformation.ExchangeRate = new ExchangeRateType()
                    {
                        //Moneda de referencia para el Tipo de Cambio
                        SourceCurrencyCode = new SourceCurrencyCodeType()
                        {
                            Value = item.tipoCambio.codMonedaOrigen
                        },

                        //Moneda objetivo para la Tasa de Cambio
                        TargetCurrencyCode = new TargetCurrencyCodeType()
                        {
                            Value = item.tipoCambio.codMonedaDestino
                        },

                        //Factor aplicado a la moneda de origen para calcular la moneda de destino (Tipo de cambio)
                        CalculationRate = new CalculationRateType()
                        {
                            Value = item.tipoCambio.factorConversion
                        },

                        //Fecha de cambio
                        Date = new DateType()
                        {
                            Value = item.tipoCambio.fecha
                        }
                    };
                }

                _documentosReferencia.Add(_documentoReferencia);
            }

            return _documentosReferencia.ToArray();
        }

        /// <summary>
        /// Convierte un objeto CREType a RetentionType
        /// </summary>
        /// <param name="datos">Informacion del comprobante</param>
        /// <param name="emisor">Informacion del emisor</param>
        /// <param name="signature">Una cadena de texto que se usa para "Signature ID", Por defecto se usará la cadena predeterminada "signatureGASPERSOFT"</param>
        /// <returns>RetentionType con la informacion del documento</returns>
        public static RetentionType GetDocumento(CREType datos, EmisorType emisor, string signature = null)
        {
            var _retention = new RetentionType()
            {
                //Versión del UBL (an3 M)
                UBLVersionID = new UBLVersionIDType()
                {
                    Value = "2.0"
                },

                //Versión de la estructura del documento (an3 M)
                CustomizationID = new CustomizationIDType()
                {
                    Value = "1.0",
                    schemeAgencyName = "PE:SUNAT"
                },

                UBLExtensions = (datos.informacionAdicionalEnXml && datos.informacionAdicional?.Count > 0) ? Comun.GetUBLExtensions(datos.informacionAdicional) : Comun.GetUBLExtensions(),

                //Firma Digital
                Signature = Comun.GetSignature(emisor, signature)[0],

                //Serie y número del comprobante "R###-NNNNNNNN"
                ID = new IDType() { Value = $"{datos.serie}-{datos.numero}" },

                //Fecha de Emision (an..10 M)
                IssueDate = new IssueDateType() { Value = datos.fechaEmision },

                //Datos del emisor electrónico
                AgentParty = GetEmisor(emisor),

                //Información del proveedor
                ReceiverParty = GetProveedor(datos.proveedor),

                //Código del régimen de retención
                SUNATRetentionSystemCode = new IDType()
                {
                    Value = datos.codigoRegimenRetencion
                },

                //Tasa de retención
                SUNATRetentionPercent = new PercentType()
                {
                    Value = datos.tasaRetencion
                },

                //Observaciones
                Note = GetObservaciones(datos.Observaciones),

                //Importe total retenido
                TotalInvoiceAmount = new TotalInvoiceAmountType()
                {
                    Value = datos.importeTotalRetenido,
                    currencyID = GetCurrencyID(datos.codMoneda)
                },

                //Importe total Pagado
                SUNATTotalPaid = new AmountType1()
                {
                    Value = datos.importeTotalPagado,
                    currencyID = GetCurrencyID(datos.codMoneda)
                },

                SUNATRetentionDocumentReference = GetItems(datos.detalles, datos.codMoneda)
            };

            if (datos.totalRedondeoImporteTotalPagado != 0)
            {
                _retention.PayableRoundingAmount = new PayableRoundingAmountType()
                {
                    Value = datos.totalRedondeoImporteTotalPagado,
                    currencyID = GetCurrencyID(datos.codMoneda)
                };
            }

            return _retention;
        }
    }
}
