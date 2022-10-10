namespace EvidenceProject.Helpers;
public class UniversalHelper
{

    /// <summary>
    /// Jméno cookie
    /// </summary>
    public static string LoggedInKey => "loggedin";

    /// <summary>
    /// Json hláška při chybě
    /// </summary>
    public static string SomethingWentWrongMessage => "Něco se pokazilo";


    /// <summary>
    /// Zjistíme, zda je přihlášen uživatel
    /// </summary>
    public static bool getLoggedUser(HttpContext context, out string? userID)
    {
        userID = context.Session.GetString(LoggedInKey);
        return userID != null;
    }

    /// <summary>
    /// Vyhledání projektu dle ID
    /// </summary>
    public static bool getProject(ProjectContext context, int id, out Project project)
    {
        project = context?.projects?.ToList().FirstOrDefault(project => project.id == id);
        return project != null;
    }
}
