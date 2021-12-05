using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Beetroot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        private MessageQueryParametersDto CreateParametersDto(
            string ipAddress,
             DateTime dateStart,
             DateTime dateEnd,
             int pageN,
             int pageSize)
        {
            MessageQueryParametersDto ParametersDto = new MessageQueryParametersDto()
            {
                IpAddress = (string.IsNullOrEmpty(ipAddress)) ? null : ipAddress,

                DateStart = (dateStart == DateTime.MinValue) ? null : dateStart.ToUniversalTime(),
                DateEnd = (dateEnd == DateTime.MinValue) ? null : dateEnd.ToUniversalTime(),
                PageN = (pageN == 0) ? 1 : pageN,
                PageSize = (pageSize == 0) ? 10 : pageSize
            };

            return ParametersDto;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageViewDto>>> Get(string ipAddress, 
            DateTime dateStart, DateTime dateEnd,
            int pageN, int pageSize)
        {
            var messageQueryParametersDto = CreateParametersDto(ipAddress,
                dateStart, dateEnd,  pageN, pageSize);

            var list = await Task.Run(() => _messageService.GetMessages(messageQueryParametersDto));
            return Ok(list);
        }
    }
}
