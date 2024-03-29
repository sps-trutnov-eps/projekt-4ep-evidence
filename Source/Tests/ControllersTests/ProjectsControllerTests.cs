using EvidenceProject.Controllers;
using EvidenceProject.Controllers.ActionData;
using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NUnit.Framework.Internal;

#nullable disable
namespace Tests.ControllersTests;
public class ProjectControllerTests : ControllerTestsBase
{
    ProjectContext DBContext { get; set; }

    ProjectController Controller { get; set;}

    MemoryCacheOptions Opt = new();

    Logger<ProjectController> Logger = new(ControllerTestsBase.LoggerFactory);

    public ProjectControllerTests()
    {
        MemoryCache memoryCache = new(Opt);
        DBContext = GetContext();
        Controller = new ProjectController(DBContext, memoryCache, Logger);
    }

    [Test]
    [TestCase("test", "test")]
    [TestCase("est", "test")]
    [TestCase("st", "test")]
    public void SearchProject(string projectQuery, string expectedProject)
    {
        SearchData searchData = new()
        {
            text = projectQuery,
        };
        var response = (JsonResult)Controller.Search(searchData);
        var data = (List<Project>)response.Value;
        Assert.That(data?.First().name, Is.EqualTo(expectedProject));
    }

    [Test]
    public void ProjectInfo()
    {
        var response = (ViewResult)Controller.Project(1);
        var data = (Project)response.Model;
        Assert.That(data, !Is.EqualTo(null));
    }

    [Test]
    public void Delete()
    {
        var response = Controller.Delete(1);
        var message = response.Value;
        Assert.That(message, Is.EqualTo("ok"));
    }

    [Test]
    public void CreateProject()
    {
        string name = $"Test{Guid.NewGuid().ToString("N")}"[0..25];
        ProjectCreateData data = new()
        {
            projectName = name,
        };
        var response = (ViewResult)Controller.Create(data, true);
        Assert.That(response.ViewName, Is.EqualTo("Index"));
    }

}
#nullable enable