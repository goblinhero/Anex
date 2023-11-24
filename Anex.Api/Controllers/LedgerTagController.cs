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
public class LedgerTagController : BaseController
{
    public LedgerTagController(ISessionHelper sessionHelper)
        : base(sessionHelper)
    {
    }

    [HttpGet]
    [ProducesResponseType<IList<LedgerTagDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        return await TryExecuteQuery(new GetListQuery<LedgerTagDto>());
    }

    [HttpGet("{id}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        return await TryExecuteSingleQuery<LedgerTagDto>(id);
    }

    [HttpPost]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableLedgerTagDto dto)
    {
        return await TryExecuteCreateCommand(new CreateLedgerTagCommand(dto));
    }

    [HttpPut("{id}/{version}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(long id, int version, EditableLedgerTagDto dto)
    {
        return await TryExecuteUpdateCommand<LedgerTag>(new UpdateLedgerTagCommand(id, version, dto), id);
    }

    [HttpPatch("{id}/{version}")]
    [ProducesResponseType<LedgerTagDto>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Patch(long id, int version, Dictionary<string, JsonElement> updates)
    {
        return await TryExecuteUpdateCommand<LedgerTag>(new PatchLedgerTagCommand(id, version, updates), id);
    }

    [HttpDelete]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        return await TryExecuteDelete<LedgerTag>(id);
    }
}