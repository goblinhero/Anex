using System.Linq.Expressions;
using Anex.Domain.Helpers;
using Anex.Domain.Rules;

namespace Anex.Domain.Abstract;

public abstract class BaseValidatable<T> : IIsValidatable
    where T : class
{
    public virtual bool IsValid(out IEnumerable<string> validationErrors)
    {
        var entity = this as T ?? throw new Exception($"Invalid cast. Entity: {GetType().Name} is not assignable to {typeof(T).Name}");
        return new RuleSet<T>(GetValidationRules()).UpholdsRules(entity, out validationErrors);
    }
    protected virtual IEnumerable<IRule<T>> GetValidationRules()
    {
        return Enumerable.Empty<IRule<T>>();
    }
    protected IRule<T> CannotBeEmpty(Expression<Func<T, string>> property)
    {
        var expression = property.Compile();
        Predicate<T> isBrokenWhen = T => string.IsNullOrWhiteSpace(expression(T));
        return new RelayRule<T>(isBrokenWhen, $"{property.GetName()} cannot be empty for a {typeof(T).Name}");
    }
    protected IRule<T> CannotBeNull<TProperty>(Expression<Func<T, TProperty?>> property)
    {
        var expression = property.Compile();
        Predicate<T> isBrokenWhen = T => expression(T) == null;
        return new RelayRule<T>(isBrokenWhen, $"{property.GetName()} cannot be empty for a {typeof(T).Name}");
    }
}