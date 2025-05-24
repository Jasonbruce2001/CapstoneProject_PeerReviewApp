using System.ComponentModel.DataAnnotations;

public class EditAssignmentVM
{
    public int Id { get; set; }
    public int ClassId { get; set; }

    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Due date is required")]
    [Display(Name = "Due Date")]
    [DataType(DataType.Date)]
    public DateTime DueDate { get; set; }
}