using System.Threading.Tasks;
using Anex.Domain.Abstract;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class DeleteEntityCommand<TEntity> : IExecutableCommand
    where TEntity:IEntity
{
    private readonly long _id;

    public DeleteEntityCommand(long id)
    {
        _id = id;
    }

    public async Task<CommandResult> TryExecute(ISession session)
    {
        var entity = await session.GetAsync<TEntity>(_id);
        if (entity == null)
        {
            return new CommandResult(new[] { $"{typeof(TEntity).Name} not found with id: {_id}" });
        }

        await session.DeleteAsync(entity);
        return new CommandResult();
    }
}