// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System.Collections.Generic;
using System.Linq;

namespace GasperSoft.SUNAT.UBL.V2
{
    public class FacturaBoleta : Comun
    {
        private static OrderReferenceType GetOrdenReferencia(CPEType datos)
        {
            if (!string.IsNullOrEmpty(datos.ordenCompra))
            {
                return new OrderReferenceType()
                {
                    ID = new IDType()
                    {
                        Value = datos.ordenCompra
                    }
                };
            }

            return null;
        }

        private static void SetDetraccion(InvoiceType invoice, CPEType datos)
        {
            var _paymentTerms = new List<PaymentTermsType>();

            if (invoice.PaymentTerms != null)
            {
                _paymentTerms.AddRange(invoice.PaymentTerms);
            }

            _paymentTerms.Add(new PaymentTermsType()
            {
                ID = new IDType()
                {
                    Value = "Detraccion"
                },

                PaymentMeansID = new PaymentMeansIDType[]
                        {
                            new PaymentMeansIDType()
                            {
                                schemeName = "Codigo de detraccion",
                                schemeAgencyName= "PE:SUNAT",
                                schemeURI="urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo54",
                                Value = datos.detraccion.codigoBienServicio
                            }
                        },

                //Monto y Porcentaje de la detracción
                PaymentPercent = new PaymentPercentType()
                {
                    Value = datos.detraccion.porcentaje
                },

                Amount = new AmountType2()
                {
                    currencyID = datos.detraccion.codMoneda,
                    Value = datos.detraccion.importe
                }
            });

            invoice.PaymentTerms = _paymentTerms.ToArray();

            var _paymentMeans = new List<PaymentMeansType>();

            if (invoice.PaymentMeans != null)
            {
                _paymentMeans.AddRange(invoice.PaymentMeans);
            }

            _paymentMeans.Add(new PaymentMeansType()
            {
                ID = new IDType()
                {
                    Value = "Detraccion"
                },

                PaymentMeansCode = new PaymentMeansCodeType()
                {
                    listAgencyName = "PE:SUNAT",
                    listName = "Medio de pago",
                    listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo59",
                    Value = datos.detraccion.metodoPago
                },

                PayeeFinancialAccount = new FinancialAccountType()
                {
                    ID = new IDType()
                    {
                        Value = datos.detraccion.numeroCuentaBancoNacion
                    }
                }
            });

            invoice.PaymentMeans = _paymentMeans.ToArray();
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="invoice"></param>
        ///// <param name="datos"></param>
        //private static void SetDatosGuia(InvoiceType invoice, CPEType datos)
        //{
        //    invoice.Delivery = new DeliveryType[]
        //    {
        //        new DeliveryType()
        //        {
        //            Shipment = new ShipmentType()
        //            {
        //                ID = new IDType()
        //                {
        //                    schemeName = "Motivo de Traslado",
        //                    schemeAgencyName = "PE:SUNAT",
        //                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo20",
        //                    Value = datos.datosEnvio.motivoTraslado
        //                },

        //                GrossWeightMeasure = new GrossWeightMeasureType()
        //                {
        //                    Value = datos.datosEnvio.pesoBrutoTotalBienes,
        //                    unitCode = datos.datosEnvio.unidadMedidaPesoBruto,
        //                },

        //                ShipmentStage = new ShipmentStageType[]
        //                {
        //                    new ShipmentStageType()
        //                    {
        //                        TransportModeCode = new TransportModeCodeType()
        //                        {
        //                            listName = "Modalidad de Transporte",
        //                            listAgencyName = "PE:SUNAT",
        //                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo18",
        //                            Value= datos.datosEnvio.modalidadTraslado
        //                        },

        //                        TransitPeriod = new PeriodType()
        //                        {
        //                            StartDate = new StartDateType()
        //                            {
        //                                Value = datos.datosEnvio.fechaInicioTraslado
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    };

        //    //Datos del transportita
        //    if (datos.datosEnvio.transportista != null)
        //    {
        //        invoice.Delivery[0].Shipment.ShipmentStage[0].CarrierParty = new PartyType[]
        //        {
        //            new PartyType()
        //            {
        //                PartyIdentification = new PartyIdentificationType[]
        //                {
        //                    new PartyIdentificationType()
        //                    {
        //                        ID = new IDType()
        //                        {
        //                            //Numero de RUC transportista
        //                            Value = datos.datosEnvio.transportista.numeroDocumentoIdentificacion,

        //                            //Tipo de documento del transportista
        //                            schemeID = datos.datosEnvio.transportista.tipoDocumentoIdentificacion,

        //                            schemeName = "Documento de Identidad",
        //                            schemeAgencyName = "PE:SUNAT",
        //                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
        //                        }
        //                    }
        //                },

        //                PartyName = new PartyNameType[]
        //                {
        //                    new PartyNameType()
        //                    {
        //                        //Apellidos y Nombres o denominacion o razon social del transportista
        //                        Name = new NameType1()
        //                        {
        //                            Value = datos.datosEnvio.transportista.nombre
        //                        }
        //                    }
        //                }
        //            }
        //        };
        //    }

        //    //Placas de la unidad de transporte
        //    if (datos.datosEnvio?.placasVehiculo != null && datos.datosEnvio.placasVehiculo.Count > 0)
        //    {
        //        invoice.Delivery[0].Shipment.ShipmentStage[0].TransportMeans = new TransportMeansType()
        //        {
        //            RoadTransport = new RoadTransportType()
        //            {
        //                LicensePlateID = new LicensePlateIDType()
        //                {
        //                    Value = datos.datosEnvio.placasVehiculo[0]
        //                }
        //            }
        //        };

        //        var _TransportEquipment = new List<TransportEquipmentType>();

        //        foreach (var item in datos.datosEnvio.placasVehiculo)
        //        {
        //            if (item != datos.datosEnvio.placasVehiculo[0])
        //            {
        //                _TransportEquipment.Add(new TransportEquipmentType()
        //                {
        //                    ID = new IDType()
        //                    {
        //                        Value = item
        //                    }
        //                });
        //            }
        //        }

        //        var _TransportHandlingUnitType = new TransportHandlingUnitType()
        //        {
        //            //La primera placa del vehiculo
        //            ID = new IDType()
        //            {
        //                Value = datos.datosEnvio.placasVehiculo[0]
        //            }
        //        };

        //        if (_TransportEquipment.Count > 0)
        //        {
        //            _TransportHandlingUnitType.TransportEquipment = _TransportEquipment.ToArray();
        //        }

        //        invoice.Delivery[0].Shipment.TransportHandlingUnit = new TransportHandlingUnitType[]
        //        {
        //            _TransportHandlingUnitType
        //        };
        //    }

        //    //CONDUCTOR (Transporte Privado)
        //    if (datos.datosEnvio?.conductores != null && datos.datosEnvio.conductores.Count > 0)
        //    {
        //        var _driverPerson = new List<PersonType>();

        //        foreach (var item in datos.datosEnvio.conductores)
        //        {
        //            _driverPerson.Add(new PersonType()
        //            {
        //                ID = new IDType()
        //                {
        //                    //Numero de documento de identidad del conductor
        //                    Value = item.numeroDocumentoIdentificacion,

        //                    //Tipo de documento de identidad del conductor
        //                    schemeID = item.tipoDocumentoIdentificacion,

        //                    schemeName = "Documento de Identidad",
        //                    schemeAgencyName = "PE:SUNAT",
        //                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
        //                }
        //            });
        //        }

        //        invoice.Delivery[0].Shipment.ShipmentStage[0].DriverPerson = _driverPerson.ToArray();
        //    }

        //    //Direccion punto de llegada
        //    if (datos.datosEnvio?.puntoLlegada != null)
        //    {
        //        invoice.Delivery[0].Shipment.Delivery = new DeliveryType()
        //        {
        //            DeliveryAddress = new AddressType()
        //            {
        //                //Ubigeo

        //                ID = new IDType()
        //                {
        //                    Value = datos.datosEnvio.puntoLlegada.ubigeo,
        //                    schemeAgencyName = "PE:INEI",
        //                    schemeName = "Ubigeos"
        //                },

        //                //Direccion completa y detallada
        //                AddressLine = new AddressLineType[]
        //                {
        //                    new AddressLineType()
        //                    {
        //                        Line = new LineType()
        //                        {
        //                            Value = datos.datosEnvio.puntoLlegada.direccion
        //                        }
        //                    }
        //                }
        //            }
        //        };
        //    }

        //    //Direccion del punto de partida 
        //    if (datos.datosEnvio?.puntoPartida != null)
        //    {
        //        invoice.Delivery[0].Shipment.OriginAddress = new AddressType()
        //        {
        //            //Ubigeo
        //            ID = new IDType()
        //            {
        //                Value = datos.datosEnvio.puntoPartida.ubigeo,
        //                schemeAgencyName = "PE:INEI",
        //                schemeName = "Ubigeos"
        //            },

        //            //Direccion completa y detallada
        //            AddressLine = new AddressLineType[]
        //            {
        //                new AddressLineType()
        //                {
        //                    Line = new LineType()
        //                    {
        //                        Value = datos.datosEnvio.puntoPartida.direccion
        //                    }
        //                }
        //            },
        //        };
        //    }
        //}

        private static InvoiceLineType[] GetItems(List<ItemCPEType> items, string codMoneda)
        {
            var _invoiceLines = new List<InvoiceLineType>();
            int _secuenciaId = 1;

            foreach (var item in items)
            {
                var _invoiceLine = new InvoiceLineType()
                {
                    //Número de orden del Ítem (n..3 M)
                    ID = new IDType()
                    {
                        Value = _secuenciaId.ToString()
                    },

                    //Unidad de medida por ítem
                    InvoicedQuantity = new InvoicedQuantityType()
                    {
                        //Catálogo No. 03(an..3 M)
                        unitCode = item.unidadMedida,
                        unitCodeListID = "UN/ECE rec 20",
                        unitCodeListAgencyName = "United Nations Economic Commission for Europe",
                        //Cantidad de unidades por ítem(an..16 M n(12,10))
                        Value = item.cantidad
                    },

                    Item = GetItem(item),

                    //27- Valor unitario por ítem (an..15 M n(12,10))
                    Price = new PriceType()
                    {
                        PriceAmount = new PriceAmountType()
                        {
                            currencyID = codMoneda,
                            Value = item.valorVentaUnitario
                        }
                    },

                    PricingReference = GetPreciosReferenciaItem(item, codMoneda),

                    TaxTotal = GetTotalesItem(item, codMoneda),

                    //Valor de venta por ítem (M)
                    LineExtensionAmount = new LineExtensionAmountType()
                    {
                        currencyID = codMoneda,
                        Value = item.valorVenta
                    },

                    //Cargo/descuento por ítem (C)
                    AllowanceCharge = GetDescuentosCargosItem(item, codMoneda)
                };

                _invoiceLines.Add(_invoiceLine);
                _secuenciaId++;
            }

            return _invoiceLines.ToArray();
        }

        /// <summary>
        /// Extructura tomada de:
        /// http://orientacion.sunat.gob.pe/images/LEGISLACION/RS-340-2017-SUNAT/anexoVIIa-340-2017.pdf
        /// </summary>
        /// <param name="datos">Informacion del comprobante</param>
        /// <param name="emisor">Informacion del emisor</param>
        /// <returns></returns>
        public static InvoiceType GetDocumento(CPEType datos, EmisorType emisor, string signature = null)
        {
            var _invoice = new InvoiceType()
            {
                //Versión del UBL (an3 M)
                UBLVersionID = new UBLVersionIDType()
                {
                    Value = "2.1"
                },

                //Versión de la estructura del documento (an3 M)
                CustomizationID = new CustomizationIDType()
                {
                    Value = "2.0",
                    schemeAgencyName = "PE:SUNAT"
                },

                //Fecha de Emision (YYYY-MM-DD M)
                IssueDate = new IssueDateType() { Value = datos.fechaEmision },

                //Hora de Emision (hh:mm:ss C)
                IssueTime = GetHoraEmision(datos.horaEmision),

                //Tipo de documento - Catálogo No. 01 (an2 M)
                InvoiceTypeCode = new InvoiceTypeCodeType()
                {
                    listAgencyName = "PE:SUNAT",
                    listName = "Tipo de Documento",
                    listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01",
                    listID = datos.codigoTipoOperacion,
                    Value = datos.tipoDocumento
                },

                //Serie y número del comprobante "F###-NNNNNNNN"
                ID = new IDType() { Value = $"{datos.serie}-{datos.numero}" },

                //Tipo de moneda - Catálogo No. 02 (an3 M)
                DocumentCurrencyCode = new DocumentCurrencyCodeType()
                {
                    listID = "ISO 4217 Alpha",
                    listName = "Currency",
                    listAgencyName = "United Nations Economic Commission for Europe",
                    Value = datos.codMoneda
                },

                //Fecha de vencimiento (YYYY-MM-DD C)
                DueDate = GetFechaVencimiento(datos.fechaVencimiento),

                UBLExtensions = new UBLExtensionType[]
                {
                    new UBLExtensionType(){ }
                },

                //Firma Digital
                Signature = GetSignature(emisor, signature),

                //Aqui colocamos la informacion del EMISOR
                AccountingSupplierParty = GetEmisor(emisor, datos.codigoEstablecimiento),

                //Aqui colocamos informacion del CLIENTE
                AccountingCustomerParty = GetAdquiriente(datos.adquirente),

                //Tipo y número de la guía de remisión relacionada (C)
                DespatchDocumentReference = GetGuiasRemisionRelacionadas(datos),

                //Tipo y número de otro documento relacionado (C)
                AdditionalDocumentReference = GetDocumentosReferenciaAdicionales(datos),

                //Cantidad de ítems de la factura
                LineCountNumeric = new LineCountNumericType()
                {
                    Value = datos.detalles.Count()
                },

                //Los detalles del comprobante
                InvoiceLine = GetItems(datos.detalles, datos.codMoneda),

                TaxTotal = GetTotales(datos),

                //Cargos y Descuentos
                AllowanceCharge = GetDescuentosCargos(datos),

                LegalMonetaryTotal = GetLegalMonetaryTotal(datos),

                //Leyenda
                Note = GetNotes(datos),

                //Número de la orden de compra
                OrderReference = GetOrdenReferencia(datos)
            };

            if (datos.informacionPago != null)
            {
                if (datos.informacionPago.formaPago == FormaPagoType.Contado)
                {
                    var _paymentTerms = new List<PaymentTermsType>()
                    {
                        new PaymentTermsType()
                        {
                            ID = new IDType()
                            {
                                Value = "FormaPago"
                            },

                            PaymentMeansID = new PaymentMeansIDType[]
                            {
                                new PaymentMeansIDType()
                                {
                                    Value = "Contado"
                                }
                            }
                        }
                    };

                    //Adicionar la leyenda de "COMPROBANTE DE PERCEPCIÓN"
                    if (datos.percepcion != null)
                    {
                        _paymentTerms.Add(new PaymentTermsType()
                        {
                            ID = new IDType()
                            {
                                Value = "Percepcion"
                            },

                            Amount = new AmountType2()
                            {
                                Value = datos.percepcion.importeTotalEnSolesConPercepcion,
                                currencyID = datos.percepcion.codMoneda
                            }
                        });
                    }

                    _invoice.PaymentTerms = _paymentTerms.ToArray();

                    //metodo de pago
                    if (!string.IsNullOrEmpty(datos.informacionPago.metodoPago))
                    {
                        _invoice.PaymentMeans = new PaymentMeansType[]
                        {
                            new PaymentMeansType()
                            {
                                PaymentMeansCode = new PaymentMeansCodeType()
                                {
                                    listAgencyName = "PE:SUNAT",
                                    listName = "Medio de pago",
                                    listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo59",
                                    Value = datos.informacionPago.metodoPago
                                }
                            }
                        };
                    }
                }
                else //Credito
                {
                    if (datos.informacionPago.cuotas?.Count > 0)
                    {
                        var _paymentTerms = new List<PaymentTermsType>();

                        _paymentTerms.Add(new PaymentTermsType()
                        {
                            ID = new IDType()
                            {
                                Value = "FormaPago"
                            },

                            PaymentMeansID = new PaymentMeansIDType[]
                            {
                                    new PaymentMeansIDType()
                                    {
                                        Value = "Credito"
                                    }
                            },

                            Amount = new AmountType2()
                            {
                                currencyID = datos.codMoneda,
                                Value = datos.informacionPago.montoPendientePago
                            },
                        });

                        int _secuencia = 1;

                        //Ordenamos las cuotas por fecha de pago
                        var _cuotas = datos.informacionPago.cuotas.OrderBy(x => x.fechaPago).ToList();

                        foreach (var cuota in _cuotas)
                        {
                            var _id = $"Cuota{_secuencia.ToString().PadLeft(3, '0')}";

                            _paymentTerms.Add(new PaymentTermsType()
                            {
                                ID = new IDType()
                                {
                                    Value = "FormaPago"
                                },

                                PaymentMeansID = new PaymentMeansIDType[]
                                {
                                        new PaymentMeansIDType()
                                        {
                                            Value = _id
                                        }
                                },

                                Amount = new AmountType2()
                                {
                                    currencyID = datos.codMoneda,
                                    Value = cuota.monto
                                },

                                PaymentDueDate = new PaymentDueDateType()
                                {
                                    Value = cuota.fechaPago
                                }
                            });

                            _secuencia++;
                        }

                        _invoice.PaymentTerms = _paymentTerms.ToArray();
                    }
                }
            }

            if (datos.anticipos?.Count > 0)
            {
                var _anticipos = new List<PaymentType>();

                var _documentsReference = new List<DocumentReferenceType>();

                if (_invoice.AdditionalDocumentReference != null)
                {
                    _documentsReference.AddRange(_invoice.AdditionalDocumentReference);
                }

                int _secuenciaId = 1;

                foreach (var item in datos.anticipos)
                {
                    _anticipos.Add(new PaymentType()
                    {
                        ID = new IDType()
                        {
                            schemeName = "Anticipo",
                            schemeAgencyName = "PE:SUNAT",
                            Value = _secuenciaId.ToString()
                        },

                        PaidAmount = new PaidAmountType()
                        {
                            currencyID = datos.codMoneda,
                            Value = item.importeTotal
                        },

                        PaidDate = new PaidDateType()
                        {
                            Value = item.fechaPago
                        }
                    });

                    var _id = $"{item.serie}-{item.numero}";

                    if (string.IsNullOrEmpty(item.tipoDocumento))
                    {
                        if (datos.tipoDocumento == "01")//Es factura
                        {
                            item.tipoDocumento = "02";
                        }
                        else
                        {
                            if (datos.tipoDocumento == "03")//Es Boleta
                            {
                                item.tipoDocumento = "03";
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(item.tipoDocumentoIdentificacionEmisor) ||
                        string.IsNullOrEmpty(item.numeroDocumentoIdentificacionEmisor))
                    {
                        item.tipoDocumentoIdentificacionEmisor = emisor.tipoDocumentoIdentificacion;
                        item.numeroDocumentoIdentificacionEmisor = emisor.ruc;
                    }

                    //Para el caso de reorganización de empresas
                    if ($"{item.tipoDocumentoIdentificacionEmisor}{item.numeroDocumentoIdentificacionEmisor}" !=
                        $"{emisor.tipoDocumentoIdentificacion}{emisor.ruc}")
                    {
                        _id = $"{item.numeroDocumentoIdentificacionEmisor}-{_id}";
                    }

                    _documentsReference.Add(new DocumentReferenceType()
                    {
                        ID = new IDType()
                        {
                            Value = _id
                        },

                        //Tipo de documento - Catálogo No. 12
                        DocumentTypeCode = new DocumentTypeCodeType()
                        {
                            listAgencyName = "PE:SUNAT",
                            listName = "Documento Relacionado",
                            listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo12",
                            Value = item.tipoDocumento
                        },

                        DocumentStatusCode = new DocumentStatusCodeType()
                        {
                            listName = "Anticipo",
                            listAgencyName = "PE:SUNAT",
                            Value = _secuenciaId.ToString()
                        },

                        IssuerParty = new PartyType()
                        {
                            PartyIdentification = new PartyIdentificationType[]
                            {
                                new PartyIdentificationType()
                                {
                                    ID = new IDType()
                                    {
                                        schemeID = item.tipoDocumentoIdentificacionEmisor,
                                        schemeName = "Documento de Identidad",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeDataURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06",
                                        Value = item.numeroDocumentoIdentificacionEmisor
                                    }
                                }
                            }
                        }
                    });

                    _secuenciaId++;
                }

                _invoice.PrepaidPayment = _anticipos.ToArray();
                _invoice.AdditionalDocumentReference = _documentsReference.ToArray();
            }

            if (datos.detraccion != null)
            {
                //Informacion de detraccion
                SetDetraccion(_invoice, datos);
            }

            ////Guia
            //if (datos.datosEnvio != null)
            //{
            //    SetDatosGuia(_invoice, datos);
            //}

            return _invoice;
        }
    }
}
