using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace WebApplication1.Helpers
{
    public class Methods
    {
        public static string GetUsernameFromCookie(HttpContextBase contextBase)
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = contextBase.Request.Cookies[cookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            string Username = ticket.Name;
            return Username;
        }
    }
}