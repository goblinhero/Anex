using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using NHibernate;

namespace Anex.Api.Database.Queries;

public interface IExecutableQuery<T>
{
    Task<QueryResult<T>> TryExecute(ISession session);
}