using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.API.Services
{
    public class UdpHostedService : BackgroundService
    {
        private int _portUdp;
        private string _secretKey;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UdpHostedService> _logger;

        public UdpHostedService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<UdpHostedService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _secretKey = configuration["UdpConfiguration:SecretKey"];
            if (!int.TryParse(configuration["UdpConfiguration:Port"], out _portUdp))
                _portUdp = 8001;

            _logger.LogInformation($"Started service for UDP. Port: {_portUdp}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await ReceiveMessage(stoppingToken);
        }

        private bool IsNotProperlyMessage(string message)
        {
            return (string.IsNullOrEmpty(message) || !message.Contains(_secretKey));
        }

        private MessageDto CreateMessageDto(string text, IPAddress address)
        {
            return new MessageDto()
            {
                IpAddress = address,
                TextMessage = text,
                DateMessage = DateTime.Now
            };
        }

        private string ClearMessageText(string message)
        {
            return message.Replace(_secretKey, "");
        }

        private async Task SaveMessage(MessageDto messageDto, CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                Guid messageId = await messageService.SaveMessage(messageDto, cancellationToken);

                _logger.LogDebug($"ID saved message in DB: {messageId}");
            }
        }

        private async Task ReceiveMessage(CancellationToken stoppingToken)
        {
            UdpClient receiver = new UdpClient(_portUdp); 
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var udpReceiveResult = await receiver.ReceiveAsync();
                    _logger.LogDebug($"Get message from IP: {udpReceiveResult.RemoteEndPoint.Address}");

                    string messageText = Encoding.Unicode.GetString(udpReceiveResult.Buffer);
                    if (IsNotProperlyMessage(messageText))
                    {
                        _logger.LogError($"Message empty or doesn't contain Secret Key");
                        continue;
                    }                      

                    var messageDto = CreateMessageDto(
                        ClearMessageText(messageText), 
                        udpReceiveResult.RemoteEndPoint.Address);

                    await SaveMessage(messageDto, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in ReceiveMessage: {ex.Message}");
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
