// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using GasperSoft.SUNAT.DTO.Resumen;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V1
{
    public class ComunicacionBaja
    {
        private static VoidedDocumentsLineType[] GetItems(List<ItemComunicacionBajaType> items)
        {
            var _voidedDocumentsLines = new List<VoidedDocumentsLineType>();

            int _secuenciaId = 1;

            foreach (var item in items)
            {
                _voidedDocumentsLines.Add(new VoidedDocumentsLineType()
                {
                    LineID = new LineIDType()
                    {
                        Value = _secuenciaId.ToString()
                    },

                    DocumentTypeCode = new DocumentTypeCodeType()
                    {
                        Value = item.tipoDocumento
                    },

                    DocumentSerialID = new IdentifierType()
                    {
                        Value = item.serie
                    },

                    DocumentNumberID = new IdentifierType()
                    {
                        Value = item.numero.ToString()
                    },

                    VoidReasonDescription = new TextType()
                    {
                        Value = item.motivo
                    }
                });

                _secuenciaId++;
            }

            return _voidedDocumentsLines.ToArray();
        }

        public static VoidedDocumentsType GetDocumento(ComunicacionBajaType datos, EmisorType emisor, string signature = "signatureGASPERSOFT")
        {
            var _voidedDocuments = new VoidedDocumentsType()
            {
                //Aqui colocamos la informacion del EMISOR
                AccountingSupplierParty = Comun.GetEmisor(emisor),

                ReferenceDate = new ReferenceDateType()
                {
                    Value = datos.fechaBaja
                },

                VoidedDocumentsLine = GetItems(datos.detalles),

                UBLExtensions = new UBLExtensionType[]
                {
                    //Aqui se va colocar la firma
                    new UBLExtensionType(){ }
                },

                Signature = Comun.GetSignature(emisor, signature),

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
                    Value = "1.0"
                }
            };

            return _voidedDocuments;
        }
    }
}
