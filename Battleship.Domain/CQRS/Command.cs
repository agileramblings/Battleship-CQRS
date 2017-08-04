using System;

namespace Battleship.Domain.CQRS
{
    public class Command : IMessage
    {
        public DateTime ReceivedOn { get; set; } = DateTime.UtcNow;
        public string Ip { get; set; } = "127.0.0.1";
        public string RequestedBy { get; set; } = "Localhost";
        public string RequestUrl { get; set; } = "http://localhost";
        public string RequestBody { get; set; } = string.Empty;
    }
}