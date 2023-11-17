using NHibernate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Anex.Api.Database.Queries;

public class GetQuery<T> : IExecutableQuery<T>
{
    private readonly object _id;

    public GetQuery(object id)
    {
        _id = id;
    }

    public async Task<QueryResult<T>> TryExecute(ISession session)
    {
        var entity = await session.GetAsync<T>(_id);
        return entity != null 
            ? new QueryResult<T>(entity) 
            : new QueryResult<T>($"Failed to find {typeof(T).Name} with id: {_id}");
    }
}
public class GetListQuery<T> : IExecutableQuery<IList<T>>
    where T : class
{
    public async Task<QueryResult<IList<T>>> TryExecute(ISession session)
    {
        var list = await session.QueryOver<T>().ListAsync();
        return new QueryResult<IList<T>>(list);
    }
}