using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;

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
        builder.Entity<Project>().HasMany(p => p.assignees);

        builder.Entity<DialCode>().HasOne(d => d.DialInfo).WithMany(d => d.DialCodes);
    }
}