using EmployeeManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace EmployeeManagement.Helpers
{
    public class EmailUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
                var email = value.ToString();
                var exists = dbContext.Employees.Any(e => e.Email == email && e.IsActive);

                if (exists)
                    return new ValidationResult("An employee with this email already exists.");
            }
            return ValidationResult.Success;
        }
    }

    public class PhoneNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var phoneNumber = value.ToString();
                var regex = new Regex(@"^[\+]?[1-9][\d]{0,15}$");
                if (!regex.IsMatch(phoneNumber))
                    return new ValidationResult("Please enter a valid phone number.");
            }
            return ValidationResult.Success;
        }
    }
}
