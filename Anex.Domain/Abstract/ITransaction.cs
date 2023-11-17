namespace Anex.Domain.Abstract;

public interface ITransaction : IIsValidatable, IHasId, IHasCreationDate
{
    IEnumerable<string> UpdateableColumns { get; }
    int Version { get; }
}