using OnionSharp.Microservices;
using OnionSharp.Tor;
using OnionSharp.Tor.Models;

namespace ASP.NET_Core_Sample_Project_OnionSharp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var dataDir = EnvironmentHelpers.GetDataDir("Sample-Project");

            TorSettings torSetting = new TorSettings(dataDir,
                distributionFolderPath: EnvironmentHelpers.GetFullBaseDirectory(),
                terminateOnExit: true,
                TorMode.Enabled,
                socksPort: 37155,
                controlPort: 37156);

            builder.Services.AddSingleton(torSetting);

            builder.Services.AddSingleton<IHttpClientFactory>(services => new OnionHttpClientFactory(torSetting.SocksEndpoint.ToUri("socks5")));

            builder.Services.AddHostedService<TorProcessManagerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
