// Licencia MIT 
// Copyright (C) 2023 GasperSoft.
// Contacto: it@gaspersoft.com

using System;
using System.Xml.Serialization;

namespace GasperSoft.SUNAT.UBL.V2
{
    /// <summary>
    /// Estructura usada por el CDR de SUNAT
    /// </summary>
    [Serializable()]
    [XmlType(Namespace = "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2")]
    [XmlRoot("ApplicationResponse", Namespace = "urn:oasis:names:specification:ubl:schema:xsd:ApplicationResponse-2", IsNullable = false)]
    public class RespuestaSunat : ApplicationResponseType
    {

    }
}
