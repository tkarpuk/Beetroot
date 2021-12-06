using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.BLL.Services
{
    public interface IUdpReceiveService
    {
        int PortUdp { get; }
        Task ReceiveMessageAsync(CancellationToken stoppingToken);
    }
}