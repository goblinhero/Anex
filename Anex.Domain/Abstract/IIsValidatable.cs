namespace Anex.Domain.Abstract;

public interface IIsValidatable
{
    bool IsValid(out IEnumerable<string> validationErrors);
}