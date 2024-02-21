namespace dotNetShop.Data
{
    public class Settings
    {
        public static class PolicyName
        {
            public const string Admin = "Admin";
            public const string NonAdmin = "NonAdmin";
            public const string AuthNonAdmin = "AuthNonAdmin";
        }

        public static class AdminCreds
        {
            public const string Username = "adminuser@localhost";
            public const string Password = "aUpass1!";
        }

        public static class WebApi
        {
            public const string BaseApiUrl = "https://localhost:44389/api/";
            public const string ArticlePath = "article/";
            public const string CategoryPath = "category/";
        }
    }
}
