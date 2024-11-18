using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Banking.Application.Services;

public class TransferService(ITransferRepository repository, IEventBus bus) : ITransferService
{
    private readonly ITransferRepository _repository = repository;
    private readonly IEventBus _bus = bus;
    
    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return _repository.GetTransferLogs();
    }
}