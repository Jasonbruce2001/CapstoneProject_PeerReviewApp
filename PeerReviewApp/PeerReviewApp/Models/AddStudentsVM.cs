using PeerReviewApp.Models;
using System.ComponentModel.DataAnnotations;

public class AddStudentsVM
{
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public string Term { get; set; }
    public List<AppUser> AvailableStudents { get; set; }
    [Display(Name = "Select Students")]
    public List<string> SelectedStudentIds { get; set; } = new List<string>();
}