using arTWander.Data;
using System.ComponentModel.DataAnnotations;

namespace arTWander.ModelValid
{
    public class CheckEmailAttribute : ValidationAttribute
    {
        private string _errorMessage = "";
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _errorMessage = value;
            }
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ApplicationContext context = (ApplicationContext)validationContext.GetService(typeof(ApplicationContext));

            string email = (string)value;

            var result = context.Users.Any(m => m.Email == email);

            if (result)
                return new ValidationResult(_errorMessage);

            return ValidationResult.Success;
        }
    }
}
