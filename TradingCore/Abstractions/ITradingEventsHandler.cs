using Models.Events;

namespace TradingCore.Abstractions
{
    public interface ITradingEventsHandler
    {
        void On(BaseEvent @event);
    }
}