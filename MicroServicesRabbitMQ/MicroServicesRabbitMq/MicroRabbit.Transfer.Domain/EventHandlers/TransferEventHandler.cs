using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Domain.EventHandlers;

public class TransferEventHandler : IEventHandler<TransferCreateEvent>
{
    public Task Handle(TransferCreateEvent @event)
    {
        return Task.CompletedTask;
    }
}