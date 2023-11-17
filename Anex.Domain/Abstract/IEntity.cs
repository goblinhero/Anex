namespace Anex.Domain.Abstract;

public interface IEntity : IIsValidatable, IHasId, IHasCreationDate
{
    int Version { get; }
}