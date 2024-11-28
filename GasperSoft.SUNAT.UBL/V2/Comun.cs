// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GasperSoft.SUNAT.UBL.V2
{
    public class Comun
    {
        internal static MonetaryTotalType GetLegalMonetaryTotal(CPEType datos)
        {
            var _legalMonetaryTotal = new MonetaryTotalType();

            //Total Descuentos que no afectan a la base imponible (C)
            if (datos.totalDescuentosNoAfectaBI > 0)
            {
                _legalMonetaryTotal.AllowanceTotalAmount = new AllowanceTotalAmountType()
                {
                    Value = datos.totalDescuentosNoAfectaBI,
                    currencyID = datos.codMoneda
                };
            }

            //Total Otros Cargos (C)
            if (datos.sumatoriaOtrosCargosNoAfectaBI > 0)
            {
                _legalMonetaryTotal.ChargeTotalAmount = new ChargeTotalAmountType()
                {
                    Value = datos.sumatoriaOtrosCargosNoAfectaBI,
                    currencyID = datos.codMoneda
                };
            }

            if (datos.totalAnticipos > 0)
            {
                _legalMonetaryTotal.PrepaidAmount = new PrepaidAmountType()
                {
                    Value = datos.totalAnticipos,
                    currencyID = datos.codMoneda
                };
            }

            //Importe total (M)
            _legalMonetaryTotal.PayableAmount = new PayableAmountType()
            {
                Value = datos.importeTotal,
                currencyID = datos.codMoneda
            };

            //Total Valor de Venta (C)
            _legalMonetaryTotal.LineExtensionAmount = new LineExtensionAmountType()
            {
                Value = datos.valorVenta,
                currencyID = datos.codMoneda
            };

            //Total Precio de Venta (C)
            _legalMonetaryTotal.TaxInclusiveAmount = new TaxInclusiveAmountType()
            {
                Value = datos.precioVenta,
                currencyID = datos.codMoneda
            };

            //Total Redondeo 
            if (datos.totalRedondeo != 0)
            {
                _legalMonetaryTotal.PayableRoundingAmount = new PayableRoundingAmountType()
                {
                    Value = datos.totalRedondeo,
                    currencyID = datos.codMoneda
                };
            }

            return _legalMonetaryTotal;
        }

        internal static IssueTimeType GetHoraEmision(string strHoraEmision)
        {
            if (!string.IsNullOrEmpty(strHoraEmision))
            {
                return new IssueTimeType()
                {
                    Value = strHoraEmision
                };
            }

            return null;
        }

        internal static DueDateType GetFechaVencimiento(DateTime? fechaVencimiento)
        {
            if (fechaVencimiento != null)
            {
                return new DueDateType()
                {
                    Value = Convert.ToDateTime(fechaVencimiento)
                };
            }

            return null;
        }

        internal static SignatureType[] GetSignature(EmisorType emisor, string signature)
        {
            if (string.IsNullOrEmpty(signature))
            {
                signature = "signatureGASPERSOFT";
            }

            var _signature = new List<SignatureType>
            {
                new SignatureType()
                {
                    ID = new IDType()
                    {
                        Value = $"{signature}"
                    },

                    //Note = new NoteType[]
                    //{
                    //    new NoteType()
                    //    {
                    //        Value = "GASPERSOFT"
                    //    }
                    //},

                    SignatoryParty = new PartyType()
                    {
                        PartyIdentification = new PartyIdentificationType[]
                        {
                            new PartyIdentificationType()
                            {
                                ID = new IDType()
                                {
                                    Value = emisor.ruc
                                }
                            }
                        },

                        PartyName = new PartyNameType[]
                        {
                            new PartyNameType()
                            {
                                Name = new NameType1()
                                {
                                    Value = emisor.razonSocial
                                }
                            }
                        }
                    },

                    DigitalSignatureAttachment = new AttachmentType()
                    {
                        ExternalReference = new ExternalReferenceType()
                        {
                            URI = new URIType()
                            {
                                Value = $"#{signature}"
                            }
                        }
                    }
                }
            };

            return _signature.ToArray();
        }

        internal static CustomerPartyType GetAdquiriente(InfoPersonaType adquirente)
        {
            return new CustomerPartyType()
            {
                Party = new PartyType()
                {
                    //Tipo y Número de documento de identidad del adquirente o usuario
                    PartyIdentification = new PartyIdentificationType[]
                    {
                        new PartyIdentificationType()
                        {
                            ID = new IDType()
                            {
                                //Catálogo No. 06
                                schemeID = adquirente.tipoDocumentoIdentificacion,
                                schemeName = "Documento de Identidad",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06",
                                Value = adquirente.numeroDocumentoIdentificacion
                            }
                        }
                    },

                    PartyLegalEntity = new PartyLegalEntityType[]
                    {
                        new PartyLegalEntityType()
                        {
                            //Apellidos y nombres, denominación o razón social del adquirente o usuario (an..100 M)
                            RegistrationName = new RegistrationNameType() { Value = adquirente.nombre }
                        }
                    }
                },
            };
        }

        internal static DocumentReferenceType[] GetGuiasRemisionRelacionadas(CPEType datos)
        {
            if (datos.guiasRemision?.Count > 0)
            {
                var _documentsReference = new List<DocumentReferenceType>();

                foreach (var item in datos.guiasRemision)
                {
                    _documentsReference.Add(new DocumentReferenceType()
                    {
                        //Número de guía (an..30 <Serie>-<Número>)
                        ID = new IDType()
                        {
                            Value = item.numeroDocumento
                        },

                        //Tipo de documento - Catálogo No. 01 ( "09" = GUIA DE REMISIÓN REMITENTE, "31" = GUIA DE REMISIÓN TRANSPORTISTA)
                        DocumentTypeCode = new DocumentTypeCodeType()
                        {
                            listAgencyName = "PE:SUNAT",
                            listName = "Tipo de Documento",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01",
                            Value = item.tipoDocumento
                        }
                    });
                }

                return _documentsReference.ToArray();
            }

            return null;
        }

        internal static DocumentReferenceType[] GetDocumentosReferenciaAdicionales(CPEType datos)
        {
            var _documentsReference = new List<DocumentReferenceType>();

            //Parametros otros documentos relacionados
            if (datos.documentosRelacionados != null && datos.documentosRelacionados.Count > 0)
            {
                foreach (var item in datos.documentosRelacionados)
                {
                    _documentsReference.Add(new DocumentReferenceType()
                    {
                        ID = new IDType()
                        {
                            Value = item.numeroDocumento
                        },

                        //Tipo de documento - Catálogo No. 12
                        DocumentTypeCode = new DocumentTypeCodeType()
                        {
                            listAgencyName = "PE:SUNAT",
                            listName = "SUNAT:Identificador de documento relacionado",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo12",
                            Value = item.tipoDocumento
                        }
                    });
                }
            }

            if (_documentsReference.Count > 0)
            {
                return _documentsReference.ToArray();
            }

            return null;
        }

        internal static NoteType[] GetNotes(CPEType datos)
        {
            var _additionalPropertys = new List<NoteType>();

            if (!string.IsNullOrEmpty(datos.importeTotalLetras))
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "1000",

                    //Descripción de la leyenda
                    Value = datos.importeTotalLetras
                });
            }

            if (datos.indTransferenciaGratuita)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "1002",

                    //Descripción de la leyenda
                    Value = "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE"
                });
            }

            //Adicionar la leyenda de "COMPROBANTE DE PERCEPCIÓN"
            if (datos.percepcion != null)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "2000",

                    //Descripción de la leyenda
                    Value = "COMPROBANTE DE PERCEPCIÓN"
                });
            }

            if (datos.indBienesTransferidosSelva)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "2001",

                    //Descripción de la leyenda
                    Value = "BIENES TRANSFERIDOS EN LA AMAZONÍA REGIÓN SELVA PARA SER CONSUMIDOS EN LA MISMA"
                });
            }

            if (datos.indServiciosSelva)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "2002",

                    //Descripción de la leyenda
                    Value = "SERVICIOS PRESTADOS EN LA AMAZONÍA REGIÓN SELVA PARA SER CONSUMIDOS EN LA MISMA"
                });
            }

            if (datos.indContratosSelva)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "2003",

                    //Descripción de la leyenda
                    Value = "CONTRATOS DE CONSTRUCCIÓN EJECUTADOS EN LA AMAZONÍA REGIÓN SELVA"
                });
            }

            if (datos.detraccion != null)
            {
                _additionalPropertys.Add(new NoteType()
                {
                    //Código de la leyenda - Catálogo No. 52
                    languageLocaleID = "2006",

                    //Descripción de la leyenda
                    Value = "Operación sujeta a detracción"
                });
            }

            return _additionalPropertys.ToArray();
        }

        #region Emisor

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

        internal static SupplierPartyType GetEmisor(EmisorType emisor, string codigoEstablecimiento = "0000")
        {
            return new SupplierPartyType()
            {
                Party = new PartyType()
                {
                    PartyIdentification = new PartyIdentificationType[]
                    {
                        //9- Número de RUC
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

                    //10- Nombre Comercial (an..1500 C)
                    PartyName = GetNombreComercialEmisor(emisor),

                    PartyLegalEntity = new PartyLegalEntityType[]
                    {
                        new PartyLegalEntityType()
                        {
                            //11- Apellidos y nombres, denominación o razón social (an..1500 M)
                            RegistrationName = new RegistrationNameType()
                            {
                                Value = emisor.razonSocial
                            },

                            //12- Domicilio Fiscal 
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
                                },

                                //14- Código asignado por SUNAT para el establecimiento anexo declarado en el RUC (an4 M)
                                AddressTypeCode = new AddressTypeCodeType()
                                {
                                    listAgencyName = "PE:SUNAT",
                                    listName = "Establecimientos anexos",
                                    Value = codigoEstablecimiento
                                }
                            }
                        }
                    }
                }
            };
        }

        #endregion

        #region Items

        internal static ItemIdentificationType GetCodigoProductoItem(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                return new ItemIdentificationType()
                {
                    //22- Código de producto (an..30 C)
                    ID = new IDType()
                    {
                        Value = codigo
                    }
                };
            }

            return null;
        }

        internal static CommodityClassificationType[] GetCodigoProductoSunatItem(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
            {
                return new CommodityClassificationType[]
                {
                   new CommodityClassificationType()
                   {
                       ItemClassificationCode = new ItemClassificationCodeType()
                       {
                           //Catálogo No. 25
                           Value = codigo,
                           listID = "UNSPSC",
                           listAgencyName = "GS1 US",
                           listName = "Item Classification"
                       }
                   }
                };
            }

            return null;
        }

        internal static ItemIdentificationType GetCodigoProductoGS1Item(string tipo, string codigo)
        {
            if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(codigo))
            {
                return new ItemIdentificationType()
                {
                    ID = new IDType()
                    {
                        Value = codigo,
                        schemeID = tipo
                    }
                };
            }

            return null;
        }

        internal static ItemPropertyType[] GetPropiedadesAdicionalesItem(ItemCPEType item)
        {
            var _additionalItemProperty = new List<ItemPropertyType>();

            //25- Número de placa del vehículo (Información Adicional - Gastos art.37° Renta) C
            if (!string.IsNullOrEmpty(item.numeroPlacaVehiculo))
            {
                _additionalItemProperty.Add(new ItemPropertyType()
                {
                    Name = new NameType1()
                    {
                        Value = "Gastos Art. 37 Renta: Número de Placa"
                    },

                    NameCode = new NameCodeType()
                    {
                        Value = "7000",
                        listName = "Propiedad del item",
                        listAgencyName = "PE:SUNAT",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                    },

                    Value = new ValueType()
                    {
                        Value = item.numeroPlacaVehiculo
                    }
                });
            }

            if (_additionalItemProperty.Count > 0)
            {
                return _additionalItemProperty.ToArray();
            }

            return null;
        }

        internal static ItemType GetItem(ItemCPEType item)
        {
            var _item = new ItemType()
            {
                //22- Código de producto (C)
                SellersItemIdentification = GetCodigoProductoItem(item.codigoProducto),

                //23- Codigo producto de SUNAT (C)
                CommodityClassification = GetCodigoProductoSunatItem(item.codigoProductoSunat),

                //24- Código de producto GS1 (C)
                StandardItemIdentification = GetCodigoProductoGS1Item(item.tipoCodigoProductoGS1, item.codigoProductoGS1),

                AdditionalItemProperty = GetPropiedadesAdicionalesItem(item),

                //26- Descripción detallada del servicio prestado, bien vendido o cedido en uso, indicando las características (an..500 M)
                Description = new DescriptionType[]
                {
                    new DescriptionType()
                    {
                        Value = item.nombre
                    }
                }
            };

            if (item.informacionHospedaje != null)
            {
                var _additionalItemProperty = new List<ItemPropertyType>();

                //Código de país de emisión del pasaporte, Codigo "4000" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionHospedaje.codigoPaisEmisionPasaporte))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Código de país de emisión del pasaporte"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4000",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionHospedaje.codigoPaisEmisionPasaporte
                        }
                    });
                }

                //Código de país de residencia del sujeto no domiciliado, Codigo "4001" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionHospedaje.codigoPaisResidenciaSujetoNoDomiciliado))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Código de país de residencia del sujeto no domiciliado"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4001",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionHospedaje.codigoPaisResidenciaSujetoNoDomiciliado
                        }
                    });
                }

                //Fecha de ingreso al país, Codigo "4002" del catalogo 55
                if (item.informacionHospedaje.fechaIngresoPais != null)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Fecha de ingreso al país"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4002",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartDate = new StartDateType()
                            {
                                Value = Convert.ToDateTime(item.informacionHospedaje.fechaIngresoPais)
                            }
                        }
                    });
                }

                //Fecha de Ingreso al Establecimiento, Codigo "4003" del catalogo 55
                if (item.informacionHospedaje.fechaIngresoEstablecimiento != null)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Fecha de Ingreso al Establecimiento"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4003",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartDate = new StartDateType()
                            {
                                Value = Convert.ToDateTime(item.informacionHospedaje.fechaIngresoEstablecimiento)
                            }
                        }
                    });
                }

                //Fecha de Salida del Establecimiento, Codigo "4004" del catalogo 55
                if (item.informacionHospedaje.fechaSalidaEstablecimiento != null)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Fecha de Salida del Establecimiento"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4004",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartDate = new StartDateType()
                            {
                                Value = Convert.ToDateTime(item.informacionHospedaje.fechaSalidaEstablecimiento)
                            }
                        }
                    });
                }

                //Número de Días de Permanencia, Codigo "4005" del catalogo 55
                if (item.informacionHospedaje.numeroDiasPermanencia > 0)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Número de Días de Permanencia"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4005",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            DurationMeasure = new DurationMeasureType()
                            {
                                Value = item.informacionHospedaje.numeroDiasPermanencia,
                                unitCode = "DAY"
                            }
                        }
                    });
                }

                //Fecha de Consumo, Codigo "4006" del catalogo 55
                if (item.informacionHospedaje.fechaConsumo != null)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Fecha de Consumo"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4006",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartDate = new StartDateType()
                            {
                                Value = Convert.ToDateTime(item.informacionHospedaje.fechaConsumo)
                            }
                        }
                    });
                }

                //Nombres y apellidos del huesped, Codigo "4007" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionHospedaje.nombreHuesped))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Nombres y apellidos del huesped"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4007",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionHospedaje.nombreHuesped
                        }
                    });
                }

                //Tipo de documento de identidad del huesped, Codigo "4008" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionHospedaje.tipoDocumentoIdentificacionHuesped))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Tipo de documento de identidad del huesped"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4008",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionHospedaje.tipoDocumentoIdentificacionHuesped
                        }
                    });
                }

                //Número de documento de identidad del huesped, Codigo "4009" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionHospedaje.numeroDocumentoIdentificacionHuesped))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Número de documento de identidad del huesped"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "4009",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionHospedaje.numeroDocumentoIdentificacionHuesped
                        }
                    });
                }

                if (_additionalItemProperty.Count > 0)
                {
                    _item.AdditionalItemProperty = _additionalItemProperty.ToArray();
                }
            }

            if (item.informacionPasajeTransporteTerrestre != null)
            {
                var _additionalItemProperty = new List<ItemPropertyType>();

                //Número de asiento, Codigo "3050" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.numeroAsiento))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Número de asiento"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3050",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.numeroAsiento
                        }
                    });
                }

                //Información de manifiesto de pasajeros, Codigo "3051" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.numeroManifiesto))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Información de manifiesto de pasajeros"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3051",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.numeroManifiesto
                        }
                    });
                }

                //Número de documento de identidad del pasajero, Codigo "3052" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.numeroDocumentoIdentificacionPasajero))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Número de documento de identidad del pasajero"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3052",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.numeroDocumentoIdentificacionPasajero
                        }
                    });
                }

                //Tipo de documento de identidad del pasajero, Codigo "3053" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.tipoDocumentoIdentificacionPasajero))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Tipo de documento de identidad del pasajero"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3053",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.tipoDocumentoIdentificacionPasajero
                        }
                    });
                }

                //Nombres y apellidos del pasajero, Codigo "3054" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.nombrePasajero))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Nombres y apellidos del pasajero"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3054",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.nombrePasajero
                        }
                    });
                }

                //Ciudad o lugar de destino - Ubigeo, Codigo "3055" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.ubigeoDestino))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Ciudad o lugar de destino - Ubigeo"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3055",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.ubigeoDestino
                        }
                    });
                }

                //Ciudad o lugar de destino - Dirección detallada, Codigo "3056" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.direccionDestino))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Ciudad o lugar de destino - Dirección detallada"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3056",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.direccionDestino
                        }
                    });
                }

                //Ciudad o lugar de origen - Ubigeo, Codigo "3057" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.ubigeoOrigen))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Ciudad o lugar de origen - Ubigeo"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3057",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.ubigeoOrigen
                        }
                    });
                }

                //Ciudad o lugar de origen - Dirección detallada, Codigo "3058" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.direccionOrigen))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Ciudad o lugar de origen - Dirección detallada"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3058",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        Value = new ValueType()
                        {
                            Value = item.informacionPasajeTransporteTerrestre.direccionOrigen
                        }
                    });
                }

                //Fecha de inicio programado, Codigo "3059" del catalogo 55
                if (item.informacionPasajeTransporteTerrestre.fechaViaje != null)
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Fecha de inicio programado"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3059",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartDate = new StartDateType()
                            {
                                Value = Convert.ToDateTime(item.informacionPasajeTransporteTerrestre.fechaViaje)
                            }
                        }
                    });
                }

                //Hora de inicio programado, Codigo "3060" del catalogo 55
                if (!string.IsNullOrEmpty(item.informacionPasajeTransporteTerrestre.horaViaje))
                {
                    _additionalItemProperty.Add(new ItemPropertyType()
                    {
                        Name = new NameType1()
                        {
                            Value = "Hora de inicio programado"
                        },

                        NameCode = new NameCodeType()
                        {
                            Value = "3060",
                            listName = "Propiedad del item",
                            listAgencyName = "PE:SUNAT",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                        },

                        UsabilityPeriod = new PeriodType()
                        {
                            StartTime = new StartTimeType()
                            {
                                Value = item.informacionPasajeTransporteTerrestre.horaViaje
                            }
                        }
                    });
                }

                if (_additionalItemProperty.Count > 0)
                {
                    _item.AdditionalItemProperty = _additionalItemProperty.ToArray();
                }
            }

            return _item;
        }

        internal static PricingReferenceType GetPreciosReferenciaItem(ItemCPEType item, string codMoneda)
        {
            var _precios = new List<PriceType>();
            string _tipoPrecio;
            if (item.codAfectacionIGV == "10" ||
                item.codAfectacionIGV == "20" ||
                item.codAfectacionIGV == "30" ||
                item.codAfectacionIGV == "40")
            {
                _tipoPrecio = "01";
            }
            else
            {
                _tipoPrecio = "02";
            }

            //Precio de venta unitario por item (an..15 M n(12,10))
            if (item.precioVentaUnitario > 0)
            {
                _precios.Add(new PriceType()
                {
                    PriceAmount = new PriceAmountType()
                    {
                        currencyID = codMoneda,
                        Value = item.precioVentaUnitario
                    },

                    //Código de tipo de precio - Catálogo No. 16 (an2)
                    PriceTypeCode = new PriceTypeCodeType()
                    {
                        listName = "Tipo de Precio",
                        listAgencyName = "PE:SUNAT",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo16",
                        Value = _tipoPrecio
                    }
                });
            }

            return new PricingReferenceType()
            {
                AlternativeConditionPrice = _precios.ToArray()
            };
        }

        internal static TaxTotalType[] GetTotalesItem(ItemCPEType item, string codMoneda)
        {
            var _taxSubTotal = new List<TaxSubtotalType>();
            string _nombreTributo;
            string _codigoInternacionaltributo;
            string _codigoTributo;

            switch (item.codAfectacionIGV)
            {
                case "10"://Gravado
                    _codigoTributo = "1000";
                    _nombreTributo = "IGV";
                    _codigoInternacionaltributo = "VAT";
                    break;
                case "20"://Exonerado
                    _codigoTributo = "9997";
                    _nombreTributo = "EXO";
                    _codigoInternacionaltributo = "VAT";
                    break;
                case "30"://Inafecto
                    _codigoTributo = "9998";
                    _nombreTributo = "INA";
                    _codigoInternacionaltributo = "FRE";
                    break;
                case "40"://Exportación
                    _codigoTributo = "9995";
                    _nombreTributo = "EXP";
                    _codigoInternacionaltributo = "FRE";
                    break;
                default://Gratuito
                    _codigoTributo = "9996";
                    _nombreTributo = "GRA";
                    _codigoInternacionaltributo = "FRE";
                    break;
            }

            _taxSubTotal.Add(new TaxSubtotalType()
            {
                TaxableAmount = new TaxableAmountType()
                {
                    Value = item.montoBaseIGV,
                    currencyID = codMoneda
                },

                TaxAmount = new TaxAmountType()
                {
                    Value = item.montoIGV,
                    currencyID = codMoneda
                },

                //Estos datos son estaticos
                TaxCategory = new TaxCategoryType()
                {
                    Percent = new PercentType1()
                    {
                        Value = item.tasaIGV
                    },

                    //Tipo de Afectación al IGV
                    TaxExemptionReasonCode = new TaxExemptionReasonCodeType()
                    {
                        Value = item.codAfectacionIGV,
                        listAgencyName = "PE:SUNAT",
                        listName = "Afectacion del IGV",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo07"
                    },

                    TaxScheme = new TaxSchemeType()
                    {
                        //Código de tributo - Catálogo No. 05
                        ID = new IDType()
                        {
                            Value = _codigoTributo,
                            schemeName = "Codigo de tributos",
                            schemeAgencyName = "PE:SUNAT",
                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                        },

                        //Nombre de tributo - Catálogo No. 05
                        Name = new NameType1()
                        {
                            Value = _nombreTributo
                        },

                        //Código internacional tributo - Catálogo No. 05
                        TaxTypeCode = new TaxTypeCodeType()
                        {
                            Value = _codigoInternacionaltributo
                        }
                    }
                }
            });

            #region  Aqui agrego el ISC solo si es mayor a 0

            if (item.montoISC > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Monto base
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = item.montoBaseISC,
                        currencyID = codMoneda
                    },

                    //Monto de ISC de la línea
                    TaxAmount = new TaxAmountType()
                    {
                        Value = item.montoISC,
                        currencyID = codMoneda
                    },

                    TaxCategory = new TaxCategoryType()
                    {
                        Percent = new PercentType1()
                        {
                            Value = item.tasaISC
                        },

                        //Tipo de sistema de ISC
                        TierRange = new TierRangeType()
                        {
                            Value = item.codSistemaCalculoISC
                        },

                        //Estos datos son estaticos
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "2000",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "ISC"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "EXC"
                            }
                        }
                    }
                });
            }

            #endregion

            #region  Aqui agrego el ICBPER solo si es mayor a 0

            if (item.montoICBPER > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Monto de ICBPER de la línea
                    TaxAmount = new TaxAmountType()
                    {
                        Value = item.montoICBPER,
                        currencyID = codMoneda
                    },

                    BaseUnitMeasure = new BaseUnitMeasureType()
                    {
                        Value = (int)item.cantidad,
                        unitCode = item.unidadMedida
                    },

                    TaxCategory = new TaxCategoryType()
                    {
                        PerUnitAmount = new PerUnitAmountType()
                        {
                            Value = item.tasaUnitariaICBPER,
                            currencyID = codMoneda
                        },

                        //Estos datos son estaticos
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "7152",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "ICBPER"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "OTH"
                            }
                        }
                    }
                });
            }

            #endregion

            #region Aqui agrego OTH solo si es mayor a 0

            if (item.montoOTH > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = item.montoBaseOTH,
                        currencyID = codMoneda
                    },

                    TaxAmount = new TaxAmountType()
                    {
                        Value = item.montoOTH,
                        currencyID = codMoneda
                    },

                    TaxCategory = new TaxCategoryType()
                    {
                        Percent = new PercentType1()
                        {
                            Value = item.tasaOTH
                        },

                        //Estos datos son estaticos
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9999",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "OTROS"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "OTH"
                            }
                        }
                    }
                });
            }

            #endregion

            return new TaxTotalType[]
            {
                new TaxTotalType()
                {
                    TaxAmount = new TaxAmountType()
                    {
                        Value = item.sumatoriaImpuestos,
                        currencyID = codMoneda
                    },

                    TaxSubtotal = _taxSubTotal.ToArray()
                }
            };
        }

        internal static AllowanceChargeType[] GetDescuentosCargosItem(ItemCPEType item, string codMoneda)
        {
            var _descuentosCargos = new List<AllowanceChargeType>();

            //Descuentos
            if (item.descuento != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    //Indicador de descuento, colocar false (an5 C)
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = false
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        //OTROS DESCUENTOS(Catálogo No. 53)
                        Value = "00",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    //Factor de cargo/descuento
                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = item.descuento.tasa
                    },

                    //Monto del descuento
                    Amount = new AmountType2()
                    {
                        Value = item.descuento.importe,
                        currencyID = codMoneda
                    },

                    //Monto base del descuento
                    BaseAmount = new BaseAmountType()
                    {
                        Value = item.descuento.montoBase,
                        currencyID = codMoneda
                    }
                });
            }

            //Otros Cargos No Afecta BI
            if (item.otrosCargosNoAfectaBI != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = true
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "48",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    //Factor de cargo/descuento
                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = item.otrosCargosNoAfectaBI.tasa
                    },

                    //Monto del descuento
                    Amount = new AmountType2()
                    {
                        Value = item.otrosCargosNoAfectaBI.importe,
                        currencyID = codMoneda
                    },

                    //Monto base del descuento
                    BaseAmount = new BaseAmountType()
                    {
                        Value = item.otrosCargosNoAfectaBI.montoBase,
                        currencyID = codMoneda
                    }
                });
            }

            //Otros Descuentos No Afecta BI
            if (item.descuentoNoAfectaBI != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = false
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "00",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    //Factor de cargo/descuento
                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = item.descuentoNoAfectaBI.tasa
                    },

                    //Monto del descuento
                    Amount = new AmountType2()
                    {
                        Value = item.descuentoNoAfectaBI.importe,
                        currencyID = codMoneda
                    },

                    //Monto base del descuento
                    BaseAmount = new BaseAmountType()
                    {
                        Value = item.descuentoNoAfectaBI.montoBase,
                        currencyID = codMoneda
                    }
                });
            }

            if (_descuentosCargos.Count > 0)
            {
                return _descuentosCargos.ToArray();
            }

            return null;
        }

        #endregion

        internal static TaxTotalType[] GetTotales(CPEType datos)
        {
            var _taxSubTotal = new List<TaxSubtotalType>();
            var _esNotaCreditoMotivo13 = false;


            decimal _totalAnticiposGravados = 0;
            decimal _totalAnticiposExonerados = 0;
            decimal _totalAnticiposInafectos = 0;
            decimal _totalAnticiposExportacion = 0;

            if (datos.anticipos?.Count > 0)
            {
                _totalAnticiposGravados = datos.anticipos.Sum(x => x.totalOperacionesGravadas);
                _totalAnticiposExonerados = datos.anticipos.Sum(x => x.totalOperacionesExoneradas);
                _totalAnticiposInafectos = datos.anticipos.Sum(x => x.totalOperacionesInafectas);
                _totalAnticiposExportacion = datos.anticipos.Sum(x => x.totalOperacionesExportacion);
            }

            //Cuando es una nota de crédito con motivo 13, todos los montos son 0, y es requerido enviar un “TaxSubTotal” como mínimo con monto 0
            //en este caso al ser la variable "_esNotaCreditoMotivo13 = verdadero" se agregará el “TaxSubTotal” de operaciones gravadas
            if (datos.motivosNota?.Count > 0)
            {
                if (datos.motivosNota[0].tipoNota == "13")
                {
                    _esNotaCreditoMotivo13 = true;
                }
            }

            //Total valor de venta operaciones gravadas(IGV o IVAP)/41- Sumatoria IGV o IVAP (M)
            if (datos.totalOperacionesGravadas > 0 || _totalAnticiposGravados > 0 || _esNotaCreditoMotivo13)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Total valor de venta operaciones gravadas
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.totalOperacionesGravadas,
                        currencyID = datos.codMoneda
                    },

                    //Sumatoria de IGV o IVAP, según corresponda (M)
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaIGV,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "1000",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "IGV"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "VAT"
                            }
                        }
                    }
                });
            }

            //Total valor de venta Exportación (M)
            if (datos.totalOperacionesExportacion > 0 || _totalAnticiposExportacion > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Total Valor de Venta
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.totalOperacionesExportacion,
                        currencyID = datos.codMoneda,
                    },

                    //Monto Total del concepto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = 0,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9995",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "EXP"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "FRE"
                            }
                        }
                    }
                });
            }

            //Total valor de venta operaciones inafectas (M)
            if (datos.totalOperacionesInafectas > 0 || _totalAnticiposInafectos > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Total Valor de Venta
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.totalOperacionesInafectas,
                        currencyID = datos.codMoneda,
                    },

                    //Monto Total del concepto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = 0,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9998",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "INA"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "FRE"
                            }
                        }
                    }
                });
            }

            //Total valor de venta operaciones exoneradas (M)
            if (datos.totalOperacionesExoneradas > 0 || _totalAnticiposExonerados > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Total Valor de Venta
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.totalOperacionesExoneradas,
                        currencyID = datos.codMoneda,
                    },

                    //Monto Total del concepto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = 0,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9997",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "EXO"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "VAT"
                            }
                        }
                    }
                });
            }

            //Total valor de venta operaciones gratuitas (C)
            if (datos.totalOperacionesGratuitas > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Total valor de venta operaciones gratuitas
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.totalOperacionesGratuitas,
                        currencyID = datos.codMoneda,
                    },

                    //Monto Total del concepto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaIGVGratuitas,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9996",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "GRA"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "FRE"
                            }
                        }
                    }
                });
            }

            //Sumatoria ISC (C)
            if (datos.sumatoriaISC > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Monto base
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.montoBaseISC,
                        currencyID = datos.codMoneda
                    },

                    //Monto del Impuesto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaISC,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "2000",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "ISC"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "EXC"
                            }
                        }
                    }
                });
            }

            //Sumatoria ISC (C)
            if (datos.sumatoriaICBPER > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Monto del Impuesto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaICBPER,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "7152",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "ICBPER"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "OTH"
                            }
                        }
                    }
                });
            }

            //Sumatoria Otros Tributos
            if (datos.sumatoriaOTH > 0)
            {
                _taxSubTotal.Add(new TaxSubtotalType()
                {
                    //Monto base
                    TaxableAmount = new TaxableAmountType()
                    {
                        Value = datos.montoBaseOTH,
                        currencyID = datos.codMoneda
                    },

                    //Monto del Impuesto
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaOTH,
                        currencyID = datos.codMoneda
                    },

                    //Estos datos son estaticos
                    TaxCategory = new TaxCategoryType()
                    {
                        TaxScheme = new TaxSchemeType()
                        {
                            //Código de tributo - Catálogo No. 05
                            ID = new IDType()
                            {
                                Value = "9999",
                                schemeName = "Codigo de tributos",
                                schemeAgencyName = "PE:SUNAT",
                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo05"
                            },

                            //Nombre de tributo - Catálogo No. 05
                            Name = new NameType1()
                            {
                                Value = "OTROS"
                            },

                            //Código internacional tributo - Catálogo No. 05
                            TaxTypeCode = new TaxTypeCodeType()
                            {
                                Value = "OTH"
                            }
                        }
                    }
                });
            }

            return new TaxTotalType[]
            {
                new TaxTotalType()
                { 
                    //Suma de todos los impuestos
                    TaxAmount = new TaxAmountType()
                    {
                        Value = datos.sumatoriaImpuestos,
                        currencyID = datos.codMoneda
                    },

                    TaxSubtotal = _taxSubTotal.ToArray()
                }
            };
        }

        internal static AllowanceChargeType[] GetDescuentosCargos(CPEType datos)
        {
            var _descuentosCargos = new List<AllowanceChargeType>();

            if (datos.tasaDescuentoGlobal > 0 && datos.descuentoGlobalAfectaBI != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    //Indicador de descuento, colocar false (an5 C)
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = false
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        //Descuentos globales que afectan la base imponible del IGV/IVAP(Catálogo No. 53)
                        Value = "02",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    //Factor de cargo/descuento
                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.tasaDescuentoGlobal
                    },

                    //Monto del descuento
                    Amount = new AmountType2()
                    {
                        Value = datos.descuentoGlobalAfectaBI.importe,
                        currencyID = datos.codMoneda
                    },

                    //Monto base del descuento(los descuentos se aplican sobre el valor venta)
                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.descuentoGlobalAfectaBI.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.tasaDescuentoGlobal > 0 && datos.descuentoGlobalNoAfectaBI != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    //Indicador de descuento, colocar false (an5 C)
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = false
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        //Descuentos globales que no afectan la base imponible del IGV/IVAP(Catálogo No. 53)
                        Value = "03",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    //Factor de cargo/descuento
                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.tasaDescuentoGlobal
                    },

                    //Monto del descuento
                    Amount = new AmountType2()
                    {
                        Value = datos.descuentoGlobalNoAfectaBI.importe,
                        currencyID = datos.codMoneda
                    },

                    //Monto base del descuento(los descuentos se aplican sobre el valor venta)
                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.descuentoGlobalNoAfectaBI.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.percepcion != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = true
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        //Código del motivo del cargo/descuento 51 o 52 o 53
                        Value = datos.percepcion.codigo,
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.percepcion.tasa
                    },

                    Amount = new AmountType2()
                    {
                        Value = datos.percepcion.importe,
                        currencyID = datos.codMoneda
                    },

                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.percepcion.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.retencion != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = false
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "62",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.retencion.tasa
                    },

                    Amount = new AmountType2()
                    {
                        Value = datos.retencion.importe,
                        currencyID = datos.codMoneda
                    },

                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.retencion.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.recargoFISE != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = true
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "45",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.recargoFISE.tasa
                    },

                    Amount = new AmountType2()
                    {
                        Value = datos.recargoFISE.importe,
                        currencyID = datos.codMoneda
                    },

                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.recargoFISE.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.recargoAlConsumo != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = true
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "46",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    MultiplierFactorNumeric = new MultiplierFactorNumericType()
                    {
                        Value = datos.recargoAlConsumo.tasa
                    },

                    Amount = new AmountType2()
                    {
                        Value = datos.recargoAlConsumo.importe,
                        currencyID = datos.codMoneda
                    },

                    BaseAmount = new BaseAmountType()
                    {
                        Value = datos.recargoAlConsumo.montoBase,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.otrosCargosGlobalNoAfectaBI != null)
            {
                _descuentosCargos.Add(new AllowanceChargeType()
                {
                    ChargeIndicator = new ChargeIndicatorType()
                    {
                        Value = true
                    },

                    AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                    {
                        Value = "50",
                        listAgencyName = "PE:SUNAT",
                        listName = "Cargo/descuento",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                    },

                    Amount = new AmountType2()
                    {
                        Value = datos.otrosCargosGlobalNoAfectaBI.importe,
                        currencyID = datos.codMoneda
                    }
                });
            }

            if (datos.anticipos?.Count > 0)
            {
                foreach (var item in datos.anticipos)
                {
                    if (item.totalOperacionesGravadas > 0)
                    {
                        _descuentosCargos.Add(new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType()
                            {
                                Value = false
                            },

                            AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                            {
                                Value = "04",
                                listAgencyName = "PE:SUNAT",
                                listName = "Cargo/descuento",
                                listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                            },

                            Amount = new AmountType2()
                            {
                                Value = item.totalOperacionesGravadas,
                                currencyID = datos.codMoneda
                            }
                        });
                    }

                    if (item.totalOperacionesExoneradas > 0)
                    {
                        _descuentosCargos.Add(new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType()
                            {
                                Value = false
                            },

                            AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                            {
                                Value = "05",
                                listAgencyName = "PE:SUNAT",
                                listName = "Cargo/descuento",
                                listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                            },

                            Amount = new AmountType2()
                            {
                                Value = item.totalOperacionesExoneradas,
                                currencyID = datos.codMoneda
                            }
                        });
                    }

                    if (item.totalOperacionesInafectas > 0)
                    {
                        _descuentosCargos.Add(new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType()
                            {
                                Value = false
                            },

                            AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                            {
                                Value = "06",
                                listAgencyName = "PE:SUNAT",
                                listName = "Cargo/descuento",
                                listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                            },

                            Amount = new AmountType2()
                            {
                                Value = item.totalOperacionesInafectas,
                                currencyID = datos.codMoneda
                            }
                        });
                    }

                    if (item.totalOperacionesExportacion > 0)
                    {
                        _descuentosCargos.Add(new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType()
                            {
                                Value = false
                            },

                            AllowanceChargeReasonCode = new AllowanceChargeReasonCodeType()
                            {
                                Value = "06",
                                listAgencyName = "PE:SUNAT",
                                listName = "Cargo/descuento",
                                listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo53"
                            },

                            Amount = new AmountType2()
                            {
                                Value = item.totalOperacionesExportacion,
                                currencyID = datos.codMoneda
                            }
                        });
                    }
                }
            }

            if (_descuentosCargos.Count > 0)
            {
                return _descuentosCargos.ToArray();
            }

            return null;
        }
    }
}
