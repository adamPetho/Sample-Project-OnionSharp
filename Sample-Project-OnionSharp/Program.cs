using OnionSharp.Microservices;
using OnionSharp.Tor;
using OnionSharp.Tor.Models;

namespace Sample_Project_OnionSharp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var dataDir = EnvironmentHelpers.GetDataDir("Sample-Project");

            var torSetting = new TorSettings(dataDir,
                distributionFolderPath: EnvironmentHelpers.GetFullBaseDirectory(),
                terminateOnExit: true,
                TorMode.Enabled,
                socksPort: 37155,
                controlPort: 37156);

            TorProcessManager torProcessManager = new TorProcessManager(torSetting);
            await torProcessManager.StartAsync(attempts: 3, CancellationToken.None);

            var onionClientFactory = new OnionHttpClientFactory(torSetting.SocksEndpoint.ToUri("socks5"));

            var blockstreamClient = onionClientFactory.CreateClient("blockstream.info");

            var response = await blockstreamClient.GetAsync("http://explorerzydxu5ecjrkwceayqybizmpjjznk5izmitf2modhcusuqlid.onion/api/mempool/recent");

            Console.WriteLine($"Response Status: {response.StatusCode}");
        }
    }
}
