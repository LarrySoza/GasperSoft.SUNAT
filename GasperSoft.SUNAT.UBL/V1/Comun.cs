// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using GasperSoft.SUNAT.DTO;
using System.Collections.Generic;

namespace GasperSoft.SUNAT.UBL.V1
{
    public class Comun
    {
        internal static SupplierPartyType GetEmisor(EmisorType emisor)
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

                    //Note = new NoteType()
                    //{
                    //    Value = "GASPERSOFT"
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
    }
}
