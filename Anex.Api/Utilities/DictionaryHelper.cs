using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Anex.Api.Utilities;

public class DictionaryHelper : IPropertyHelper
{
    private readonly Dictionary<string, JsonElement> _properties;

    public DictionaryHelper(IDictionary<string, JsonElement> properties)
    {
        _properties = properties.ToDictionary(e => e.Key, e => e.Value, StringComparer.OrdinalIgnoreCase);
    }

    public bool TryGetValue<T>(string propertyName, out T? value)
    {
        if (!_properties.TryGetValue(propertyName, out var rawValue))
        {
            value = default;
            return false;
        }
        var strategies = new[]
        {
            new RelayStrategy<JsonElement?, object?>(_ => default(T), v => !v.HasValue || v.Value.ValueKind == JsonValueKind.Null),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetString(), _ => typeof(T) == typeof(string)),
            new RelayStrategy<JsonElement?, object?>(v => DateOnly.Parse(v!.Value.GetString()!), _ => typeof(T) == typeof(DateOnly)),
            new RelayStrategy<JsonElement?, object?>(v => DateOnly.Parse(v!.Value.GetString()!), _ => typeof(T) == typeof(DateOnly?)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetDecimal(), _ => typeof(T) == typeof(decimal)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetDecimal(), _ => typeof(T) == typeof(decimal?)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetInt32(), _ => typeof(T) == typeof(int)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetInt32(), _ => typeof(T) == typeof(int?)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetInt64(), _ => typeof(T) == typeof(long)),
            new RelayStrategy<JsonElement?, object?>(v => v!.Value.GetInt64(), _ => typeof(T) == typeof(long?)),
            new RelayStrategy<JsonElement?, object?>(v => v)
        };
        try
        {
            value = (T?)strategies.SafeExecute(rawValue);
            return true;
        }
        catch (Exception)
        {
            value = default;
            return false;
        }
    }
}
