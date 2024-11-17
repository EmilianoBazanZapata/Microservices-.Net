using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data.Repository;

public class AccountRepository(BankingDbContext context) : IAccountRepository
{
    private readonly BankingDbContext _context = context;

    public IEnumerable<Account> GetAccouts()
    {
        return _context.Accounts;
    }
}