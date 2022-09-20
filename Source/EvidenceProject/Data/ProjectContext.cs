using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data;

public class ProjectContext: DbContext
{

    public DbSet<Project>? projects { get; set; }
    public DbSet<AuthUser>? globalUsers { get; set; }
    
    public DbSet<ProjectType>? projectTypes { get; set; }
    public DbSet<ProjectState>? projectStates { get; set; }
    
}