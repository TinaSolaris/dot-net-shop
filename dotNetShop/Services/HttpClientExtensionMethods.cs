using dotNetShop.Data;
using System;
using System.Net.Http;
using System.Text;

namespace dotNetShop.Services
{
    public static class HttpClientExtensionMethods
    {
        public static void AppendRequestDefaults(this HttpClient client)
        {
            client.BaseAddress = new Uri(Settings.WebApi.BaseApiUrl);

            var authenticationString = $"{Settings.AdminCreds.Username}:{Settings.AdminCreds.Password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(authenticationString));
            client.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
        }
    }
}
