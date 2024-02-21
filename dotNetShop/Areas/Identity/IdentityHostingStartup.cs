using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(dotNetShop.Areas.Identity.IdentityHostingStartup))]
namespace dotNetShop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}