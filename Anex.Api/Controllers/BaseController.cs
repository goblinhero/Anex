using System;
using System.Threading.Tasks;
using Anex.Api.Database;
using Anex.Api.Database.Commands;
using Anex.Api.Database.Queries;
using Anex.Domain.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Anex.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected readonly ISessionHelper _sessionHelper;

    protected BaseController(ISessionHelper sessionHelper)
    {
        _sessionHelper = sessionHelper;
    }

    protected async Task<IActionResult> TryExecuteQuery<T>(IExecutableQuery<T> query)
    {
        var result = await _sessionHelper.TryExecuteQuery(query);
        return result.Success
            ? Ok(result.Result)
            : BadRequest($"An error occured. Details: {string.Join(Environment.NewLine, result.Errors)}");
    }
    
    protected async Task<IActionResult> TryExecuteSingleQuery<T>(object id)
    {
        var result = await _sessionHelper.TryExecuteQuery(new GetQuery<T>(id));
        return result.Success
            ? Ok(result.Result)
            : NotFound();
    }

    protected async Task<IActionResult> TryExecuteDelete<T>(long id)
        where T : IEntity
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(new DeleteEntityCommand<T>(id));
        return commandResult.Success 
            ? Ok($"{nameof(T)} with id: {id} deleted.") 
            : BadRequest(commandResult);
    }

    protected async Task<IActionResult> TryExecuteUpdateCommand<T>(IExecutableCommand command, long id)
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(command);
        return commandResult.Success
            ? await TryExecuteSingleQuery<T>(id)
            : BadRequest(commandResult);
    }
    
    protected async Task<IActionResult> TryExecuteCreateCommand<T>(BaseCreateCommand<T> command) 
        where T : IHasId, IIsValidatable
    {
        var commandResult = await _sessionHelper.TryExecuteCommand(command);
        if (!commandResult.Success)
        {
            return BadRequest(commandResult);
        }
        
        return command.AssignedId.HasValue
            ? await TryExecuteSingleQuery<T>(command.AssignedId.Value)
            : BadRequest("Failed to assign id");    }
}