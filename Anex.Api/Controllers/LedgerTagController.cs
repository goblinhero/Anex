using System;
using Anex.Api.Database;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Anex.Api.Database.Commands;
using Anex.Domain;
using NHibernate.Linq;

namespace Anex.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LedgerTagController : ControllerBase
{
    private readonly ISessionHelper _sessionHelper;

    public LedgerTagController(ISessionHelper sessionHelper)
    {
        _sessionHelper = sessionHelper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetListQuery<LedgerTagDto>());
        return query.Success
            ? Ok(query.Result)
            : NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetQuery<LedgerTagDto>(id));
        return query.Success
            ? Ok(query.Result)
            : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create(LedgerTagDto dto)
    {
        var command = new CreateLedgerTagCommand(dto);
        var commandResult = await _sessionHelper.TryExecuteCommand(command);
        if (!commandResult.Success)
        {
            return BadRequest(commandResult);
        }

        return command.AssignedId.HasValue
            ? await GetById(command.AssignedId.Value)
            : BadRequest("Failed to assign id");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteById(long id)
    {
        var command = new DeleteEntityCommand<LedgerTag>(id);
        var commandResult = await _sessionHelper.TryExecuteCommand(command);
        if (!commandResult.Success)
        {
            return BadRequest(commandResult);
        }

        return Ok($"LedgerTag with id: {id} deleted.");
    }

    [HttpGet("Environment")]
    public Task<IActionResult> GetEnvironmentVariables()
    {
        return Task.FromResult((IActionResult)Ok(_sessionHelper.GetConnectionString()));
    }

    [HttpGet("Testers")]
    public async Task<IActionResult> GetException()
    {
        try
        {
            var query = await _sessionHelper.TryExecuteQuery(new GetListQuery<LedgerTagDto>());
            return query.Success
                ? Ok(query.Result)
                : NotFound();
        }
        catch (Exception ex)
        {
            return Ok(new { ex.Message, ex.StackTrace });
        }
    }
}