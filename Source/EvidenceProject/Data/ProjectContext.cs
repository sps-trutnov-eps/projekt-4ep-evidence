using EvidenceProject.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data;

public class ProjectContext: DbContext
{

    public DbSet<Project>? projects { get; set; }
    public DbSet<AuthUser>? globalUsers { get; set; }
    
    public DbSet<DialInfo>? dialInfos { get; set; }
    public DbSet<DialCode>? dialCodes { get; set; }
    
}