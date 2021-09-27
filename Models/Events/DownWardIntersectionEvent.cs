using Models.TinkoffOpenApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Events
{
    public class DownWardIntersectionEvent : BaseEvent
    {
        public Candle EventDayCandle { get; set; }
    }
}
