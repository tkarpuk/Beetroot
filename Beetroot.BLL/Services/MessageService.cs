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

        private static Func<Message, bool> CreateAddressCondition(MessageQueryParametersDto queryParametersDto)
        {
            if (queryParametersDto.IpAddress == null)
                return m => true;

            return m => (m.IpAddress.IpAddress == queryParametersDto.IpAddress); 
        }

        private static Func<Message, bool> CreateMessageCondition(MessageQueryParametersDto queryParametersDto)
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

            Func<Message, bool> AddressCondition = CreateAddressCondition(queryParametersDto);
            Func<Message, bool> MessageCondition = CreateMessageCondition(queryParametersDto);

            var listResult = _dbContext.Messages.Include(m => m.IpAddress)
                .Where(AddressCondition)
                .Where(MessageCondition)
                .Select(m => new MessageViewDto()
                {
                    IpAddress = m.IpAddress.IpAddress.ToString(),
                    Date = m.Date,
                    Text = m.Text
                })
                .Take(_limit)
                .Skip(_offset);

            return listResult.ToList();
        }

        private async Task<Address> GetAddressAsync(MessageDto messageDto, CancellationToken cancellationToken)
        {
            Address address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.IpAddress == messageDto.IpAddress, cancellationToken);
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
            Address address = await GetAddressAsync(messageDto, cancellationToken);

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
