using System;
using System.Collections.Generic;
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
public class FiscalPeriodController : BaseController
{
    public FiscalPeriodController(ISessionHelper sessionHelper)
        :base(sessionHelper)
    {
    }
    
    [HttpGet]
    [ProducesResponseType<IList<FiscalPeriodDto>>(200)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        return await TryExecuteQuery(new GetListQuery<FiscalPeriodDto>());
    }

    [HttpGet("{id}")]
    [ProducesResponseType<FiscalPeriodDto>(200)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        return await TryExecuteSingleQuery<FiscalPeriodDto>(id);
    }
    
    [HttpPost]
    [ProducesResponseType<FiscalPeriodDto>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(EditableFiscalPeriodDto dto)
    {
        return await TryExecuteCreateCommand<FiscalPeriod, FiscalPeriodDto>(new CreateFiscalPeriodCommand(dto));
    }
    
    [HttpDelete]
    [ProducesResponseType<string>(200)]
    [ProducesResponseType<CommandResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteById(long id)
    {
        return await TryExecuteDelete<FiscalPeriod>(id);
    }
}