// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.CPE;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V2
{
    /// <remarks/>
    public static class NotaDebito
    {
        private static ResponseType[] GetMotivosNota(CPEType datos)
        {
            var _motivos = new List<ResponseType>();

            foreach (var item in datos.motivosNota)
            {
                _motivos.Add(new ResponseType()
                {
                    //Código de tipo de nota de debito - Catálogo No. 10
                    ResponseCode = new ResponseCodeType()
                    {
                        Value = item.tipoNota,
                        listAgencyName = "PE:SUNAT",
                        listName = "Tipo de nota de debito",
                        listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo10"
                    },

                    //Motivo o Sustento (an..500 M)
                    Description = new DescriptionType[]
                    {
                        new DescriptionType()
                        {
                            Value = item.sustento
                        }
                    }
                });
            }

            return _motivos.ToArray();
        }

        private static BillingReferenceType[] GetDocumentosModifica(CPEType datos)
        {
            var _billingReference = new List<BillingReferenceType>();

            foreach (var item in datos.motivosNota)
            {
                if (!string.IsNullOrEmpty(item.serie))
                {
                    _billingReference.Add(new BillingReferenceType()
                    {
                        InvoiceDocumentReference = new DocumentReferenceType()
                        {
                            ID = new IDType()
                            {
                                Value = $"{item.serie}-{item.numero}"
                            },

                            DocumentTypeCode = new DocumentTypeCodeType()
                            {
                                Value = item.tipoDocumento,
                                listAgencyName = "PE:SUNAT",
                                listName = "Tipo de Documento",
                                listURI = "urn:pe:gob:sunat:cpe:see:gem:catalogos:catalogo01"
                            }
                        }
                    });
                }
            }

            if (_billingReference.Count > 0)
            {
                return _billingReference.ToArray();
            }

            return null;
        }

        private static DebitNoteLineType[] GetItems(List<ItemCPEType> items, string codMoneda)
        {
            var _debitNoteLines = new List<DebitNoteLineType>();
            int _secuenciaId = 1;

            foreach (var item in items)
            {
                var _debitNoteLine = new DebitNoteLineType()
                {
                    //Número de orden del Ítem (n..3 M)
                    ID = new IDType()
                    {
                        Value = _secuenciaId.ToString()
                    },

                    //Unidad de medida por ítem
                    DebitedQuantity = new DebitedQuantityType()
                    {
                        //Catálogo No. 03(an..3 M)
                        unitCode = item.unidadMedida,
                        unitCodeListID = "UN/ECE rec 20",
                        unitCodeListAgencyName = "United Nations Economic Commission for Europe",
                        //Cantidad de unidades por ítem(an..16 M n(12,10))
                        Value = item.cantidad
                    },

                    Item = Comun.GetItem(item),

                    //Valor unitario por ítem (an..15 M n(12,2))
                    Price = new PriceType()
                    {
                        PriceAmount = new PriceAmountType()
                        {
                            currencyID = codMoneda,
                            Value = item.valorVentaUnitario
                        }
                    },

                    PricingReference = Comun.GetPreciosReferenciaItem(item, codMoneda),

                    TaxTotal = Comun.GetTotalesItem(item, codMoneda),

                    //Valor de venta por ítem (M)
                    LineExtensionAmount = new LineExtensionAmountType()
                    {
                        currencyID = codMoneda,
                        Value = item.valorVenta
                    },

                    //Cargo/descuento por ítem (C)
                    //AllowanceCharge = GetDescuentosCargosItem(item, codMoneda)
                };

                _debitNoteLines.Add(_debitNoteLine);
                _secuenciaId++;
            }

            return _debitNoteLines.ToArray();
        }

        private static MonetaryTotalType GetRequestedMonetaryTotal(CPEType datos)
        {
            var _legalMonetaryTotal = new MonetaryTotalType();

            //Total Descuentos (C)
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

            //Importe total (M)
            _legalMonetaryTotal.PayableAmount = new PayableAmountType()
            {
                Value = datos.importeTotal,
                currencyID = datos.codMoneda
            };

            return _legalMonetaryTotal;
        }

        /// <summary>
        /// Convierte un objeto CPEType a DebitNoteType
        /// </summary>
        /// <param name="datos">Informacion del comprobante</param>
        /// <param name="emisor">Informacion del emisor</param>
        /// <param name="signature">Una cadena de texto que se usa para "Signature ID", Por defecto se usará la cadena predeterminada "signatureGASPERSOFT"</param>
        /// <returns>DebitNoteType con la informacion del documento</returns>
        public static DebitNoteType GetDocumento(CPEType datos, EmisorType emisor, string signature = null)
        {
            var _debitNote = new DebitNoteType()
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

                //Serie y número del comprobante "F###-NNNNNNNN"
                ID = new IDType() { Value = $"{datos.serie}-{datos.numero}" },

                //Fecha de Emision (YYYY-MM-DD M)
                IssueDate = new IssueDateType() { Value = datos.fechaEmision },

                //Hora de Emision (hh:mm:ss C)
                IssueTime = Comun.GetHoraEmision(datos.horaEmision),

                //Código de tipo de nota de crédito (M)
                DiscrepancyResponse = GetMotivosNota(datos),

                //Tipo de moneda - Catálogo No. 02 (an3 M)
                DocumentCurrencyCode = new DocumentCurrencyCodeType()
                {
                    listID = "ISO 4217 Alpha",
                    listName = "Currency",
                    listAgencyName = "United Nations Economic Commission for Europe",
                    Value = datos.codMoneda
                },

                UBLExtensions = (datos.informacionAdicionalEnXml && datos.informacionAdicional?.Count > 0) ? Comun.GetUBLExtensions(datos.informacionAdicional) : Comun.GetUBLExtensions(),

                Signature = Comun.GetSignature(emisor, signature),

                //Aqui colocamos la informacion del EMISOR
                AccountingSupplierParty = Comun.GetEmisor(emisor, datos.codigoEstablecimiento),

                //Aqui colocamos informacion del CLIENTE
                AccountingCustomerParty = Comun.GetAdquiriente(datos.adquirente),

                //Documentos que modifica (M)
                BillingReference = GetDocumentosModifica(datos),

                //Tipo y número de la guía de remisión relacionada (C)
                DespatchDocumentReference = Comun.GetGuiasRemisionRelacionadas(datos),

                //Tipo y número de otro documento relacionado (C)
                AdditionalDocumentReference = Comun.GetDocumentosReferenciaAdicionales(datos),

                //Los detalles del comprobante
                DebitNoteLine = GetItems(datos.detalles, datos.codMoneda),

                TaxTotal = Comun.GetTotales(datos),

                RequestedMonetaryTotal = GetRequestedMonetaryTotal(datos),

                //Leyenda
                Note = Comun.GetNotes(datos)
            };

            return _debitNote;
        }
    }
}
