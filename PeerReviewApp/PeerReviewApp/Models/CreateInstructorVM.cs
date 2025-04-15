using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class CreateInstructorVM
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Institutiion Code Is required")]
        [Display(Name = "Institution")]
        public int InstitutionId { get; set; }


        public IList<Institution> Institutions { get; set; } = new List<Institution>(); 
    }
}

