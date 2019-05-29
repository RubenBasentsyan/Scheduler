using System.Web;
using System.Web.Security;

namespace WebApplication1.Helpers
{
    public class Methods
    {
        public static string GetUsernameFromCookie(HttpContextBase contextBase)
        {
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = contextBase.Request.Cookies[cookieName];
            var ticket = FormsAuthentication.Decrypt(authCookie.Value);
            var username = ticket.Name;
            return username;
        }
    }
}