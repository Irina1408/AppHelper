using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppCommon.ValidationRules
{
    public class RangeValidationRule : ValidationRule
    {
        private Type type;

        public Object MinValue { get; private set; }
        public Object MaxValue { get; private set; }

        public RangeValidationRule(Type type, object minValue, object maxValue)
        {
            this.type = type;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Value cannot be empty!");

            var stringValue = value.ToString();

            try
            {
                if ((type == typeof(int) &&
                    ((int)value < (int)MinValue || (int)value > (int)MaxValue))
                    ||
                    (type == typeof(decimal) &&
                     (decimal.Parse(stringValue, cultureInfo) < decimal.Parse(MinValue.ToString(), cultureInfo) || decimal.Parse(stringValue, cultureInfo) > decimal.Parse(MaxValue.ToString(), cultureInfo)))
                    ||
                    (type == typeof(double) &&
                    ((double)value < (double)MinValue || (double)value > (double)MaxValue)))
                {
                    return new ValidationResult(false, "Acceptable value range: [" + MinValue + ";" + MaxValue + "]");
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Value is not correct! Should be numeric value!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
