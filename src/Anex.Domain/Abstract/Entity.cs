using Anex.Domain.Rules;

namespace Anex.Domain.Abstract;

public abstract class Entity<T> : IEntity
    where T : class
{
    public virtual long? Id { get; private set; }
    public virtual int Version { get; private set; }
    public virtual DateTime Created { get; set; }

    public virtual bool IsValid(out IEnumerable<string> validationErrors)
    {
        var entity = this as T ?? throw new Exception($"Invalid cast. Entity: {GetType().Name} is not assignable to {typeof(T).Name}");
        return new RuleSet<T>(GetValidationRules()).UpholdsRules(entity, out validationErrors);
    }
    protected virtual IEnumerable<IRule<T>> GetValidationRules()
    {
        return Enumerable.Empty<IRule<T>>();
    }
}