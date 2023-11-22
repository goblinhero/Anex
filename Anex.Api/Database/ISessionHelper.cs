﻿using Anex.Api.Database.Queries;
using System.Threading.Tasks;
using Anex.Api.Database.Commands;

namespace Anex.Api.Database;

public interface ISessionHelper
{
    Task<QueryResult<T>> TryExecuteQuery<T>(IExecutableQuery<T> query);
    Task<CommandResult> TryExecuteCommand(IExecutableCommand command);
    void MigrateDatabaseToNewest();
    void MigrateDatabaseDownToVersion(long version);
}