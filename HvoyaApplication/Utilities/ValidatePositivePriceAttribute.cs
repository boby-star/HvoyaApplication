using System.ComponentModel.DataAnnotations;

namespace HvoyaApplication.Utilities
{
    public class ValidatePositivePriceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int price && price <= 0)
            {
                return new ValidationResult("Ціна повинна бути більшою за 0.");
            }
            return ValidationResult.Success;
        }
    }

}
