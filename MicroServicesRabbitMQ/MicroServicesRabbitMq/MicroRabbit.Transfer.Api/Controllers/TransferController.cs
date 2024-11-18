using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransferController(ITransferService transferService) : ControllerBase
{
    private readonly ITransferService _transferService = transferService;

    [HttpGet]
    public ActionResult<IEnumerable<TransferLog>> Get()
    {
        return Ok(_transferService.GetTransferLogs());
    }
}