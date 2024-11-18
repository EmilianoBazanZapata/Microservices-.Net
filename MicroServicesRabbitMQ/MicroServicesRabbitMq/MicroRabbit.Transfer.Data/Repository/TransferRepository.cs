using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository;

public class TransferRepository(TransferDbContext dbContext) : ITransferRepository
{
    private readonly TransferDbContext _dbContext = dbContext;
    
    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return _dbContext.TransferLogs;
    }

    public void AddTransferLog(TransferLog log) 
    {
        _dbContext.Add(log);
        _dbContext.SaveChanges();
    }
}