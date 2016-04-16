using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppUtils
{
    public static class EnumHelper
    {
        /// <summary>
        /// Returns list of EnumDesc where EnumDesc item value is integer
        /// and EnumDesc description is description
        /// </summary>
        public static List<EnumDescription> ToList(Type enumType)
        {
            var result = new List<EnumDescription>();
            foreach (var enumItem in Enum.GetValues(enumType))
            {
                var descriptionAttribute = enumType.GetField(Enum.GetName(enumType, enumItem)).GetCustomAttributes(false)
                    .FirstOrDefault(item => item is DescriptionAttribute) as DescriptionAttribute;

                result.Add(
                    new EnumDescription(
                        (int)enumItem,
                        descriptionAttribute != null ? descriptionAttribute.Description : Enum.GetName(enumType, enumItem)));
            }

            return result;
        }

        public static List<string> ToDescriptionsList(Type enumType)
        {
            var result = new List<string>();
            foreach (var enumItem in Enum.GetValues(enumType))
            {
                var descriptionAttribute = enumType.GetField(Enum.GetName(enumType, enumItem)).GetCustomAttributes(false)
                    .FirstOrDefault(item => item is DescriptionAttribute) as DescriptionAttribute;

                result.Add(descriptionAttribute != null ? descriptionAttribute.Description : Enum.GetName(enumType, enumItem));
            }

            return result;
        }

        /// <summary>
        /// Returns list of EnumDesc where EnumDesc item value is enum (original value)
        /// and EnumDesc description is description
        /// </summary>
        public static List<EnumDescription> ToListOfOriginalValueAndDescription(Type enumType)
        {
            var result = new List<EnumDescription>();
            foreach (var enumItem in Enum.GetValues(enumType))
            {
                var descriptionAttribute = enumType.GetField(Enum.GetName(enumType, enumItem)).GetCustomAttributes(false)
                    .FirstOrDefault(item => item is DescriptionAttribute) as DescriptionAttribute;

                result.Add(
                    new EnumDescription(
                        enumItem,
                        descriptionAttribute != null ? descriptionAttribute.Description : Enum.GetName(enumType, enumItem)));
            }

            return result;
        }

        public static string GetValueDescription<T>(T enumValue)
        {
            var enumDectiprion =
                ToListOfOriginalValueAndDescription(typeof (T))
                    .FirstOrDefault(item => ((T) item.Value).Equals(enumValue));
            return enumDectiprion != null ? enumDectiprion.Description : string.Empty;
        }
    }

    /// <summary>
    /// Description of an enum value
    /// </summary>
    public class EnumDescription
    {
        public EnumDescription(object value, string description)
        {
            Value = value;
            Description = description;
        }

        public object Value { get; private set; }
        public string Description { get; private set; }
    }
}
