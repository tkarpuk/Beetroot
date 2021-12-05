using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        private IServiceScopeFactory _serviceScopeFactory;

        public UdpHostedService(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration)
        {
            _serviceScopeFactory = serviceScopeFactory;            
            _secretKey = configuration["UdpConfiguration:SecretKey"];
            if (!int.TryParse(configuration["UdpConfiguration:Port"], out _portUdp))
                _portUdp = 8001;
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
                // log!
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

                    string messageText = Encoding.Unicode.GetString(udpReceiveResult.Buffer);
                    if (IsNotProperlyMessage(messageText)) 
                        continue;

                    var messageDto = CreateMessageDto(
                        ClearMessageText(messageText), 
                        udpReceiveResult.RemoteEndPoint.Address);

                    await SaveMessage(messageDto, stoppingToken);
                    /*
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IMessageService messageService = scope.ServiceProvider.GetRequiredService<IMessageService>();
                        Guid messageId = await messageService.SaveMessage(messageDto, CancellationToken.None);
                        // log!
                    }
                    */
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                // Log!
            }
            finally
            {
                receiver.Close();
            }
        }
    }
}
