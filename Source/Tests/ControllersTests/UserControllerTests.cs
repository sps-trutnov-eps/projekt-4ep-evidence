using EvidenceProject.Controllers;
using EvidenceProject.Controllers.RequestClasses;
using EvidenceProject.Data;
using EvidenceProject.Helpers;
using Microsoft.AspNetCore.Mvc;
#nullable disable

namespace Tests.ControllersTests;
public class UserControllerTests : ControllerTestsBase
{
    ProjectContext DBContext { get; set; }

    UserController Controller { get; set; }
    public UserControllerTests()
    {
        DBContext = GetContext();
        Controller = new UserController(DBContext);
    }

    [TestCase("guid","12345", true)]
    [TestCase("test","12345", false)]
    [TestCase(null,null, false)]
    public void Register(string username, string password, bool successful)
    {
        var data = new LoginData()
        {
            username = username == "guid" ? Guid.NewGuid().ToString("N") : username,
            password = password
        };

        if (successful)
        {
            var RedirectResult = (RedirectResult)Controller.RegisterPost(data);
            var viewName = RedirectResult.Url;
            Assert.That(viewName, Is.EqualTo("/users/login"));
            return;
        }
        var JsonResult = (JsonResult)Controller.RegisterPost(data);
        if(JsonResult.Value.ToString() == UniversalHelper.SomethingWentWrongMessage)
        {
            Assert.That(JsonResult.Value, Is.EqualTo(UniversalHelper.SomethingWentWrongMessage));
            return;
        }
        Assert.That(JsonResult.Value, Is.EqualTo("Uživatel již existuje"));

    }


    [TestCase("test", "12345", true)]
    [TestCase("test", "123456", false)]
    [TestCase(null, null, false)]
    public void Login(string username, string password, bool successful)
    {

        var loginData = new LoginData()
        {
            username = username,
            password = password
        };

        var data = Controller.LoginPost(loginData, true);
        if (successful)
        {
            var RedirectResult = (RedirectResult)data;
            Assert.That(RedirectResult.Url, Is.EqualTo("/"));
            return;           
        }
        var JsonResult = (JsonResult)data;
        Assert.That(JsonResult.Value, Is.EqualTo(UniversalHelper.SomethingWentWrongMessage));
    }
}
#nullable enable