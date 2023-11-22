namespace Anex.Api.Utilities;

public interface IPropertyHelper
{
    bool TryGetValue<T>(string propertyName, out T? value);
}