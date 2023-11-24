using System.Threading.Tasks;
using Anex.Api.Database.Commands.Utilities;
using NHibernate;

namespace Anex.Api.Database.Commands.Abstract;

public interface IExecutableCommand
{
    Task<CommandResult> TryExecute(ISession session);
}