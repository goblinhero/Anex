using System.Threading.Tasks;
using NHibernate;

namespace Anex.Api.Database.Commands;

public interface IExecutableCommand
{
    Task<CommandResult> TryExecute(ISession session);
}