using EvidenceProject.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.ControllersTests;
public class ControllerTestsBase
{
    /// <summary>
    /// Získání dat z DB
    /// </summary>
    public ProjectContext GetContext()
    {
        var builder = new DbContextOptionsBuilder<ProjectContext>();
        var context = new ProjectContext(builder.Options);
        return context;
    }
}

