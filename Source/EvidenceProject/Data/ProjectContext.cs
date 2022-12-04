using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data;

public class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options){}

    public DbSet<Project>? projects { get; set; }
    public DbSet<AuthUser>? globalUsers { get; set; }

    public DbSet<DialInfo>? dialInfos { get; set; }
    public DbSet<DialCode>? dialCodes { get; set; }

    public DbSet<DbFile>? files { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        // M:N DB enitity connections

        builder.Entity<Project>().HasMany(p => p.assignees).WithMany(a => a.Projects);

        builder.Entity<Project>().HasMany(p => p.applicants).WithMany();

        builder.Entity<Project>().HasMany(p => p.files).WithOne();

        builder.Entity<Project>().HasMany(p => p.projectAchievements).WithOne(a => a.project);

        builder.Entity<DialCode>().HasOne(d => d.dialInfo).WithMany(d => d.dialCodes);

        // Duplicates

        builder.Entity<AuthUser>().HasIndex(u => u.id_key).IsUnique();

        builder.Entity<AuthUser>().HasIndex(u => u.username).IsUnique();

        builder.Entity<DialInfo>().HasIndex(d => d.name).IsUnique();

        builder.Entity<DialCode>().HasIndex(d => d.name).IsUnique();

        // Cascades
        builder.Entity<Project>().HasOne(p => p.projectState).WithMany().OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Project>().HasMany(p => p.projectTechnology).WithMany();
        builder.Entity<Project>().HasOne(p => p.projectType).WithMany().OnDelete(DeleteBehavior.Restrict);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=EvidenceContext;Trusted_Connection=True;MultipleActiveResultSets=True");
    }
}