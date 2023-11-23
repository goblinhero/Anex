using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Anex.Api.Database;
using Anex.Api.Database.Commands;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anex.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FiscalPeriodController : ControllerBase
{
    private readonly ISessionHelper _sessionHelper;

    public FiscalPeriodController(ISessionHelper sessionHelper)
    {
        _sessionHelper = sessionHelper;
    }
    
    [HttpGet]
    [ProducesResponseType<IList<FiscalPeriodDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetListQuery<FiscalPeriodDto>());
        return query.Success
            ? Ok(query.Result)
            : BadRequest($"An error occured. Details: {string.Join(Environment.NewLine,query.Errors)}");
    }

    [HttpGet("{id}")]
    [ProducesResponseType<FiscalPeriodDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetQuery<FiscalPeriodDto>(id));
        return query.Success
            ? Ok(query.Result)
            : NotFound();
    }
    
    [HttpPost]
    [ProducesResponseType<FiscalPeriodDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableFiscalPeriodDto dto)
    {
        var command = new CreateFiscalPeriodCommand(dto);
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
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(new DeleteEntityCommand<FiscalPeriod>(id));
        if (!commandResult.Success)
        {
            return BadRequest(commandResult);
        }

        return Ok($"{nameof(FiscalPeriod)} with id: {id} deleted.");
    }
}