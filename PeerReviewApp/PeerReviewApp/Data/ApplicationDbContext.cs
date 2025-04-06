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

    public DbSet<Institution> Institution { get; set; } = default!;

    // one DbSet for each domain model class
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Assignment> Assignments { get; set; } = default!;
    public DbSet<Document> Document { get; set; } = default!;
    public DbSet<Grade> Grade { get; set; } = default!;
    public DbSet<AssignmentGroup> AssignmentGroups { get; set; } = default!;
    public DbSet<PartnerGroup> PartnerGroups { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}