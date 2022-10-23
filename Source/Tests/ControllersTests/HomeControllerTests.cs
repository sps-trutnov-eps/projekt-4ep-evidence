using EvidenceProject.Controllers;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using Microsoft.AspNetCore.Mvc;
namespace Tests.ControllersTests
{
    public class HomeControllerTests : ControllerTestsBase
    {
        [Test]
        [TestCase("test", "test")]
        [TestCase("est", "test")]
        [TestCase("st", "test")]
        public void SearchProject(string projectQuery, string expectedProject)
        {
            // todo pøesunout do projectscontrollertests
            var context = GetContext();
            var c = new ProjectController(context);
            var response = c.Search(projectQuery) as ViewResult;
            var data = response?.Model as List<Project>;
            Assert.AreEqual(expectedProject, data?.First().name);
        }
    }
}