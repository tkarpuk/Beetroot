using System;
using System.Net;

namespace Beetroot.BLL.Dto
{
    public class MessageDto
    {
        // GUiD ???
        public IPAddress IpAddress { get; set; }
        public string TextMessage { get; set; }
        public DateTime DateMessage { get; set; }
    }
}
