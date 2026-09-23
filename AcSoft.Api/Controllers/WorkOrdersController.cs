using AcSoft.Application.Features.WorkOrders.DTOs;
using AcSoft.Application.Features.WorkOrders.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace AcSoft.Api.Controllers;

[ApiController]
[Route("api/work-orders")]
public sealed class WorkOrdersController : ControllerBase
{
    private readonly CreateWorkOrderHandler _handler;

    public WorkOrdersController(
    CreateWorkOrderHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWorkOrderRequest request,
        CancellationToken cancellationToken)
    {

        var id = await _handler.HandleAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // ...
        return Ok();
    }
}