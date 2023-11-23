using System;
using System.Collections.Generic;
using System.Text.Json;
using Anex.Api.Database;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Anex.Api.Database.Commands;
using Anex.Domain;
using Microsoft.AspNetCore.Http;

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
    [ProducesResponseType<IList<LedgerTagDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetListQuery<LedgerTagDto>());
        return query.Success
            ? Ok(query.Result)
            : BadRequest($"An error occured. Details: {string.Join(Environment.NewLine,query.Errors)}");
    }

    [HttpGet("{id}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var query = await _sessionHelper.TryExecuteQuery(new GetQuery<LedgerTagDto>(id));
        return query.Success
            ? Ok(query.Result)
            : NotFound();
    }

    [HttpPost]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableLedgerTagDto dto)
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

    [HttpPut("{id}/{version}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, int version, EditableLedgerTagDto dto)
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(new UpdateLedgerTagCommand(id, version, dto));
        return commandResult.Success 
            ? await GetById(id) 
            : BadRequest(commandResult);
    }
    
    [HttpPatch("{id}/{version}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(long id, int version, Dictionary<string, JsonElement> updates)
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(new PatchLedgerTagCommand(id, version, updates));
        return commandResult.Success 
            ? await GetById(id) 
            : BadRequest(commandResult);
    }
    
    [HttpDelete]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(new DeleteEntityCommand<LedgerTag>(id));
        if (!commandResult.Success)
        {
            return BadRequest(commandResult);
        }

        return Ok($"LedgerTag with id: {id} deleted.");
    }
}