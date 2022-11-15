using EvidenceProject.Controllers;
using EvidenceProject.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.ControllersTests
{
    public class ProjectControllerTests
    {
        private ProjectController Controller = new ProjectController(new ProjectContext(new DbContextOptions<ProjectContext>()));
        [Test]
        public void Test1() => Assert.Pass();
    }
}