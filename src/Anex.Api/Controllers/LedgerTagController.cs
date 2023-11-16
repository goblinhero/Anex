using Anex.Api.Dto;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Anex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LedgerTagController : ControllerBase
    {
        private static Dictionary<long, LedgerTagDto> _ledgerTags = new Dictionary<long,LedgerTagDto>
        {
            {1, new LedgerTagDto{Id=1, Description="Telephone costs"}},
            {2, new LedgerTagDto{Id=2, Description="Other costs"} }
        };
        public LedgerTagController()
        {
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok(_ledgerTags.Values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            return _ledgerTags.TryGetValue(id, out var ledgerTag) 
                ? Ok(ledgerTag) 
                : NotFound();
        }
    }
}