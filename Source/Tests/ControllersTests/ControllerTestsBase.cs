using EvidenceProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Tests.ControllersTests;
public class ControllerTestsBase
{
    public static LoggerFactory LoggerFactory = new();
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

