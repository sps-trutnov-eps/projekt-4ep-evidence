using EvidenceProject.Controllers;
using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Data.DataModels;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Internal;

#nullable disable
namespace Tests.ControllersTests;
public class ProjectControllerTests : ControllerTestsBase
{
    ProjectContext DBContext { get; set; }

    ProjectController Controller { get; set;}

    public ProjectControllerTests()
    {
        DBContext = GetContext();
        Controller = new ProjectController(DBContext);
    }

    [Test]
    [TestCase("test", "test")]
    [TestCase("est", "test")]
    [TestCase("st", "test")]
    public void SearchProject(string projectQuery, string expectedProject)
    {
        var response = (ViewResult)Controller.Search(projectQuery);
        var data = (List<Project>)response.Model;
        Assert.That(data?.First().name, Is.EqualTo(expectedProject));
    }

    [Test]
    public void ProjectInfo()
    {
        var response = (ViewResult)Controller.ProjectInfo(1);
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
        string name = $"Test{Guid.NewGuid().ToString("N")}";
        ProjectCreateData data = new()
        {
            projectName = name,
            projectState = "State",
            projectType = "Type",
            technology = "Technology"
        };
        var response = (ViewResult)Controller.Create(data);
        Assert.That(response.ViewName, Is.EqualTo("Index"));
    }

}
#nullable enable