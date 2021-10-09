using Models.Events;
using System;
using System.Collections.Generic;
using System.Text;
using TradingCore.Abstractions;

namespace TradingCore
{
    public class TradingEventsHandler : ITradingEventsHandler
    {
        public void On(BaseEvent @event)
        {
            if(@event.GetType() == typeof(UpWardIntersectionEvent))
                Console.WriteLine($"event: {@event.GetType()}");
        }
    }
}
