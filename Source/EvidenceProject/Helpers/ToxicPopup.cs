namespace EvidenceProject.Helpers;
public class ToxicPopup
{
    public static string RenderError(string message, out string css)
    {
        css = ".popup{\r\n   background-color: var(--primary);\r\n    padding: 25px 10px 0px 10px;\r\n   width: 420px;position: absolute;transform: translate(-50%,-50%);left: 50%;top: 50%;border-radius: 8px;\r\n    font-family: \"Poppins\",sans-serif;\r\n    text-align: center;\r\n}.popup button{ font-size: 30px;\r\n    color: #ffffff;\r\n    width: 40px;\r\n    height: 40px;\r\n    border: none;\r\n    outline: none;\r\n    cursor: pointer;\r\n    margin: 0;\r\n    position: absolute;\r\n    right: 0;\r\n    top: 0;\r\n    background: none;}\r\n .popup h2 {\r\n    text-transform: inherit !important;\r\n    border: none !important;\r\n      height: 80px;\r\n    text-align: center;\r\n    color: white;\r\n    font-weight: bold;    padding: 1rem;\r\n    color: white;\r\n    font-size: 26px;}";
        var html = "<div class=\"popup\"><h2>"+message+"</h2><button id=\"close\"></button></div>";
        return html; 
    }
}
