using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(CarService.Areas.Identity.IdentityHostingStartup))]
namespace CarService.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}