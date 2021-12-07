using System;
using System.Collections.Generic;
using System.Net;

namespace Beetroot.DAL.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}
