using Microsoft.AspNetCore.Identity;
using PeerReviewApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PeerReviewApp.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    // constructor just calls the base class constructor
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // one DbSet for each domain model class
    public DbSet<Assignment> Assignments { get; set; } = default!;
    public DbSet<AssignmentVersion> AssignmentVersions { get; set; } = default!;
    public DbSet<Class> Classes { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<Document> Documents { get; set; } = default!;
    public DbSet<Grade> Grades { get; set; } = default!;
    public DbSet<Institution> Institutions { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<ReviewGroup> ReviewGroups { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}