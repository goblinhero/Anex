namespace Anex.Domain.Abstract;

public abstract class BaseTransaction<T> : BaseValidatable<T>, ITransaction
    where T : class
{
    public virtual IEnumerable<string> UpdateableColumns => Enumerable.Empty<string>();
    public virtual long? Id { get; private set; }
    public virtual int Version { get; private set; }
    public virtual DateTime Created { get; set; }
}