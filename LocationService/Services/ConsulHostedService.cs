using Consul;
using System.Net;

namespace LocationService.Services
{
    public class ConsulHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly string _registrationId;

        public ConsulHostedService(IConsulClient consulClient, IHostApplicationLifetime hostApplicationLifetime)
        {
            _consulClient = consulClient;
            _hostApplicationLifetime = hostApplicationLifetime;
            _registrationId = $"{Dns.GetHostName()}-{Guid.NewGuid()}";
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var registration = new AgentServiceRegistration
            {
                ID = _registrationId,
                Name = "LocationService",
                Address = "localhost",
                Port = 5001
            };

            await _consulClient.Agent.ServiceRegister(registration, cancellationToken);

            _hostApplicationLifetime.ApplicationStopping.Register(async () =>
            {
                await _consulClient.Agent.ServiceDeregister(_registrationId);
            });
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
