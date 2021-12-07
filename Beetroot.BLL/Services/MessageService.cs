using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Beetroot.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Beetroot.DAL.Entities;
using System.Threading;

namespace Beetroot.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAppDbContext _dbContext;
        public MessageService(IAppDbContext dbContext)
        { 
            _dbContext = dbContext; 
        }

        private Func<Address, bool> GetAddressCondition(MessageQueryParametersDto queryParametersDto)
        {
            if (queryParametersDto.IpAddress == null)
                return a => true;

            return a => (a.IpAddress.ToString() == queryParametersDto.IpAddress); 
        }

        private Func<Message, bool> GetMessageCondition(MessageQueryParametersDto queryParametersDto)
        {
            if (queryParametersDto.DateStart != null && queryParametersDto.DateEnd != null)
                return m => m.Date >= queryParametersDto.DateStart && m.Date <= queryParametersDto.DateEnd;

            if (queryParametersDto.DateStart != null && queryParametersDto.DateEnd == null)
                return m => m.Date >= queryParametersDto.DateStart;

            if (queryParametersDto.DateStart == null && queryParametersDto.DateEnd != null)
                return m => m.Date <= queryParametersDto.DateEnd;

            return m => true;
        }

        public List<MessageViewDto> GetMessages(MessageQueryParametersDto queryParametersDto)
        {
            int _limit = queryParametersDto.PageSize;
            int _offset = queryParametersDto.PageSize * (queryParametersDto.PageNumber - 1);

            Func<Address, bool> AddressCondition = GetAddressCondition(queryParametersDto);
            Func<Message, bool> MessageCondition = GetMessageCondition(queryParametersDto);

            var listResultAddress = _dbContext.Addresses.Where(AddressCondition).ToList();
            var listResultMessages = _dbContext.Messages.Where(MessageCondition).ToList();
            
            var listResult = listResultAddress
                .Join(listResultMessages,
                a => a.Id,
                           m => m.AddressId,
                           (a, m) =>
                            new MessageViewDto()
                            {
                                IpAddress = a.IpAddress.ToString(),
                                Date = m.Date,
                                Text = m.Text
                            }
                )
                .Take(_limit)
                .Skip(_offset);
            
            return listResult.ToList();
        }

        private async Task<Address> GetAddress(MessageDto messageDto, CancellationToken cancellationToken)
        {
            Address address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.IpAddress == messageDto.IpAddress);
            if (address == null)
            {
                address = new Address()
                {
                    Id = Guid.NewGuid(),
                    IpAddress = messageDto.IpAddress
                };

                _dbContext.Addresses.Add(address);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return address;
        }

        public async Task<Guid> SaveMessageAsync(MessageDto messageDto, CancellationToken cancellationToken)
        {
            Address address = await GetAddress(messageDto, cancellationToken);

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Text = messageDto.Text,
                Date = messageDto.Date,
                IpAddress = address
            };
           
            _dbContext.Messages.Add(message);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return message.Id;
        }
    }
}
