using EvidenceProject.Controllers;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Tests.ControllersTests
{
    public class HomeControllerTests
    {
        private ProjectController Controller = new ProjectController(new ProjectContext(new DbContextOptions<ProjectContext>()));

        [Test]
        [TestCase("test", "test")]
        [TestCase("est", "test")]
        [TestCase("st", "test")]
        public void SearchProject(string projectQuery, string expectedProject)
        {
            var response = Controller.Search(projectQuery) as ViewResult;
            var data = response?.Model as List<Project>;
            Assert.AreEqual(expectedProject, data?.First().name);
        }
    }
}