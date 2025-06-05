namespace PeerReviewApp.Models
{
    public class StudentListVM
    {
        public IList<StudentClassInfo> Students { get; set; } = new List<StudentClassInfo>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalStudents { get; set; }
        public string SearchTerm { get; set; } = "";
    }

    public class StudentClassInfo
    {
        public AppUser Student { get; set; }
        public Class Class { get; set; }
    }
}