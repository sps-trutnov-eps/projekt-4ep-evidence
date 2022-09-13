namespace EvidenceProject.Helpers
{
    public class UniversalHelper
    {
        private static string sessionKeyValue = "true";
        public static bool IsLogged(HttpContext context)
        {
            string? loggedInString = context.Session.GetString("loggedin");
            bool isLogged = sessionKeyValue.Equals(loggedInString);
            return isLogged;
        }
    }
}
