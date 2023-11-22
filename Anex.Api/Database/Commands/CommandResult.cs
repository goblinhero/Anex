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

    public static CommandResult NotFoundResult<T>(long id)
    {
        return new CommandResult(new[] { $"{typeof(T).Name} not found with id: {id}" });
    }

    public static CommandResult VersionConflict<T>(long id, int currentVersion, int dtoVersion)
    {
        return new CommandResult(new[] { $"{typeof(T).Name} was found with id: {id}, but has version {currentVersion}. Expected version was: {dtoVersion}" });
    }
}