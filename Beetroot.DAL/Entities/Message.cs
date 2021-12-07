using System;

namespace Beetroot.DAL.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Guid AddressId { get; set; }
        public Address IpAddress { get; set; }
    }
}
