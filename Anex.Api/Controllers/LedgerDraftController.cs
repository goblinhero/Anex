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
public class LedgerDraftController : BaseController
{
    public LedgerDraftController(ISessionHelper sessionHelper) 
        : base(sessionHelper)
    {
    }
    
    [HttpGet]
    [ProducesResponseType<IList<LedgerDraftDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        return await TryExecuteQuery(new GetListQuery<LedgerDraftDto>());
    }

    [HttpGet("{id}")]
    [ProducesResponseType<LedgerDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        return await TryExecuteSingleQuery<LedgerDraftDto>(id);
    }

    [HttpPost]
    [ProducesResponseType<LedgerDraftDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableLedgerDraftDto dto)
    {
        return await TryExecuteCreateCommand<LedgerDraft, LedgerDraftDto>(new CreateLedgerDraftCommand(dto));
    }

    [HttpPut("{id}/{version}")]
    [ProducesResponseType<LedgerDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, int version, EditableLedgerDraftDto dto)
    {
        return await TryExecuteUpdateCommand<LedgerDraftDto>(new UpdateLedgerDraftCommand(id, version, dto), id);
    }

    [HttpPatch("{id}/{version}")]
    [ProducesResponseType<LedgerDraftDto>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(long id, int version, Dictionary<string, JsonElement> updates)
    {
        return await TryExecuteUpdateCommand<LedgerDraftDto>(new PatchLedgerDraftCommand(id, version, updates), id);
    }

    [HttpDelete]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        return await TryExecuteDelete<LedgerDraft>(id);
    }
}