using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService(IAccountRepository accountRespository, IEventBus eventBus) : IAccountService
{
    private readonly IAccountRepository _accountRespository = accountRespository;
    private readonly IEventBus _eventBus = eventBus;

    public IEnumerable<Account> GetAccounts()
    {
        return _accountRespository.GetAccouts();
    }

    public void Transfer(AccountTransfer accountTransfer)
    {
        var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount,
                                                               accountTransfer.ToAccount,
                                                               accountTransfer.TransferAmount);

        _eventBus.SendCommand(createTransferCommand);
    }
}