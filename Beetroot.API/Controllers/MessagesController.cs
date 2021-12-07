using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<MessagesController> _logger;
        public MessagesController(IMessageService messageService, ILogger<MessagesController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessageViewDto>>> Get(string ipAddress, 
            DateTime dateStart, DateTime dateEnd,
            int pageNumber, int pageSize)
        {
            _logger.LogDebug($"Request with parameterstring: {Request.QueryString}");

            var messageQueryParametersDto = new MessageQueryParametersDto(ipAddress,
                dateStart, dateEnd,  pageNumber, pageSize);

            var list = await Task.Run(() => _messageService.GetMessages(messageQueryParametersDto));
            return Ok(list);
        }
    }
}
