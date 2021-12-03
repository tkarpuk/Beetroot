using System;

namespace Beetroot.DAL.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string TextMessage { get; set; }
        public DateTime DateMessage { get; set; }

        public Guid AddressId { get; set; }
        public Address IpAddress { get; set; }
    }
}
