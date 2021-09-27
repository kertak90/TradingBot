using Models.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace TradingCore
{
    public class TradingEventsHandler : ITradingEventsHandler
    {
        public void On(BaseEvent @event)
        {
            Console.WriteLine($"event: {@event.EventType}");
        }
    }
}
