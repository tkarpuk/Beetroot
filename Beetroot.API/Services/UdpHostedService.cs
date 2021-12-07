using Beetroot.BLL.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.API.Services
{
    public class UdpHostedService : BackgroundService
    {
        private readonly ILogger<UdpHostedService> _logger;
        private readonly IUdpReceiveService _receiveService;

        public UdpHostedService(IUdpReceiveService receiveService, ILogger<UdpHostedService> logger)
        {
            _logger = logger;
            _receiveService = receiveService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _receiveService.ReceiveMessageAsync(stoppingToken);
        }
    }
}
