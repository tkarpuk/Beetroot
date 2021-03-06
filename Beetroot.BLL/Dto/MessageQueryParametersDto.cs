using System;

namespace Beetroot.BLL.Dto
{
    public class MessageQueryParametersDto
    {  
        public string IpAddress { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public MessageQueryParametersDto(
            string ipAddress,
            DateTime dateStart,
            DateTime dateEnd,
            int pageNumber,
            int pageSize)
        {
            this.IpAddress = (string.IsNullOrEmpty(ipAddress)) ? null : ipAddress;
            this.DateStart = (dateStart == DateTime.MinValue) ? null : dateStart.ToUniversalTime();
            this.DateEnd = (dateEnd == DateTime.MinValue) ? null : dateEnd.ToUniversalTime();
            this.PageNumber = (pageNumber == 0) ? 1 : pageNumber;
            this.PageSize = (pageSize == 0) ? 10 : pageSize;
        }
    }
}
