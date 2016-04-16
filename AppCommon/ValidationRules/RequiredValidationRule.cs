using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppCommon.ValidationRules
{
    public class RequiredValidationRule : ValidationRule
    {
        public RequiredValidationRule(bool isRequired = false)
        {
            IsRequired = isRequired;
        }

        public bool IsRequired { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (IsRequired && (value is string && string.IsNullOrEmpty(value as string) || value == null))
                return new ValidationResult(false, "The property is required!");
            return ValidationResult.ValidResult;
        }
    }
}
