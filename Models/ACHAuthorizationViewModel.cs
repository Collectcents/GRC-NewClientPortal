using System.ComponentModel.DataAnnotations;

namespace GRC_NewClientPortal.Models
{
    public class ACHAuthorizationViewModel : IValidatableObject
    {
        // -----------------------
        // Account Information
        // -----------------------

        [Display(Name = "Checking Account Number")]
        [StringLength(17, ErrorMessage = "Checking Account Number must be 17 digits or less.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Checking Account Number must contain only numbers.")]
        public string? CheckingAccountNumber { get; set; }

        [Display(Name = "Savings Account Number")]
        [StringLength(17, ErrorMessage = "Savings Account Number must be 17 digits or less.")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Savings Account Number must contain only numbers.")]
        public string? SavingsAccountNumber { get; set; }

        [Required(ErrorMessage = "Routing Number is required.")]
        [Display(Name = "Routing Number")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Routing Number must be 9 digits.")]
        public string RoutingNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Financial Institution's Name is required.")]
        [Display(Name = "Financial Institution's Name")]
        [StringLength(100)]
        public string FIN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Financial Institution's Address is required.")]
        [Display(Name = "Financial Institution's Address")]
        [StringLength(120)]
        public string FIA { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        [StringLength(60)]
        public string CCity { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        [StringLength(10)]
        public string CState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zipcode is required.")]
        [Display(Name = "Zipcode")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Valid zip code xxxxx-xxxx or xxxxx required.")]
        public string CZipcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Effective Date is required.")]
        [Display(Name = "Effective Date")]
        [DataType(DataType.Date)]
        public DateTime? CEffectiveDate { get; set; }

        [Required(ErrorMessage = "Federal ID Number is required.")]
        [Display(Name = "Federal ID Number")]
        [StringLength(30)]
        public string FIDN { get; set; } = string.Empty;

        // -----------------------
        // Authorized Agent Information
        // -----------------------

        [Required(ErrorMessage = "Name of Authorized Agent of Company is required.")]
        [Display(Name = "Name of Authorized Agent of Company")]
        [StringLength(100)]
        public string NAgent { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company Name is required.")]
        [Display(Name = "Company Name")]
        [StringLength(120)]
        public string CN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company Address is required.")]
        [Display(Name = "Company Address")]
        [StringLength(120)]
        public string CA { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required.")]
        [Display(Name = "City")]
        [StringLength(60)]
        public string ACity { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required.")]
        [Display(Name = "State")]
        [StringLength(10)]
        public string AState { get; set; } = string.Empty;

        [Required(ErrorMessage = "Zipcode is required.")]
        [Display(Name = "Zipcode")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Valid zip code xxxxx-xxxx or xxxxx required.")]
        public string AZipcode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? ADate { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [Display(Name = "Phone Number")]
        [StringLength(25)]
        public string APhoneN { get; set; } = string.Empty;

        [Display(Name = "Comments")]
        [StringLength(500)]
        public string? AComments { get; set; }

        // -----------------------
        // Cross-field rule:
        // Either Checking OR Savings, not both, not none
        // -----------------------
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var hasChecking = !string.IsNullOrWhiteSpace(CheckingAccountNumber);
            var hasSavings = !string.IsNullOrWhiteSpace(SavingsAccountNumber);

            if (hasChecking == hasSavings) // both true OR both false
            {
                yield return new ValidationResult(
                    "Enter either Checking Account Number or Savings Account Number (but not both).",
                    new[] { nameof(CheckingAccountNumber), nameof(SavingsAccountNumber) }
                );
            }
        }
    }
}
