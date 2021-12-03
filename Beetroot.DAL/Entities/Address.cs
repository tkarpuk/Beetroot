using System;
using System.Collections.Generic;
using System.Net;

namespace Beetroot.DAL.Entities
{
    public class Address
    {
        public Guid Id { get; set; }
        public IPAddress IpAddress { get; set; }

        public List<Message> Messages { get; set; }
    }
}
