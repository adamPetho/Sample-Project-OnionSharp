# Sample Projects for OnionSharp Nuget Package

Simple console application and a .NET server to showcase how to:
- Create TorSettings
- Create and Start TorProcessManager
- Create OnionHttpClientFactory
- Create and use HttpClient from OnionHttpClientFactory

## Nuget package
https://github.com/adamPetho/OnionSharp

# Examples
See below or check the projects in this repository.
## Client side
```cs
// You can use the EnvironmentHelpers to grab the data dir or insert your own path/of/data/directory. 
var dataDir = EnvironmentHelpers.GetDataDir("Sample-Project");

// Create your Tor Settings
var torSetting = new TorSettings(dataDir,
			distributionFolderPath: EnvironmentHelpers.GetFullBaseDirectory(),
			terminateOnExit: true,
			TorMode.Enabled,
			socksPort: 37155,
			controlPort: 37156);

// Create and start TorProcessManager which will start tor.exe
TorProcessManager torProcessManager = new TorProcessManager(torSetting);
await torProcessManager.StartAsync(attempts: 3, CancellationToken.None);

// Create the OnionHttpFactory
var onionClientFactory = new OnionHttpClientFactory(torSetting.SocksEndpoint.ToUri("socks5"));

// Create Http Clients 
var myHttpClient = onionClientFactory.CreateClient("name-of-your-client");

// Send requests to onion address (for example)
await myHttpClient.GetAsync("http://explorerzydxu5ecjrkwceayqybizmpjjznk5izmitf2modhcusuqlid.onion/api/mempool/recent");
```
## Server side
```cs
// You can use the EnvironmentHelpers to grab the data dir or insert your own path/of/data/directory. 
var dataDir = EnvironmentHelpers.GetDataDir("Sample-Project-Backend");

// Create your Tor Settings
TorSettings torSetting = new TorSettings(dataDir,
    distributionFolderPath: EnvironmentHelpers.GetFullBaseDirectory(),
    terminateOnExit: true,
    TorMode.Enabled,
    socksPort: 37155,
    controlPort: 37156);

builder.Services.AddSingleton(torSetting);

// Create the OnionHttpFactory and register as a service, so .NET can DI it anywhere you want to use it.
builder.Services.AddSingleton<IHttpClientFactory>(services => new OnionHttpClientFactory(torSetting.SocksEndpoint.ToUri("socks5")));

// Important! Add TorProcessManagerService as HostedService. This class will launch and handle your tor.exe on the server side.
builder.Services.AddHostedService<TorProcessManagerService>();
```