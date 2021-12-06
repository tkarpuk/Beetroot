using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.BLL.Services
{
    public interface IUdpReceiveService
    {
        Task ReceiveMessageAsync(CancellationToken stoppingToken);
    }
}