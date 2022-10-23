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
    
    public DbSet<DbFile>? files { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // M:N DB enitity connections

        builder.Entity<Project>().HasMany(p => p.assignees).WithMany(a => a.Projects);

        builder.Entity<Project>().HasMany(p => p.projectAchievements).WithOne(a => a.project);

        builder.Entity<DialCode>().HasOne(d => d.dialInfo).WithMany(d => d.dialCodes);

        // Duplicates

        builder.Entity<AuthUser>().HasIndex(u => u.username).IsUnique();

        builder.Entity<DialInfo>().HasIndex(d => d.name).IsUnique();

        builder.Entity<DialCode>().HasIndex(d => d.name).IsUnique();

        // Cascades

        builder.Entity<Project>().HasOne(p => p.projectState).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Project>().HasOne(p => p.projectTechnology).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Project>().HasOne(p => p.projectType).WithMany().OnDelete(DeleteBehavior.Restrict);
    }
}