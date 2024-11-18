using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Banking.Application.Interfaces;

public interface ITransferService
{
    IEnumerable<TransferLog> GetTransferLogs();
}