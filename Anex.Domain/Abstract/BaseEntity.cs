namespace Anex.Domain.Abstract;

public abstract class BaseEntity<T> : BaseValidatable<T>, IEntity
    where T : class
{
    protected BaseEntity()
    {
    }
    public virtual long? Id { get; private set; }
    public virtual int Version { get; private set; }
    public virtual DateTime Created { get; set; }

}