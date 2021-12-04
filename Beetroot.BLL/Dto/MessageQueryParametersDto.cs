using System;
using System.Net;

namespace Beetroot.BLL.Dto
{
    public class MessageQueryParametersDto
    {
        public IPAddress IpAddress { get; set; }        
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int PageN { get; set; }
        public int PageSize { get; set; }
    }
}
