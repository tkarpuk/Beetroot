using Beetroot.BLL.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Beetroot.BLL.Interfaces
{
    public interface IMessageService
    {
        Task<Guid> SaveMessageAsync(MessageDto messageDto,CancellationToken cancellationToken);
        List<MessageViewDto> GetMessages(MessageQueryParametersDto queryParametersDto);
    }
}
