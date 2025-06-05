namespace PeerReviewApp.Models
{
    public class AdminDashboardVM
    {
        public int TotalInstitutions { get; set; }
        public int ActiveInstructors { get; set; }

        public int ActiveCourses { get; set; }

        public int TotalStudents { get; set; }

        public IEnumerable<Institution> Institutions { get; set; }

        public IEnumerable<string> RecentActions { get; set; }

        public List<string> RecentPasswordResetRequests { get; set; } = new();
    }
}


