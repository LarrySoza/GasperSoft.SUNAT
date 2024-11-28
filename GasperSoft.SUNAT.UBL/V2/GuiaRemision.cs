// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.GRE;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V2
{
    public class GuiaRemision : Comun
    {
        private static string GetDocumentType(string codigo)
        {
            switch (codigo)
            {
                case "01": return "Factura";
                case "03": return "Boleta de Venta";
                case "04": return "Liquidación de Compra";
                case "09": return "Guía de Remisión Remitente";
                case "12": return "Ticket o cinta emitido por máquina registradora";
                case "31": return "Guía de Remisión Transportista";
                case "48": return "Comprobante de Operaciones – Ley N° 29972";
                case "49": return "Constancia de Depósito -IVAP(Ley 28211)";
                case "50": return "Declaración Aduanera de Mercancías";
                case "52": return "Declaración Simplificada (DS)";
                case "65": return "Autorización de Circulación para transportar MATPEL – Callao";
                case "66": return "Autorización de Circulación para transporte de carga y mercancías en Lima Metropolitana";
                case "67": return "Permiso de Operación Especial para el servicio de transporte de MATPEL - MTC";
                case "68": return "Habilitación Sanitaria de Transporte Terrestre de Productos Pesqueros y Acuícolas";
                case "69": return "Permiso / Autorización de operación de transporte de mercancías";
                case "71": return "Resolución de Adjudicación de bienes – SUNAT";
                case "72": return "Resolución de Comiso de bienes – SUNAT";
                case "73": return "Guía de Transporte Forestal o de Fauna - SERFOR";
                case "74": return "Guía de Tránsito – SUCAMEC";
                case "75": return "Autorización para operar como empresa de Saneamiento Ambiental – MINSA -";
                case "76": return "Autorización para manejo y recojo de residuos sólidos peligrosos y no peligrosos";
                case "77": return "Certificado fitosanitario la movilización de plantas, productos vegetales, y otros artículos reglamentados";
                case "78": return "Registro Único de Usuarios y Transportistas de Alcohol Etílico";
                case "80": return "Constancia de Depósito – Detracción";
                case "81": return "Código de autorización emitida por el SCOP";
                case "82": return "Declaración jurada de mudanza";
                default: return "";
            }
        }

        private static NoteType[] GetObservaciones(List<string> observaciones)
        {
            var _notas = new List<NoteType>();

            foreach (var item in observaciones)
            {
                _notas.Add(new NoteType()
                {
                    Value = item
                });
            }

            return _notas.ToArray();
        }

        private static OrderReferenceType[] GetGuiasRemisionDadasDeBaja(List<DocumentoRelacionadoGREType> guiasRemisionRelacionadas)
        {
            var _orderReference = new List<OrderReferenceType>();

            foreach (var item in guiasRemisionRelacionadas)
            {
                _orderReference.Add(new OrderReferenceType()
                {
                    //Serie y Numero de documento (an... 13 M "T###-NNNNNNNN")
                    ID = new IDType() { Value = item.numeroDocumento },

                    OrderTypeCode = new OrderTypeCodeType()
                    {
                        Value = item.tipoDocumento,
                        name = "Guía de Remisión"
                    }
                });
            }

            return _orderReference.ToArray();
        }

        private static DocumentReferenceType[] GetDocumentosReferenciaAdicionales(List<DocumentoRelacionadoGREType> documentosReferenciaAdicionales)
        {
            var _documentReference = new List<DocumentReferenceType>();

            foreach (var item in documentosReferenciaAdicionales)
            {
                _documentReference.Add(new DocumentReferenceType()
                {
                    ID = new IDType() { Value = item.numeroDocumento },

                    //Catalogo 61
                    DocumentTypeCode = new DocumentTypeCodeType()
                    {
                        Value = item.tipoDocumento,
                        listAgencyName = "PE:SUNAT",
                        listName = "Documento relacionado al transporte",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo61"
                    },

                    DocumentType = new DocumentTypeType()
                    {
                        Value = GetDocumentType(item.tipoDocumento)
                    },

                    IssuerParty = new PartyType()
                    {
                        PartyIdentification = new PartyIdentificationType[]
                        {
                            new PartyIdentificationType()
                            {
                                ID = new IDType()
                                {
                                    Value = item.emisor.numeroDocumentoIdentificacion,
                                    schemeID = item.emisor.tipoDocumentoIdentificacion,
                                    schemeAgencyName = "PE:SUNAT",
                                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                }
                            }
                        }
                    }
                });
            }

            return _documentReference.ToArray();
        }

        private static DespatchLineType[] GetItems(List<ItemGREType> items)
        {
            var _despatchLines = new List<DespatchLineType>();
            int _secuenciaId = 1;

            foreach (var item in items)
            {
                var _despatchLine = new DespatchLineType()
                {
                    ID = new IDType()
                    {
                        Value = _secuenciaId.ToString()
                    },

                    OrderLineReference = new OrderLineReferenceType[]
                    {
                        new OrderLineReferenceType()
                        {
                            LineID = new LineIDType()
                            {
                                Value = _secuenciaId.ToString()
                            }
                        }
                    },

                    DeliveredQuantity = new DeliveredQuantityType()
                    {
                        //DespatchLineType
                        Value = item.cantidad,

                        //DespatchLineType
                        unitCode = item.unidadMedida,

                        unitCodeListID = "UN/ECE rec 20",

                        unitCodeListAgencyName = "United Nations Economic Commission for Europe"
                    },

                    Item = new ItemType()
                    {
                        Description = new DescriptionType[]
                        {
                            new DescriptionType()
                            {
                                //Descripcion detallada del item 
                                Value = item.nombre
                            }
                        },

                        SellersItemIdentification = GetCodigoProductoItem(item.codigoProducto),

                        //Codigo producto de SUNAT (C)
                        CommodityClassification = GetCodigoProductoSunatItem(item.codigoProductoSunat),

                        //Código de producto GS1 (C)
                        StandardItemIdentification = GetCodigoProductoGS1Item(item.tipoCodigoProductoGS1, item.codigoProductoGS1),

                        AdditionalItemProperty = GetAdditionalsItemProperty(item)
                    }
                };

                _despatchLines.Add(_despatchLine);
                _secuenciaId++;
            }

            return _despatchLines.ToArray();
        }

        private static ItemPropertyType GetAdditionalItemProperty(string codigo, string nombre, string valor)
        {
            return new ItemPropertyType()
            {
                Name = new NameType1()
                {
                    Value = nombre
                },
                NameCode = new NameCodeType()
                {
                    Value = codigo,
                    listName = "Propiedad del item",
                    listAgencyName = "PE:SUNAT",
                    listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo55"
                },
                Value = new ValueType()
                {
                    Value = valor
                }
            };
        }

        private static ItemPropertyType[] GetAdditionalsItemProperty(ItemGREType item)
        {
            var _itemsPropertyType = new List<ItemPropertyType>
            {
                //Indicador de bien regulado por SUNAT
                GetAdditionalItemProperty("7022", "Indicador de bien regulado por SUNAT",item.esBienNormalizado? "1":"0")
            };

            if (!string.IsNullOrEmpty(item.partidaArancelaria))
            {
                _itemsPropertyType.Add(GetAdditionalItemProperty("7020", "Partida arancelaria", item.partidaArancelaria));
            }

            if (!string.IsNullOrEmpty(item.numeroDeclaracionAduanera))
            {
                _itemsPropertyType.Add(GetAdditionalItemProperty("7021", "Numero de declaracion aduanera (DAM)", item.numeroDeclaracionAduanera));
            }

            if (!string.IsNullOrEmpty(item.numeroSerieEnDeclaracionAduanera))
            {
                _itemsPropertyType.Add(GetAdditionalItemProperty("7023", "Numero de serie en la DAM o DS", item.numeroSerieEnDeclaracionAduanera));
            }

            return _itemsPropertyType.ToArray();
        }

        private static AddressType GetDireccion(InfoDireccionGREType direccion)
        {
            var _address = new AddressType()
            {
                //Ubigeo
                ID = new IDType()
                {
                    Value = direccion.ubigeo,
                    schemeAgencyName = "PE:INEI",
                    schemeName = "Ubigeos"
                }
            };

            if (!string.IsNullOrEmpty(direccion.direccion))
            {
                _address.AddressLine = new AddressLineType[]
                {
                    new AddressLineType()
                    {
                        Line = new LineType()
                        {
                            Value = direccion.direccion
                        }
                    }
                };
            }

            if (!string.IsNullOrEmpty(direccion.rucAsociado))
            {
                _address.AddressTypeCode = new AddressTypeCodeType()
                {
                    listID = direccion.rucAsociado
                };
            }

            if (!string.IsNullOrEmpty(direccion.codigoEstablecimiento))
            {
                if (_address.AddressTypeCode == null)
                {
                    _address.AddressTypeCode = new AddressTypeCodeType();
                }

                _address.AddressTypeCode.Value = direccion.codigoEstablecimiento;
            }

            return _address;
        }

        private static SpecialInstructionsType GetSpecialInstruction(string value)
        {
            return new SpecialInstructionsType()
            {
                Value = value
            };
        }

        private static SpecialInstructionsType[] GetSpecialInstructions(GREType datos)
        {
            var _specialInstructionsType = new List<SpecialInstructionsType>();

            if (datos.datosEnvio.indicadoresGRERemitente != null)
            {
                //SUNAT_Envio_IndicadorTransbordoProgramado
                if (datos.datosEnvio.indicadoresGRERemitente.indTransbordoProgramado)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTransbordoProgramado"));
                }

                //SUNAT_Envio_IndicadorTrasladoVehiculoM1L
                if (datos.datosEnvio.indicadoresGRERemitente.indTrasladoVehiculoM1L)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTrasladoVehiculoM1L"));
                }

                //SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio
                if (datos.datosEnvio.indicadoresGRERemitente.indRetornoVehiculoEnvaseVacio)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio"));
                }

                //SUNAT_Envio_IndicadorRetornoVehiculoVacio
                if (datos.datosEnvio.indicadoresGRERemitente.indRetornoVehiculoVacio)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorRetornoVehiculoVacio"));
                }

                //SUNAT_Envio_IndicadorTrasladoTotalDAMoDS
                if (datos.datosEnvio.indicadoresGRERemitente.indTrasladoTotalDAMoDS)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTrasladoTotalDAMoDS"));
                }

                //SUNAT_Envio_IndicadorVehiculoConductoresTransp
                if (datos.datosEnvio.indicadoresGRERemitente.indVehiculoConductoresTransp)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorVehiculoConductoresTransp"));
                }
            }

            if (datos.datosEnvio.indicadoresGRETransportista != null)
            {
                //SUNAT_Envio_IndicadorTransbordoProgramado
                if (datos.datosEnvio.indicadoresGRETransportista.indTransbordoProgramado)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTransbordoProgramado"));
                }

                //SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio
                if (datos.datosEnvio.indicadoresGRETransportista.indRetornoVehiculoEnvaseVacio)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorRetornoVehiculoEnvaseVacio"));
                }

                //SUNAT_Envio_IndicadorRetornoVehiculoVacio
                if (datos.datosEnvio.indicadoresGRETransportista.indRetornoVehiculoVacio)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorRetornoVehiculoVacio"));
                }

                //SUNAT_Envio_IndicadorTrasporteSubcontratado
                if (datos.datosEnvio.indicadoresGRETransportista.indTrasporteSubcontratado)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTrasporteSubcontratado"));
                }

                //SUNAT_Envio_IndicadorPagadorFlete_Remitente
                if (datos.datosEnvio.indicadoresGRETransportista.indPagadorFleteRemitente)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorPagadorFlete_Remitente"));
                }

                //SUNAT_Envio_IndicadorPagadorFlete_Subcontratador
                if (datos.datosEnvio.indicadoresGRETransportista.indPagadorFleteSubcontratador)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorPagadorFlete_Subcontratador"));
                }

                //SUNAT_Envio_IndicadorPagadorFlete_Tercero
                if (datos.datosEnvio.indicadoresGRETransportista.indPagadorFleteTercero)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorPagadorFlete_Tercero"));
                }

                //SUNAT_Envio_IndicadorTrasladoTotal
                if (datos.datosEnvio.indicadoresGRETransportista.indTrasladoTotal)
                {
                    _specialInstructionsType.Add(GetSpecialInstruction("SUNAT_Envio_IndicadorTrasladoTotal"));
                }
            }

            if (_specialInstructionsType.Count > 0)
            {
                return _specialInstructionsType.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Extructura tomada de:
        /// https://cpe.sunat.gob.pe/sites/default/files/inline-files/ValidacionesGREv20221020_publicacion.xlsx
        /// </summary>
        /// <param name="datos">Informacion del comprobante</param>
        /// <param name="emisor">Informacion del emisor</param>
        /// <returns></returns>
        public static DespatchAdviceType GetDocumento(GREType datos, EmisorType emisor, string signature = null)
        {
            var _despatchAdvice = new DespatchAdviceType()
            {
                //Versión del UBL  (M - Valor estatico)
                UBLVersionID = new UBLVersionIDType()
                {
                    Value = "2.1"
                },

                //Versión de la estructura del documento (M - Valor estatico)
                CustomizationID = new CustomizationIDType()
                {
                    Value = "2.0",
                    schemeAgencyName = "PE:SUNAT"
                },

                //Numeración, conformada por serie y número correlativo(an... 13 M "T###-NNNNNNNN")
                ID = new IDType() { Value = $"{datos.serie}-{datos.numero}" },

                //Fecha de Emision (an..10 M)
                IssueDate = new IssueDateType() { Value = datos.fechaEmision },

                //Hora de Emision (hh:mm:ss M)
                IssueTime = new IssueTimeType()
                {
                    Value = datos.horaEmision
                },

                //Tipo de documento (an2 M) 
                DespatchAdviceTypeCode = new DespatchAdviceTypeCodeType()
                {
                    Value = datos.tipoGuia,
                    listAgencyName = "PE:SUNAT",
                    listName = "Tipo de Documento",
                    listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01"
                },

                UBLExtensions = new UBLExtensionType[]
                {
                    new UBLExtensionType() { }
                },

                Signature = GetSignature(emisor, signature),

                //Datos del Destinatario
                DeliveryCustomerParty = new CustomerPartyType()
                {
                    Party = new PartyType()
                    {
                        PartyLegalEntity = new PartyLegalEntityType[]
                        {
                            new PartyLegalEntityType()
                            {
                                RegistrationName = new RegistrationNameType()
                                {
                                    Value = datos.destinatario.nombre
                                }
                            }
                        },
                        PartyIdentification = new PartyIdentificationType[]
                        {
                            new PartyIdentificationType()
                            {
                                ID = new IDType()
                                {
                                    Value = datos.destinatario.numeroDocumentoIdentificacion,
                                    schemeID = datos.destinatario.tipoDocumentoIdentificacion,
                                    schemeName = "Documento de Identidad",
                                    schemeAgencyName = "PE:SUNAT",
                                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                }
                            }
                        }
                    }
                },

                //Datos de Envio
                Shipment = new ShipmentType()
                {
                    ID = new IDType()
                    {
                        Value = "SUNAT_Envio"
                    },

                    //Peso bruto total de los bienes (n(12,3) M)
                    GrossWeightMeasure = new GrossWeightMeasureType()
                    {
                        Value = datos.datosEnvio.pesoBrutoTotalBienes,
                        unitCode = datos.datosEnvio.unidadMedidaPesoBruto
                    },

                    //Informacion de traslado
                    ShipmentStage = new ShipmentStageType[]
                    {
                        new ShipmentStageType()
                        {
                            //Fecha de inicio del traslado 
                            TransitPeriod = new PeriodType()
                            {
                                StartDate = new StartDateType()
                                {
                                    Value = datos.datosEnvio.fechaInicioTraslado
                                }
                            }
                        }
                    },

                    //Direccion del punto de partida
                    Delivery = new DeliveryType()
                    {
                        Despatch = new DespatchType()
                        {
                            DespatchAddress = GetDireccion(datos.datosEnvio.puntoPartida)
                        }
                    },

                    SpecialInstructions = GetSpecialInstructions(datos)
                },

                DespatchLine = GetItems(datos.detalles)
            };

            switch (datos.tipoGuia)
            {
                case "09":
                    //Motivo del traslado Catalogo 20(an2 M)
                    _despatchAdvice.Shipment.HandlingCode = new HandlingCodeType()
                    {
                        Value = datos.datosEnvio.motivoTraslado,
                        listAgencyName = "PE:SUNAT",
                        listName = "Motivo de traslado",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo20"
                    };

                    //Descripción de motivo de traslado (an..100 C)
                    _despatchAdvice.Shipment.HandlingInstructions = new HandlingInstructionsType[]
                    {
                        new HandlingInstructionsType()
                        {
                            Value = datos.datosEnvio.descripcionMotivoTraslado
                        }
                    };

                    //Modalidad de Traslado (an2 M) Catalogo 18 SUNAT
                    _despatchAdvice.Shipment.ShipmentStage[0].TransportModeCode = new TransportModeCodeType()
                    {
                        Value = datos.datosEnvio.modalidadTraslado,
                        listName = "Modalidad de traslado",
                        listAgencyName = "PE:SUNAT",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo18"
                    };

                    //Es importacion
                    if (datos.datosEnvio.motivoTraslado == "08" || datos.datosEnvio.motivoTraslado == "09")
                    {
                        //Numero de Bulltos o Pallets (n11 C)
                        _despatchAdvice.Shipment.TotalTransportHandlingUnitQuantity = new TotalTransportHandlingUnitQuantityType()
                        {
                            Value = datos.datosEnvio.totalBultos
                        };
                    }

                    //Datos del Remitente

                    if (datos.remitente != null)
                    {
                        _despatchAdvice.DespatchSupplierParty = new SupplierPartyType()
                        {
                            Party = new PartyType()
                            {
                                PartyLegalEntity = new PartyLegalEntityType[]
                                {
                                    new PartyLegalEntityType()
                                    {
                                        RegistrationName = new RegistrationNameType()
                                        {
                                            Value = datos.remitente.nombre
                                        }
                                    }
                                },
                                PartyIdentification = new PartyIdentificationType[]
                                {
                                    new PartyIdentificationType()
                                    {
                                        ID = new IDType()
                                        {
                                            Value = datos.remitente.numeroDocumentoIdentificacion,
                                            schemeID = datos.remitente.tipoDocumentoIdentificacion,
                                            schemeName = "Documento de Identidad",
                                            schemeAgencyName = "PE:SUNAT",
                                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                        }
                                    }
                                }
                            }
                        };

                        if (datos.remitente.autorizacionesEspeciales?.Count > 0)
                        {
                            var _autorizaciones = new List<PartyLegalEntityType>();

                            foreach (var item in datos.remitente.autorizacionesEspeciales)
                            {
                                _autorizaciones.Add(new PartyLegalEntityType()
                                {
                                    CompanyID = new CompanyIDType()
                                    {
                                        Value = item.numeroAutorizacion,
                                        schemeID = item.codigoEntidadAutorizadora,
                                        schemeName = "Entidad Autorizadora",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogoD37"
                                    }
                                });
                            }

                            _despatchAdvice.Shipment.Delivery.Despatch.DespatchParty = new PartyType()
                            {
                                AgentParty = new PartyType()
                                {
                                    PartyLegalEntity = _autorizaciones.ToArray()
                                }
                            };
                        }
                    }

                    //Transportista (Transporte Público)
                    if (datos.transportista != null)
                    {
                        var _transportita = new PartyType()
                        {
                            PartyIdentification = new PartyIdentificationType[]
                            {
                                new PartyIdentificationType()
                                {
                                    ID = new IDType()
                                    {
                                        //Numero de RUC transportista
                                        Value = datos.transportista.ruc,

                                        //Tipo de documento del transportista
                                        schemeID = "6",

                                        schemeName = "Documento de Identidad",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                    }
                                }
                            },

                            PartyLegalEntity = new PartyLegalEntityType[]
                            {
                                new PartyLegalEntityType()
                                {
                                    RegistrationName = new RegistrationNameType()
                                    {
                                        Value = datos.transportista.razonSocial
                                    }
                                }
                            }
                        };

                        if (!string.IsNullOrEmpty(datos.transportista.registroMTC))
                        {
                            _transportita.PartyLegalEntity[0].CompanyID = new CompanyIDType()
                            {
                                Value = datos.transportista.registroMTC
                            };
                        }

                        if (datos.transportista.autorizacionesEspeciales?.Count > 0)
                        {
                            var _autorizaciones = new List<PartyLegalEntityType>();

                            foreach (var item in datos.transportista.autorizacionesEspeciales)
                            {
                                _autorizaciones.Add(new PartyLegalEntityType()
                                {
                                    CompanyID = new CompanyIDType()
                                    {
                                        Value = item.numeroAutorizacion,
                                        schemeID = item.codigoEntidadAutorizadora,
                                        schemeName = "Entidad Autorizadora",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogoD37"
                                    }
                                });
                            }

                            _transportita.AgentParty = new PartyType()
                            {
                                PartyLegalEntity = _autorizaciones.ToArray()
                            };
                        }

                        _despatchAdvice.Shipment.ShipmentStage[0].CarrierParty = new PartyType[]
                        {
                            _transportita
                        };
                    }

                    if (datos.proveedor != null)
                    {
                        _despatchAdvice.SellerSupplierParty = new SupplierPartyType()
                        {
                            //CustomerAssignedAccountID = new CustomerAssignedAccountIDType()
                            //{
                            //    Value = datos.proveedor.numeroDocumentoIdentificacion,
                            //    schemeID = datos.proveedor.tipoDocumentoIdentificacion
                            //},

                            Party = new PartyType()
                            {
                                PartyLegalEntity = new PartyLegalEntityType[]
                                {
                            new PartyLegalEntityType()
                            {
                                RegistrationName =new RegistrationNameType()
                                {
                                    Value = datos.proveedor.nombre
                                }
                            }
                                },
                                PartyIdentification = new PartyIdentificationType[]
                                {
                            new PartyIdentificationType()
                            {
                                ID = new IDType()
                                {
                                    Value = datos.proveedor.numeroDocumentoIdentificacion,
                                    schemeID = datos.proveedor.tipoDocumentoIdentificacion,
                                    schemeName = "Documento de Identidad",
                                    schemeAgencyName = "PE:SUNAT",
                                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                }
                            }
                                }
                            }
                        };
                    }

                    if (datos.comprador != null)
                    {
                        _despatchAdvice.BuyerCustomerParty = new CustomerPartyType()
                        {
                            Party = new PartyType()
                            {
                                PartyLegalEntity = new PartyLegalEntityType[]
                                {
                            new PartyLegalEntityType()
                            {
                                RegistrationName =new RegistrationNameType()
                                {
                                    Value = datos.comprador.nombre
                                }
                            }
                                },
                                PartyIdentification = new PartyIdentificationType[]
                                {
                            new PartyIdentificationType()
                            {
                                ID = new IDType()
                                {
                                    Value = datos.comprador.numeroDocumentoIdentificacion,
                                    schemeID = datos.comprador.tipoDocumentoIdentificacion,
                                    schemeName = "Documento de Identidad",
                                    schemeAgencyName = "PE:SUNAT",
                                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                }
                            }
                                }
                            }
                        };
                    }

                    //Datos del contenedor (Motivo Importación) 
                    if (!string.IsNullOrEmpty(datos.datosEnvio.numeroContenedor))
                    {
                        _despatchAdvice.Shipment.TransportHandlingUnit = new TransportHandlingUnitType[]
                        {
                    new TransportHandlingUnitType()
                    {
                        //Numero de Contenedor
                        TransportEquipment =new TransportEquipmentType[]
                        {
                            new TransportEquipmentType()
                            {
                                ID = new IDType() { Value = datos.datosEnvio.numeroContenedor }
                            }
                        }
                    }
                        };
                    }

                    //Puerto o Aeropuerto de embarque/desembarque
                    if (!string.IsNullOrEmpty(datos.datosEnvio.codigoPuerto))
                    {
                        _despatchAdvice.Shipment.FirstArrivalPortLocation = new LocationType1()
                        {
                            ID = new IDType() { Value = datos.datosEnvio.codigoPuerto }
                        };
                    }

                    break;
                case "31":
                    //Datos del transportista
                    if (datos.transportista != null)
                    {
                        _despatchAdvice.DespatchSupplierParty = new SupplierPartyType()
                        {
                            Party = new PartyType()
                            {
                                PartyLegalEntity = new PartyLegalEntityType[]
                                {
                                new PartyLegalEntityType()
                                {
                                    RegistrationName = new RegistrationNameType()
                                    {
                                        Value = datos.transportista.razonSocial
                                    }
                                }
                                },

                                PartyIdentification = new PartyIdentificationType[]
                                {
                                new PartyIdentificationType()
                                {
                                    ID = new IDType()
                                    {
                                        Value = datos.transportista.ruc,
                                        schemeID = "6",
                                        schemeName = "Documento de Identidad",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                    }
                                }
                                }
                            }
                        };

                        //Número de Registro MTC
                        if (!string.IsNullOrEmpty(datos.transportista.registroMTC))
                        {
                            _despatchAdvice.Shipment.ShipmentStage[0].CarrierParty = new PartyType[]
                            {
                                new PartyType()
                                {
                                    PartyLegalEntity = new PartyLegalEntityType[]
                                    {
                                        new PartyLegalEntityType()
                                        {
                                            CompanyID = new CompanyIDType()
                                            {
                                                Value = datos.transportista.registroMTC
                                            }
                                        }
                                    }
                                }
                            };
                        }

                        if (datos.transportista.autorizacionesEspeciales?.Count > 0)
                        {
                            var _autorizaciones = new List<PartyLegalEntityType>();

                            foreach (var item in datos.transportista.autorizacionesEspeciales)
                            {
                                _autorizaciones.Add(new PartyLegalEntityType()
                                {
                                    CompanyID = new CompanyIDType()
                                    {
                                        Value = item.numeroAutorizacion,
                                        schemeID = item.codigoEntidadAutorizadora,
                                        schemeName = "Entidad Autorizadora",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogoD37"
                                    }
                                });
                            }

                            //Cuando datos.transportista.registroMTC es vacío/nulo entonces no se inicializa esta propiedad
                            if (_despatchAdvice.Shipment.ShipmentStage[0].CarrierParty == null)
                            {
                                _despatchAdvice.Shipment.ShipmentStage[0].CarrierParty = new PartyType[]
                                {
                                    new PartyType()
                                };
                            }

                            _despatchAdvice.Shipment.ShipmentStage[0].CarrierParty[0].AgentParty = new PartyType()
                            {
                                PartyLegalEntity = _autorizaciones.ToArray()
                            };
                        }
                    }

                    //Datos del Remitente
                    if (datos.remitente != null)
                    {
                        _despatchAdvice.Shipment.Delivery.Despatch.DespatchParty = new PartyType()
                        {
                            PartyLegalEntity = new PartyLegalEntityType[]
                            {
                                new PartyLegalEntityType()
                                {
                                    RegistrationName = new RegistrationNameType()
                                    {
                                        Value = datos.remitente.nombre
                                    }
                                }
                            },

                            PartyIdentification = new PartyIdentificationType[]
                            {
                                new PartyIdentificationType()
                                {
                                    ID = new IDType()
                                    {
                                        Value = datos.remitente.numeroDocumentoIdentificacion,
                                        schemeID = datos.remitente.tipoDocumentoIdentificacion,
                                        schemeName = "Documento de Identidad",
                                        schemeAgencyName = "PE:SUNAT",
                                        schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                    }
                                }
                            }
                        };
                    }

                    if (datos.empresaSubcontrata != null)
                    {
                        _despatchAdvice.Shipment.Consignment = new ConsignmentType[]
                        {
                            new ConsignmentType()
                            {
                                ID = new IDType()
                                {
                                    Value = "SUNAT_Envio"
                                },
                                LogisticsOperatorParty = new PartyType()
                                {
                                    PartyLegalEntity = new PartyLegalEntityType[]
                                    {
                                        new PartyLegalEntityType()
                                        {
                                            RegistrationName = new RegistrationNameType()
                                            {
                                                Value = datos.empresaSubcontrata.razonSocial
                                            }
                                        }
                                    },

                                    PartyIdentification = new PartyIdentificationType[]
                                    {
                                        new PartyIdentificationType()
                                        {
                                            ID = new IDType()
                                            {
                                                Value = datos.empresaSubcontrata.ruc,
                                                schemeID = "6",
                                                schemeName = "Documento de Identidad",
                                                schemeAgencyName = "PE:SUNAT",
                                                schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                            }
                                        }
                                    }
                                }
                            }
                        };
                    }

                    if (datos.pagadorFlete != null)
                    {
                        _despatchAdvice.OriginatorCustomerParty = new CustomerPartyType()
                        {
                            Party = new PartyType()
                            {
                                PartyLegalEntity = new PartyLegalEntityType[]
                                {
                                    new PartyLegalEntityType()
                                    {
                                        RegistrationName = new RegistrationNameType()
                                        {
                                            Value = datos.pagadorFlete.nombre
                                        }
                                    }
                                },

                                PartyIdentification = new PartyIdentificationType[]
                                {
                                    new PartyIdentificationType()
                                    {
                                        ID = new IDType()
                                        {
                                            Value = datos.pagadorFlete.numeroDocumentoIdentificacion,
                                            schemeID = datos.pagadorFlete.tipoDocumentoIdentificacion,
                                            schemeName = "Documento de Identidad",
                                            schemeAgencyName = "PE:SUNAT",
                                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                                        }
                                    }
                                }
                            }
                        };
                    }

                    break;
            }

            //Observaciones (an..250 C)
            if (datos.observaciones != null)
            {
                _despatchAdvice.Note = GetObservaciones(datos.observaciones);
            }

            //Documentos relacionados
            if (datos.documentosRelacionados?.Count > 0)
            {
                _despatchAdvice.AdditionalDocumentReference = GetDocumentosReferenciaAdicionales(datos.documentosRelacionados);
            }

            //la propiedad "vehiculos" tiene prioridad sobre la propiedad "placasVehiculo" que ya no se debería usar
            if (datos.datosEnvio.vehiculos?.Count > 0)
            {
                var _TransportEquipment = new List<TransportEquipmentType>();

                //Vehiculo secundario
                var _AttachedTransportEquipment = new List<TransportEquipmentType>();

                var _esPrincipal = true;

                foreach (var item in datos.datosEnvio.vehiculos)
                {
                    var _vehiculo = new TransportEquipmentType()
                    {
                        ID = new IDType()
                        {
                            Value = item.numeroPlaca
                        }
                    };

                    //Si existe Tuc lo agregamos
                    if (!string.IsNullOrEmpty(item.numeroTarjeta))
                    {
                        _vehiculo.ApplicableTransportMeans = new TransportMeansType()
                        {
                            RegistrationNationalityID = new RegistrationNationalityIDType()
                            {
                                Value = item.numeroTarjeta
                            }
                        };
                    }

                    //Agregamos las autorizaciones especiales si existen
                    if (item.autorizacionesEspeciales?.Count > 0)
                    {
                        var _autorizacionesEspeciales = new List<DocumentReferenceType>();

                        foreach (var _autorizacion in item.autorizacionesEspeciales)
                        {
                            _autorizacionesEspeciales.Add(new DocumentReferenceType()
                            {
                                ID = new IDType()
                                {
                                    Value = _autorizacion.numeroAutorizacion,
                                    schemeID = _autorizacion.codigoEntidadAutorizadora,
                                    schemeName = "Entidad Autorizadora",
                                    schemeAgencyName = "PE:SUNAT",
                                    schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogoD37"
                                }
                            });
                        }

                        _vehiculo.ShipmentDocumentReference = _autorizacionesEspeciales.ToArray();
                    }

                    if (_esPrincipal)
                    {
                        _TransportEquipment.Add(_vehiculo);
                    }
                    else
                    {
                        _AttachedTransportEquipment.Add(_vehiculo);
                    }

                    //después de la primera interacción ya no es el vehículo principal
                    _esPrincipal = false;
                }

                if (_AttachedTransportEquipment.Count > 0)
                {
                    _TransportEquipment[0].AttachedTransportEquipment = _AttachedTransportEquipment.ToArray();
                }

                _despatchAdvice.Shipment.TransportHandlingUnit = new TransportHandlingUnitType[]
                {
                    new TransportHandlingUnitType()
                    {
                        TransportEquipment = _TransportEquipment.ToArray()
                    }
                };
            }
            else
            {
                //Vehiculos
                if (datos.datosEnvio.placasVehiculo?.Count > 0)
                {
                    var _TransportEquipment = new List<TransportEquipmentType>();

                    //Vehiculo secundario
                    var _AttachedTransportEquipment = new List<TransportEquipmentType>();

                    var _esPrincipal = true;

                    foreach (var item in datos.datosEnvio.placasVehiculo)
                    {
                        var _vehiculo = new TransportEquipmentType()
                        {
                            ID = new IDType()
                            {
                                Value = item
                            }
                        };

                        if (_esPrincipal)
                        {
                            _TransportEquipment.Add(_vehiculo);
                        }
                        else
                        {
                            _AttachedTransportEquipment.Add(_vehiculo);
                        }

                        //después de la primera interacción ya no es el vehículo principal
                        _esPrincipal = false;
                    }

                    if (_AttachedTransportEquipment.Count > 0)
                    {
                        _TransportEquipment[0].AttachedTransportEquipment = _AttachedTransportEquipment.ToArray();
                    }

                    _despatchAdvice.Shipment.TransportHandlingUnit = new TransportHandlingUnitType[]
                    {
                        new TransportHandlingUnitType()
                        {
                            TransportEquipment = _TransportEquipment.ToArray()
                        }
                    };
                }
            }


            //CONDUCTOR (Transporte Privado)
            if (datos.datosEnvio.conductores != null && datos.datosEnvio.conductores.Count > 0)
            {
                var _driverPerson = new List<PersonType>();

                var _jobTitle = "Principal";

                foreach (var item in datos.datosEnvio.conductores)
                {
                    _driverPerson.Add(new PersonType()
                    {
                        JobTitle = new JobTitleType()
                        {
                            Value = _jobTitle
                        },

                        ID = new IDType()
                        {
                            //Numero de documento de identidad del conductor
                            Value = item.numeroDocumentoIdentificacion,

                            //Tipo de documento de identidad del conductor
                            schemeID = item.tipoDocumentoIdentificacion,

                            schemeName = "Documento de Identidad",
                            schemeAgencyName = "PE:SUNAT",
                            schemeURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo06"
                        },

                        FirstName = new FirstNameType()
                        {
                            Value = item.nombres
                        },

                        FamilyName = new FamilyNameType()
                        {
                            Value = item.apellidos
                        },

                        IdentityDocumentReference = new DocumentReferenceType[]
                        {
                                new DocumentReferenceType()
                                {
                                    ID = new IDType()
                                    {
                                        Value = item.licenciaConducir
                                    }
                                }
                        }
                    });

                    //después de la primera interacción es conductor "Secundario"
                    _jobTitle = "Secundario";
                }

                _despatchAdvice.Shipment.ShipmentStage[0].DriverPerson = _driverPerson.ToArray();
            }

            //Direccion punto de llegada
            if (datos.datosEnvio.puntoLlegada != null)
            {
                _despatchAdvice.Shipment.Delivery.DeliveryAddress = GetDireccion(datos.datosEnvio.puntoLlegada);
            }

            return _despatchAdvice;
        }
    }
}
