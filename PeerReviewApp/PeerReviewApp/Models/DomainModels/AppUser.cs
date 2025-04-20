using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PeerReviewApp.Models
{
        public class AppUser : IdentityUser
        {
            [NotMapped]
            public IList<string> RoleNames { get; set; } = null!;
            public DateTime AccountAge { get; set; }
            public IList<Class> Classes { get; set; }
        }
}
