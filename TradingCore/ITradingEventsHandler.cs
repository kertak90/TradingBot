using Models.Events;

namespace TradingCore
{
    public interface ITradingEventsHandler
    {
        void On(BaseEvent @event);
    }
}