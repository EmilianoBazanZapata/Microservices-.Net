using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventsHandlers;

public class TransferEventHandler(ITransferRepository transferRepository) : IEventHandler<TransferCreateEvent>
{
    private readonly ITransferRepository _transferRepository = transferRepository;

    public Task Handle(TransferCreateEvent @event)
    {
        var newLog = new TransferLog
        {
            FromAccount = @event.From,
            ToAccount = @event.To,
            TransferAmount = @event.Amount
        };
        
        _transferRepository.AddTransferLog(newLog);
        
        return Task.CompletedTask;
    }
}