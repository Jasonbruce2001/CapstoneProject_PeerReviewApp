namespace PeerReviewApp.Models;

public class Assignment
{
    public int Id { get; set; }
    public Course Course { get; set; }
    public DateTime DueDate { get; set; } //assignment due date
    public DateTime ReviewDueDate { get; set; }
    public string Title { get; set; }
    public IList<AssignmentVersion> Versions { get; set; }
}