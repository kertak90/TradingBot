using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Events
{
    public class BaseEvent
    {
        public int MyProperty { get; set; }
        public TradingEvents EventType { get; set; }
    }
}
