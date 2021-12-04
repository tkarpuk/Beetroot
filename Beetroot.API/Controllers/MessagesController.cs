using Beetroot.BLL.Dto;
using Beetroot.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        [HttpGet]
        public async Task<ActionResult<List<MessageDto>>> Get([FromQuery] MessageQueryParametersDto messageQueryParametersDto)
        {
            var list = await _messageService.GetMessages(messageQueryParametersDto, CancellationToken.None);
            return Ok(list);
        }
    }
}
