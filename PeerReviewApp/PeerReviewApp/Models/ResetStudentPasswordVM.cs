using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class ResetStudentPasswordVM
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        public string Token { get; set; }
    }
}
