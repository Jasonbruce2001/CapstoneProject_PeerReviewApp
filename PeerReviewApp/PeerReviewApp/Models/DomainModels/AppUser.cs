﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PeerReviewApp.Models
{
        public class AppUser : IdentityUser
        {
            [NotMapped]
            public IList<string> RoleNames { get; set; } = null!;
            public string? InstructorCode { get; set; }
            public DateTime AccountAge { get; set; }
        }
}
