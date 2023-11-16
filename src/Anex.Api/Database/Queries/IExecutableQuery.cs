using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate;

namespace Anex.Api.Database.Queries;

public interface IExecutableQuery<T>
{
    Task<QueryResult<T>> TryExecute(ISession session);
}
public struct QueryResult<T>
{
    public QueryResult(T result)
    {
        Success = true;
        Result = result;
        Errors = Enumerable.Empty<string>();
    }
    public QueryResult(params string[] errors)
    {
        Success = false;
        Errors = errors;
    }
    public bool Success { get; }
    public IEnumerable<string> Errors { get; }
    public T? Result { get; }
}