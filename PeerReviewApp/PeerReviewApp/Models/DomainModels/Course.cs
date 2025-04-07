
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerReviewApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AppUser Instructor { get; set; }
        public string Term { get; set; }
        public IList<AppUser> Students { get; set; }
    }
}

