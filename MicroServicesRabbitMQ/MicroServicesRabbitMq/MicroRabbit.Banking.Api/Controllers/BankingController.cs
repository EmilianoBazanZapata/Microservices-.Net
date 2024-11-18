using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BankingController(IAccountService accountService) : ControllerBase
{
    // Constructor

    // Acción para obtener las cuentas
    [HttpGet]
    public ActionResult<IEnumerable<Account>> Get()
    {
        return Ok(accountService.GetAccounts());
    }

    [HttpPost]
    public IActionResult Post([FromBody] AccountTransfer accountTransfer)
    {
        accountService.Transfer(accountTransfer);
        
        return Ok(accountTransfer);
    }
}