using System;
using Anex.Api.Database;
using Microsoft.AspNetCore.Mvc;

namespace Anex.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AdministrationController : ControllerBase
{
    private readonly ISessionHelper _sessionHelper;

    public AdministrationController(ISessionHelper sessionHelper)
    {
        _sessionHelper = sessionHelper;
    }

    [HttpPost("DBMigrateDown/{version}")]
    public IActionResult MigrateDown(long version)
    {
        try
        {
            _sessionHelper.MigrateDatabaseDownToVersion(version);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Description = $"Failed to migrate database to version {version}", ex.Message, ex.StackTrace });
        }

        return Ok("Database successfully migrated!");
    }
    
    [HttpPost("DBMigrateUp")]
    public IActionResult MigrateUp()
    {
        try
        {
            _sessionHelper.MigrateDatabaseToNewest();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Description = "Failed to migrate database", ex.Message, ex.StackTrace });
        }

        return Ok("Database successfully migrated!");
    }
}