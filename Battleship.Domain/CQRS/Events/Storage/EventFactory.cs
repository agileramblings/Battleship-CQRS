using System;
using Newtonsoft.Json;

namespace Battleship.Domain.CQRS.Events.Storage
{
    public class EventFactory
    {
        public static Event GetConcreteEvent(EventDescriptorEntity ede)
        {
            var t = Type.GetType(ede.EventType);
            return JsonConvert.DeserializeObject(ede.EventData, t) as Event;
        }
    }
}