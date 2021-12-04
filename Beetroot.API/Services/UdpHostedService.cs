using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.API.Services
{
    public class UdpHostedService : BackgroundService
    {
        private int _portUdp;
        private readonly IMessageService _messageService;

        public UdpHostedService(IMessageService messageService, int portUdp)
        {
            _messageService = messageService;
            _portUdp = 8001;//portUdp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ReceiveMessage();
            }
        }

        private async Task ReceiveMessage()
        {
            UdpClient receiver = new UdpClient(_portUdp); 
            try
            {
                while (true)
                {
                    var udpReceiveResult = await receiver.ReceiveAsync();

                    string messageText = Encoding.Unicode.GetString(udpReceiveResult.Buffer);
                    // check sequrity

                    var message = new MessageDto()
                    {
                        IpAddress = udpReceiveResult.RemoteEndPoint.Address,
                        TextMessage = messageText,
                        DateMessage = DateTime.Now
                    };

                    Guid messageId = await _messageService.SaveMessage(message, CancellationToken.None);
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
