// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.Resumen;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V2
{
    public class ResumenDiarioV2
    {
        private static SupplierPartyType GetEmisor(EmisorType emisor)
        {
            return new SupplierPartyType()
            {
                Party = new PartyType()
                {
                    PartyLegalEntity = new PartyLegalEntityType[]
                        {
                            new PartyLegalEntityType()
                            {
                                //Apellidos y nombres, denominación o razón social (an..100 M)
                                RegistrationName = new RegistrationNameType() { Value = emisor.razonSocial }
                            }
                        },

                    PartyName = new PartyNameType[]
                        {
                            new PartyNameType()
                            {
                                //Nombre Comercial (an..100 C)
                                Name = new NameType1() { Value = emisor.nombreComercial }
                            }
                        },

                    //Domicilio fiscal
                    PostalAddress = new AddressType()
                    {
                        //Código de ubigeo - Catálogo No. 13 (an6 C)
                        ID = new IDType() { Value = emisor.codigoUbigeo },

                        //Dirección completa y detallada (an..100 C)
                        StreetName = new StreetNameType() { Value = emisor.direccion },

                        //Urbanización (an..25 C) 
                        CitySubdivisionName = new CitySubdivisionNameType() { Value = emisor.urbanizacion },

                        //Provincia (an..30 C)
                        CityName = new CityNameType() { Value = emisor.provincia },

                        //Departamento (an..30 C)
                        CountrySubentity = new CountrySubentityType() { Value = emisor.departamento },

                        //Distrito (an.. 30 C)
                        District = new DistrictType() { Value = emisor.distrito },

                        Country = new CountryType()
                        {
                            //Código de país - Catálogo No. 04 (an2 C)
                            IdentificationCode = new IdentificationCodeType() { Value = emisor.codigoPais }
                        }
                    }
                },

                //Número de RUC (n11 M)
                CustomerAssignedAccountID = new CustomerAssignedAccountIDType() { Value = emisor.ruc },

                AdditionalAccountID = new AdditionalAccountIDType[]
                    {
                        //Tipo de documento - Catálogo No. 06 (n1 M)
                        new AdditionalAccountIDType() { Value = emisor.tipoDocumentoIdentificacion }
                    },
            };
        }

        private static TaxTotalType[] GetImpuestos(ItemResumenDiarioV2Type item)
        {
            var _codMoneda = item.codMoneda;
            var _TaxTotals = new List<TaxTotalType>
            {
                //Total ISC (M)
                new TaxTotalType()
                {
                    TaxAmount = new TaxAmountType()
                    {
                        currencyID = _codMoneda,
                        Value = item.sumatoriaISC
                    },

                    TaxSubtotal = new TaxSubtotalType[]
                {
                    new TaxSubtotalType()
                    {
                        TaxAmount = new TaxAmountType()
                        {
                            currencyID = _codMoneda,
                            Value = item.sumatoriaISC
                        },

                        TaxCategory =new TaxCategoryType()
                        {
                            TaxScheme = new TaxSchemeType()
                            {
                                //Código de tributo - Catálogo No. 05 (an4)
                                ID = new IDType()
                                {
                                    Value ="2000"
                                },

                                //Nombre de tributo - Catálogo No. 05 (an..6)
                                Name = new NameType1()
                                {
                                    Value = "ISC"
                                },

                                //Código internacional tributo - Catálogo No. 05 (an3)
                                TaxTypeCode = new TaxTypeCodeType()
                                {
                                    Value ="EXC"
                                }
                            }
                        }
                    }
                }
                },

                //Total IGV (M)
                new TaxTotalType()
                {
                    TaxAmount = new TaxAmountType()
                    {
                        currencyID = _codMoneda,
                        Value = item.sumatoriaIGV
                    },

                    TaxSubtotal = new TaxSubtotalType[]
                {
                    new TaxSubtotalType()
                    {
                        TaxAmount = new TaxAmountType()
                        {
                            currencyID = _codMoneda,
                            Value = item.sumatoriaIGV
                        },

                        TaxCategory = new TaxCategoryType() {

                            TaxScheme = new TaxSchemeType()
                            {
                                ID = new IDType()
                                {
                                    Value = "1000"
                                },

                                Name = new NameType1() {
                                    Value = "IGV"
                                },

                                TaxTypeCode = new TaxTypeCodeType()
                                {
                                    Value = "VAT"
                                }
                            }
                        }
                    }
                }
                }
            };

            if (item.sumatoriaICBPER > 0)
            {
                //Total ICBPER
                _TaxTotals.Add(new TaxTotalType()
                {
                    TaxAmount = new TaxAmountType()
                    {
                        currencyID = _codMoneda,
                        Value = item.sumatoriaICBPER
                    },

                    TaxSubtotal = new TaxSubtotalType[]
                    {
                        new TaxSubtotalType()
                        {
                            TaxAmount = new TaxAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.sumatoriaICBPER
                            },

                            TaxCategory =new TaxCategoryType()
                            {
                                TaxScheme = new TaxSchemeType()
                                {
                                    //Código de tributo - Catálogo No. 05
                                    ID = new IDType()
                                    {
                                        Value ="7152"
                                    },

                                    //Nombre de tributo - Catálogo No. 05
                                    Name = new NameType1()
                                    {
                                        Value = "ICBPER"
                                    },

                                    //Código internacional tributo - Catálogo No. 05
                                    TaxTypeCode = new TaxTypeCodeType()
                                    {
                                        Value ="OTH"
                                    }
                                }
                            }
                        }
                    }
                });
            }

            if (item.sumatoriaOTH > 0)
            {
                //Total Otros tributos (C)
                _TaxTotals.Add(new TaxTotalType()
                {
                    TaxAmount = new TaxAmountType()
                    {
                        currencyID = _codMoneda,
                        Value = item.sumatoriaOTH
                    },

                    TaxSubtotal = new TaxSubtotalType[]
                    {
                        new TaxSubtotalType()
                        {
                            TaxAmount = new TaxAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.sumatoriaOTH
                            },

                            TaxCategory =new TaxCategoryType()
                            {
                                TaxScheme = new TaxSchemeType()
                                {
                                    //Código de tributo - Catálogo No. 05
                                    ID = new IDType()
                                    {
                                        Value ="9999"
                                    },

                                    //Nombre de tributo - Catálogo No. 05
                                    Name = new NameType1()
                                    {
                                        Value = "OTROS"
                                    },

                                    //Código internacional tributo - Catálogo No. 05
                                    TaxTypeCode = new TaxTypeCodeType()
                                    {
                                        Value ="OTH"
                                    }
                                }
                            }
                        }
                    }
                });
            }

            return _TaxTotals.ToArray();
        }

        private static SummaryDocumentsLineType[] GetItems(List<ItemResumenDiarioV2Type> items)
        {
            var _summaryDocumentsLines = new List<SummaryDocumentsLineType>();

            foreach (var item in items)
            {
                var _codMoneda = item.codMoneda;

                var _summaryDocumentLine = new SummaryDocumentsLineType()
                {
                    //Número de fila (M)
                    LineID = new LineIDType()
                    {
                        Value = item.secuencia.ToString()
                    },

                    //Numeración, conformada por serie y número correlativo
                    ID = new IDType() { Value = $"{item.serie}-{item.numero}" },

                    //Tipo de documento (M)
                    DocumentTypeCode = new DocumentTypeCodeType()
                    {
                        Value = item.tipoDocumento
                    },

                    //Informacion del Adquirente
                    AccountingCustomerParty = new CustomerPartyType()
                    {
                        //Número de documento de Identidad del adquirente o usuario
                        CustomerAssignedAccountID = new CustomerAssignedAccountIDType()
                        {
                            Value = string.IsNullOrEmpty(item.numeroDocumentoIdentificacionAdquirente) ? "-" : item.numeroDocumentoIdentificacionAdquirente
                        },

                        AdditionalAccountID = new AdditionalAccountIDType[]
                        {
                            new AdditionalAccountIDType()
                            {
                                Value = item.tipoDocumentoIdentificacionAdquirente
                            }
                        }
                    },

                    Status = new StatusType()
                    {
                        ConditionCode = new ConditionCodeType()
                        {
                            Value = item.estadoItem
                        }
                    },

                    TotalAmount = new AmountType2()
                    {
                        currencyID = _codMoneda,
                        Value = item.importeTotal
                    },

                    TaxTotal = GetImpuestos(item)
                };

                #region BillingPayment

                var _billingPayment = new List<PaymentType>();

                if (item.totalOperacionesGravadas > 0)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones gravadas (M)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesGravadas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "01"
                            }
                        }
                    );
                }

                if (item.totalOperacionesExoneradas > 0)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones exoneradas (M)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesExoneradas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "02"
                            }
                        }
                    );
                }

                if (item.totalOperacionesInafectas > 0)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones inafectas (M)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesInafectas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "03"
                            }
                        }
                    );
                }

                if (item.totalOperacionesExportacion > 0)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones inafectas (M)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesInafectas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "04"
                            }
                        }
                    );
                }

                if (item.totalOperacionesGratuitas > 0)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones Gratuitas (C)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesGratuitas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "05"
                            }
                        }
                   );
                }

                //Si se esta anulando el documento y no existen operaciones Gravadas,Exoneradas,Inafectas,Exportacion o gratuitas
                //agrego PaymentType para evitar el error 2255
                if (item.estadoItem == "3"
                    && item.totalOperacionesGravadas == 0m
                    && item.totalOperacionesExoneradas == 0m
                    && item.totalOperacionesInafectas == 0m
                    && item.totalOperacionesExportacion == 0
                    && item.totalOperacionesGratuitas == 0m)
                {
                    _billingPayment.Add(
                        //Total valor de venta - operaciones gravadas (M)
                        new PaymentType()
                        {
                            PaidAmount = new PaidAmountType()
                            {
                                currencyID = _codMoneda,
                                Value = item.totalOperacionesGravadas
                            },

                            InstructionID = new InstructionIDType()
                            {
                                Value = "01"
                            }
                        }
                    );
                }

                _summaryDocumentLine.BillingPayment = _billingPayment.ToArray();

                #endregion

                #region Documentos que modifica

                if (item.tipoDocumento == "07" || item.tipoDocumento == "08")
                {
                    if (!string.IsNullOrEmpty(item.serieModifica) && !string.IsNullOrEmpty(item.tipoDocumentoModifica))
                    {
                        _summaryDocumentLine.BillingReference = new BillingReferenceType()
                        {
                            InvoiceDocumentReference = new DocumentReferenceType()
                            {
                                ID = new IDType() { Value = $"{item.serieModifica}-{item.numeroModifica}" },

                                DocumentTypeCode = new DocumentTypeCodeType()
                                {
                                    Value = item.tipoDocumentoModifica
                                }
                            }
                        };
                    }
                    else
                    {
                        _summaryDocumentLine.BillingReference = new BillingReferenceType()
                        {
                            InvoiceDocumentReference = new DocumentReferenceType()
                            {
                                ID = new IDType() { Value = "-" },

                                DocumentTypeCode = new DocumentTypeCodeType()
                                {
                                    Value = "-"
                                }
                            }
                        };
                    }
                }

                #endregion

                #region Percepcion

                if (!string.IsNullOrEmpty(item.regimenPercepcion))
                {
                    _summaryDocumentLine.SUNATPerceptionSummaryDocumentReference = new SUNATPerceptionSummaryDocumentReference()
                    {
                        //Regimen de percepción
                        SUNATPerceptionSystemCode = new IDType()
                        {
                            Value = item.regimenPercepcion
                        },

                        //Tasa de la percepción
                        SUNATPerceptionPercent = new PercentType1()
                        {
                            Value = item.tasaPercepcion
                        },

                        //Monto de la percepción
                        TotalInvoiceAmount = new TotalInvoiceAmountType()
                        {
                            Value = item.montoPercepcion
                        },

                        //Monto total a cobrar incluida la percepción
                        SUNATTotalCashed = new AmountType2()
                        {
                            Value = item.importeTotal + item.montoPercepcion
                        },

                        //Base imponible percepción
                        TaxableAmount = new TaxableAmountType()
                        {
                            Value = item.importeTotal
                        }
                    };
                }

                #endregion

                #region AllowanceCharge

                if (item.sumatoriaOtrosCargos > 0)
                {
                    _summaryDocumentLine.AllowanceCharge = new AllowanceChargeType[]
                    {
                        //Importe total de sumatoria otros cargos del item (M)
                        new AllowanceChargeType()
                        {
                            ChargeIndicator = new ChargeIndicatorType()
                            {
                                Value = true
                            },

                            Amount = new AmountType2()
                            {
                                currencyID = _codMoneda,
                                Value = item.sumatoriaOtrosCargos
                            }
                        }
                    };
                }

                #endregion

                _summaryDocumentsLines.Add(_summaryDocumentLine);
            }

            return _summaryDocumentsLines.ToArray();
        }

        public static SummaryDocumentsType GetDocumento(ResumenDiarioV2Type datos, EmisorType emisor)
        {
            var _summaryDocuments = new SummaryDocumentsType()
            {
                //Aqui colocamos la informacion del EMISOR
                AccountingSupplierParty = GetEmisor(emisor),

                ReferenceDate = new ReferenceDateType()
                {
                    Value = datos.fechaEmisionDocumentos
                },

                SummaryDocumentsLine = GetItems(datos.detalles),

                UBLExtensions = new UBLExtensionType[]
                {
                    new UBLExtensionType(){ }
                },

                Signature = Comun.GetSignature(emisor),

                //Identificador del resumen RC-<Fecha>-#####
                ID = new IDType()
                {
                    Value = datos.id
                },

                IssueDate = new IssueDateType()
                {
                    Value = datos.fechaGeneracion
                },

                //Versión del UBL  (M - Valor estatico)
                UBLVersionID = new UBLVersionIDType()
                {
                    Value = "2.0"
                },

                //Versión de la estructura del documento (M - Valor estatico)
                CustomizationID = new CustomizationIDType()
                {
                    Value = "1.1"
                }
            };

            return _summaryDocuments;
        }
    }
}
