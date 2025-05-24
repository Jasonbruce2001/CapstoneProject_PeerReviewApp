using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class ResetInstructorPasswordVM
    {
        public string InstructorId { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}


