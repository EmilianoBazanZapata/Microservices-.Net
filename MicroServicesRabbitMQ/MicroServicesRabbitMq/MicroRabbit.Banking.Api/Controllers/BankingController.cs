using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankingController : ControllerBase
{
    private readonly IAccountService _accountService;

    // Constructor
    public BankingController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    // Acción para obtener las cuentas
    [HttpGet]
    public ActionResult<IEnumerable<Account>> Get()
    {
        return Ok(_accountService.GetAccounts());
    }
}