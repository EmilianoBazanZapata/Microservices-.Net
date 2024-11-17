using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService(IAccountRepository accountRespository) : IAccountService
{
    private readonly IAccountRepository _accountRespository = accountRespository;

    public IEnumerable<Account> GetAccounts()
    {
        return _accountRespository.GetAccouts();
    }
}