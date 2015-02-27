using System;
using System.ComponentModel.DataAnnotations;

namespace Esd.FlexibleOpenGeographies.Web.ValidationAttributes
{
    public class UriAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //Allow RequiredAttribute (or lack thereof) to decide whether missing value is acceptable
            if (value == null) return ValidationResult.Success;
            return Uri.IsWellFormedUriString(value.ToString(), UriKind.Absolute) 
                ? ValidationResult.Success 
                : new ValidationResult("Please enter a valid URI");
        }
    }
}