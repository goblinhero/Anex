using System.Collections.Generic;
using System.Linq;

namespace Anex.Api.Database.Queries;

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