using System;
using Anex.Api.Database;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LedgerTagController : ControllerBase
    {
        private readonly ISessionHelper _sessionHelper;

        public LedgerTagController(ISessionHelper sessionHelper)
        {
            _sessionHelper = sessionHelper;
        }

        [HttpGet()]
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

        [HttpGet("Environment")]
        public Task<IActionResult> GetEnvironmentVariables()
        {
            return Task.FromResult((IActionResult)Ok(Environment.GetEnvironmentVariables()));
        }
        
        [HttpGet("ConnectionStrings")]
        public Task<IActionResult> ConnectionStrings()
        {
            var connections = new List<string>();
            foreach (System.Configuration.ConnectionStringSettings css in System.Configuration.ConfigurationManager.ConnectionStrings)
            {
                connections.Add(css.ConnectionString);
            }
            return Task.FromResult((IActionResult)Ok(connections));
        }
    }
}