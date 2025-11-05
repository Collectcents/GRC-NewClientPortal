using System.ComponentModel.DataAnnotations;

namespace GRC_NewClientPortal.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        public string Signon { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string MaidenName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
