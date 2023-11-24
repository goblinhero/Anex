using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Anex.Api.Database;
using Anex.Api.Database.Commands;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Anex.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LedgerPostDraftController : BaseController
{
    public LedgerPostDraftController(ISessionHelper sessionHelper) 
        : base(sessionHelper)
    {
    }
    
    [HttpGet]
    [ProducesResponseType<IList<LedgerPostDraftDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        return await TryExecuteQuery(new GetListQuery<LedgerPostDraftDto>());
    }

    [HttpGet("{id}")]
    [ProducesResponseType<LedgerPostDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        return await TryExecuteSingleQuery<LedgerPostDraftDto>(id);
    }

    [HttpPost]
    [ProducesResponseType<LedgerPostDraftDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableLedgerPostDraftDto dto)
    {
        return await TryExecuteCreateCommand<LedgerPostDraft, LedgerPostDraftDto>(new CreateLedgerPostDraftCommand(dto));
    }

    [HttpPut("{id}/{version}")]
    [ProducesResponseType<LedgerPostDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, int version, EditableLedgerPostDraftDto dto)
    {
        return await TryExecuteUpdateCommand<LedgerPostDraftDto>(new UpdateLedgerPostDraftCommand(id, version, dto), id);
    }

    [HttpPatch("{id}/{version}")]
    [ProducesResponseType<LedgerPostDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(long id, int version, Dictionary<string, JsonElement> updates)
    {
        return await TryExecuteUpdateCommand<LedgerPostDraftDto>(new PatchLedgerPostDraftCommand(id, version, updates), id);
    }

    [HttpDelete]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        return await TryExecuteDelete<LedgerPostDraft>(id);
    }
}