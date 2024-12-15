// Licencia MIT 
// Copyright (C) 2024 GasperSoft.
// Contacto: it@gaspersoft.com

using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace GasperSoft.SUNAT
{
    internal static class XmlSerializerExtensions
    {
        private static string GetNumberFormat(int numeroDecimales)
        {
            string _numberFormat = "0.";

            if (numeroDecimales == 0)
            {
                return _numberFormat;
            }

            for (int i = 0; i < numeroDecimales; i++)
            {
                _numberFormat = _numberFormat + "0";
            }

            return _numberFormat;
        }

        internal static void SerializeWithDecimalFormatting(this XmlSerializer serializer, XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces)
        {
            IteratePropertiesRecursively(o);

            serializer.Serialize(xmlWriter, o, namespaces);
        }

        private static void IteratePropertiesRecursively(object o)
        {
            if (o == null)
                return;

            var type = o.GetType();

            var properties = type.GetProperties();

            string _numberFormat = GetNumberFormat(2);

            switch (type.Name)
            {
                case "MultiplierFactorNumericType":
                case "PercentType1":
                    _numberFormat = GetNumberFormat(5);
                    break;
                case "PriceAmountType":
                case "InvoicedQuantityType":
                case "DebitedQuantityType":
                case "CreditedQuantityType":
                    _numberFormat = GetNumberFormat(10);
                    break;
                case "GrossWeightMeasureType":
                    _numberFormat = GetNumberFormat(3);
                    break;
                case "BaseUnitMeasureType":
                case "DurationMeasureType":
                    _numberFormat = GetNumberFormat(0);
                    break;
                case "CalculationRateType":
                    _numberFormat = GetNumberFormat(6);
                    break;
            }

            // enumerate the properties of the type
            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;

                // if property is a generic list
                if (propertyType.Name == "List`1" || propertyType.Name.ToLower().Contains("[]"))
                {
                    var val = property.GetValue(o, null);

                    if (val is IList elements)
                    {
                        // then iterate through all elements
                        foreach (var item in elements)
                        {
                            IteratePropertiesRecursively(item);
                        }
                    }
                }
                else if (propertyType == typeof(decimal))
                {
                    // check if there is a property with name XXXSpecified, this is the case if we have a type of decimal?
                    var specifiedPropertyName = string.Format("{0}Specified", property.Name);
                    var isSpecifiedProperty = type.GetProperty(specifiedPropertyName);
                    if (isSpecifiedProperty != null)
                    {
                        // only apply the format if the value of XXXSpecified is true, otherwise we will get a nullRef exception for decimal? types
                        var isSpecifiedPropertyValue = isSpecifiedProperty.GetValue(o, null) as bool?;
                        if (isSpecifiedPropertyValue == true)
                        {
                            FormatDecimal(property, o, _numberFormat);
                        }
                    }
                    else
                    {
                        // if there is no property with name XXXSpecified, we can safely format the decimal
                        FormatDecimal(property, o, _numberFormat);
                    }
                }
                else
                {
                    // if property is a XML class (contains XML in name) iterate through properties of this class
                    if (propertyType.Name.ToLower().Contains("type") && propertyType.IsClass)
                    {
                        IteratePropertiesRecursively(property.GetValue(o, null));
                    }
                }
            }
        }

        private static void FormatDecimal(PropertyInfo p, object o, string numberFormat)
        {
            var value = (decimal)p.GetValue(o, null);
            var formattedString = value.ToString(numberFormat, CultureInfo.InvariantCulture);
            p.SetValue(o, decimal.Parse(formattedString), null);
        }
    }
}
