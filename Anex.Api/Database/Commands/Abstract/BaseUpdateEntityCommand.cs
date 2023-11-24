using System.Threading.Tasks;
using Anex.Api.Database.Commands.Utilities;
using Anex.Domain.Abstract;
using NHibernate;

namespace Anex.Api.Database.Commands.Abstract;

public abstract class BaseUpdateEntityCommand<TEntity> : IExecutableCommand
    where TEntity : IEntity
{
    private readonly long _id;
    private readonly int _version;

    protected BaseUpdateEntityCommand(long id, int version)
    {
        _id = id;
        _version = version;
    }

    public async Task<CommandResult> TryExecute(ISession session)
    {
        var entity = await session.GetAsync<TEntity>(_id);
        if (entity == null)
        {
            return CommandResult.NotFoundResult<TEntity>(_id);
        }

        if (entity.Version != _version)
        {
            return CommandResult.VersionConflict<TEntity>(_id, entity.Version, _version);
        }

        var updateResult = await TryUpdateEntity(session, entity);
        if (!updateResult.Success) return updateResult;

        return !entity.IsValid(out var errors)
            ? new CommandResult(errors)
            : new CommandResult();
    }

    protected abstract Task<CommandResult> TryUpdateEntity(ISession session, TEntity entity);
}