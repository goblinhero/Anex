using Anex.Api.Database.Queries;
using System.Threading.Tasks;

namespace Anex.Api.Database;

public interface ISessionHelper
{
    Task<QueryResult<T>> TryExecuteQuery<T>(IExecutableQuery<T> query);
    string? GetConnectionString();
}