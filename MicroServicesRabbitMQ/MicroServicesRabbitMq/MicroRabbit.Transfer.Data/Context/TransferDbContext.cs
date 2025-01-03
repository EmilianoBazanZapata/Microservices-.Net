using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Data.Context;

public class TransferDbContext(DbContextOptions<TransferDbContext> options) : DbContext(options)
{
    public DbSet<TransferLog> TransferLogs { get; set; }
}