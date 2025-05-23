using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerReviewApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Institution Institution { get; set; }
        public IList<Assignment> Assignments { get; set; }
        public IList<Class> Subclasses { get; set; }
    }
}

