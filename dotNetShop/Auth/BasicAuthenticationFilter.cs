using System;
using System.Net;
using System.Text;
using dotNetShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotNetShop.Auth
{    
    public class BasicAuthenticationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string authorizationHeader = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var encodedUsernamePassword = authorizationHeader.Substring("Basic ".Length).Trim();
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            var username = usernamePassword.Split(':')[0];
            var password = usernamePassword.Split(':')[1];

            if (!IsValidUser(username, password))
            {
                context.Result = new UnauthorizedResult();
            }
        }

        private bool IsValidUser(string username, string password)
        {
            return username == Settings.AdminCreds.Username && password == Settings.AdminCreds.Password;
        }
    }

}
