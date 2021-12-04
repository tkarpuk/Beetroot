using Beetroot.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<Guid> SaveMessage(MessageDto messageDto,CancellationToken cancellationToken);
        Task<List<MessageDto>> GetMessages(MessageQueryParametersDto queryParametersDto, CancellationToken cancellationToken);
    }
}
