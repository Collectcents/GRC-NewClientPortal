using System.ComponentModel.DataAnnotations;

namespace GRC_NewClientPortal.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Old password is required.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%]).{8,20}$",
            ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit, and one special character (!@#$%).")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
