

using OnionSharp.Tor;

namespace ASP.NET_Core_Sample_Project_OnionSharp
{
    public class TorProcessManagerService : IHostedService
    {
        private readonly TorProcessManager _torManager;
        public TorProcessManagerService(TorSettings torSettings)
        {
            _torManager = new(torSettings);
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _torManager.StartAsync(attempts: 3, cancellationToken);

            Console.WriteLine($"Tor running: {_torManager.IsTorRunningAsync}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _torManager.DisposeAsync().ConfigureAwait(false);
        }
    }
}
