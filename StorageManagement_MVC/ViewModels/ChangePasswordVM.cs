using System.ComponentModel.DataAnnotations;


namespace StorageManagement_MVC.ViewModels
{
    public class ChangePasswordVM
    {
        [Display(Name = "Old password")]
        [Required(ErrorMessage = "Please enter Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "New password")]
        [Required(ErrorMessage = "Please enter New password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Please enter New password")]
        [DataType(DataType.Password)]
        [ComparePasswords("NewPassword", ErrorMessage = "Confirm and New password must be the same")]
        public string ConfirmPassword { get; set; }
    }

    public class ComparePasswordsAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public ComparePasswordsAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = value as string;
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonProperty == null)
                throw new ArgumentException("Property with this name not found");

            var comparisonValue = comparisonProperty.GetValue(validationContext.ObjectInstance) as string;

            if (currentValue != comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? "Confirm and New password must be the same");
            }

            return ValidationResult.Success;
        }
    }
}
