using System;
using System.Collections.Generic;

namespace Anex.Api.Database.Commands;

public struct CommandResult
{
    public CommandResult()
    {
        Success = true;
        Errors = Array.Empty<string>();
    }

    public CommandResult(IEnumerable<string> errors)
    {
        Success = false;
        Errors = errors;
    }
    public bool Success { get; }
    public IEnumerable<string> Errors { get; }
}