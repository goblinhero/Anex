namespace Anex.Domain.Rules;

public interface IRule<T>
{
    bool IsBroken(T entity);
    string BrokenMessage { get; }
}