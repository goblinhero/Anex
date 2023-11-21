using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate;

namespace Anex.Api.Database.Queries;

public class GetListQuery<T> : IExecutableQuery<IList<T>>
    where T : class
{
    public async Task<QueryResult<IList<T>>> TryExecute(ISession session)
    {
        var list = await session.QueryOver<T>().ListAsync();
        return new QueryResult<IList<T>>(list);
    }
}