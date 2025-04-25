using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models
{
    public class AddAssignmentVM
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Term { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
    }
}
