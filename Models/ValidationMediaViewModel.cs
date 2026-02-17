
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GRC_NewClientPortal.Models
{
    public class ValidationMediaViewModel
    {
        [Required(ErrorMessage = "Please enter your Institution Name")]
        [StringLength(100)]
        public string InstitutionName { get; set; }

        [Required(ErrorMessage = "Please enter your Name")]
        [StringLength(100)]
        public string SubmitterName { get; set; }

      
    [RegularExpression(
        @"^\s*(?:\+?1[\s\-\.]?)?(?:\(?\d{3}\)?[\s\-\.]?)\d{3}[\s\-\.]?\d{4}\s*$",
        ErrorMessage = "Please enter a valid 10-digit phone number (e.g., 555-555-5555).")]
    [StringLength(25)]
    public string? ContactPhone { get; set; }

        [Required(ErrorMessage = "A valid Contact E-mail is required")]
        [EmailAddress(ErrorMessage = "Contact E-mail Address is in invalid format")]
        [StringLength(50)]
        public string ContactEmail { get; set; }

        [Required(ErrorMessage = "Please select a reason for submitting media")]
        public string Reason { get; set; }

        public string? Comments { get; set; }

        // Files
        [Required(ErrorMessage = "Please provide the file containing validation media.")]
        public IFormFile ValidationMedia1 { get; set; }

        public IFormFile? ValidationMedia2 { get; set; }
        public IFormFile? ValidationMedia3 { get; set; }
    

}
}


    
