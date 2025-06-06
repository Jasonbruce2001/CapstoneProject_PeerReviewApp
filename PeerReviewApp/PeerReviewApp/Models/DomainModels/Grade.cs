using System.ComponentModel.DataAnnotations;

namespace PeerReviewApp.Models;

public class Grade
{
    public int Id { get; set; }
    [Required]
    [Range(0, 100)]
    public int Value { get; set; }  
    public AppUser Student { get; set; }
}