using Beetroot.BLL.Configurations;
using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.BLL.Services
{
    public class UdpReceiveService : IUdpReceiveService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UdpReceiveService> _logger;
        private readonly UdpConfiguration _configuration;

        public UdpReceiveService(IServiceScopeFactory serviceScopeFactory, IOptions<UdpConfiguration> udpConfiguration, ILogger<UdpReceiveService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _configuration = udpConfiguration.Value;

            _logger.LogInformation($"Created UdpReceiveService. Port: {_configuration.PortUdp}");
        }

        private bool IsNotProperlyMessage(string message)
        {
            return !((message.Length > _configuration.SecretKey.Length) && message.Contains(_configuration.SecretKey));
        }

        private static MessageDto CreateMessageDto(string text, string address)
        {
            return new MessageDto()
            {
                IpAddress = address,
                Text = text,
                Date = DateTime.UtcNow
            };
        }

        private string ClearMessageText(string message)
        {
            return message.Replace(_configuration.SecretKey, "");
        }

        private async Task SaveMessageAsync(MessageDto messageDto, CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
            Guid messageId = await messageService.SaveMessageAsync(messageDto, cancellationToken);

            _logger.LogDebug($"ID saved message in DB: {messageId}");
        }

        public async Task ReceiveMessageAsync(CancellationToken stoppingToken)
        {
            using var receiver = new UdpClient(_configuration.PortUdp);
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var udpReceiveResult = await receiver.ReceiveAsync();
                    _logger.LogDebug($"Get message from IP: {udpReceiveResult.RemoteEndPoint.Address}");

                    string messageText = Encoding.Unicode.GetString(udpReceiveResult.Buffer);
                    if (IsNotProperlyMessage(messageText))
                    {
                        _logger.LogError($"Message is empty or doesn't contain Secret Key");
                        continue;
                    }

                    var messageDto = CreateMessageDto(
                        ClearMessageText(messageText),
                        udpReceiveResult.RemoteEndPoint.Address.ToString());

                    await SaveMessageAsync(messageDto, stoppingToken);
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
