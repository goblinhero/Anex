using System;
using System.Linq.Expressions;
using Anex.Api.Utilities;
using Anex.Domain.Abstract;
using Anex.Domain.Helpers;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class EntitySetter<TEntity>
    where TEntity : IEntity
{
    private readonly TEntity _entity;
    private readonly IPropertyHelper _propertyHelper;


    public EntitySetter(IPropertyHelper propertyHelper, TEntity entity)
    {
        _entity = entity;
        _propertyHelper = propertyHelper;
    }

    public void UpdateSimpleProperty<T>(Expression<Func<TEntity, T>> property, Action<T, T?>? postUpdate = null)
    {
        var propName = property.GetName();
        if (!_propertyHelper.TryGetValue(propName, out T? newValue))
            return;
        
        UpdateEntity(property, newValue, postUpdate);
    }

    private void UpdateEntity<T>(Expression<Func<TEntity, T>> property, T? newValue, Action<T, T?>? postUpdate)
    {
        var propName = property.GetName();
        var oldValue = property.Compile().Invoke(_entity);
        if (Equals(oldValue, newValue))
            return;
        
        var propertyInfo = typeof(TEntity).GetProperty(propName);
        if (propertyInfo == null) throw new Exception($"Property {propName} not found on type {typeof(TEntity).Name}");
        if(!propertyInfo.CanWrite) throw new Exception($"Property {propName} found on type {typeof(TEntity).Name} but it is read-only");
        
        propertyInfo.SetValue(_entity, newValue, null);
        postUpdate?.Invoke(oldValue, newValue);
    }

    public void UpdateComplexProperty<T>(Expression<Func<TEntity, T>> property, ISession session, Action<T, T?>? postUpdate = null)
        where T : class, IHasId
    {
        var propName = $"{property.GetName()}Id";
        if (!_propertyHelper.TryGetValue(propName, out long? newId))
            return;
        var newValue = newId.HasValue ? session.Get<T>(newId.Value) : null;
        UpdateEntity(property, newValue, postUpdate);
    }
}