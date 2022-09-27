using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection.Emit;

namespace EvidenceProject.Data;

public class ProjectContext: DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

    public DbSet<Project>? projects { get; set; }
    public DbSet<AuthUser>? globalUsers { get; set; }
    
    public DbSet<DialInfo>? dialInfos { get; set; }
    public DbSet<DialCode>? dialCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // M:N DB enitity connections

        builder.Entity<Project>().HasMany(p => p.assignees);

        builder.Entity<Project>().HasMany(p => p.projectAchievements);

        builder.Entity<DialCode>().HasOne(d => d.dialInfo).WithMany(d => d.dialCodes);

        // Duplicates

        builder.Entity<AuthUser>().HasIndex(u => u.username).IsUnique();

        // Cascades

        builder.Entity<Project>().HasOne(p => p.projectState).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Project>().HasOne(p => p.projectTechnology).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Project>().HasOne(p => p.projectType).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}