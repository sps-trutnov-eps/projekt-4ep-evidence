namespace EvidenceProject.Helpers;
public class UniversalHelper
{
    /// <summary>
    /// Zjistíme, zda je přihlášen uživatel
    /// </summary>
    public static bool getLoggedUser(HttpContext context, out string? userID)
    {
        userID = context.Session.GetString(LoggedInKey);
        return userID != null;
    }

    /// <summary>
    /// Jméno cookie
    /// </summary>
    public static string LoggedInKey => "loggedin";
}
