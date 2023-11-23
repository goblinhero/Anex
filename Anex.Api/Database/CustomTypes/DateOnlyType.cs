using System;
using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Anex.Api.Database.CustomTypes;

[Serializable]
public class DateOnlyType : IUserType
{
    public new bool Equals(object? x, object? y)
    {
        if (ReferenceEquals(x, y))
            return true;
        
        if (x == null || y == null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(object x)
    {
        return x.GetHashCode();
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        if (names.Length == 0)
            throw new ArgumentException("Expecting at least one column");

        var date = (DateTime)NHibernateUtil.Date.NullSafeGet(rs, names[0], session);
        return DateOnly.FromDateTime(date);
    }

    public void NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session)
    {
        var parameter = cmd.Parameters[index];

        if (value == null) {
            parameter.Value = null;
        }
        else {
            parameter.Value = ((DateOnly)value).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        }
    }

    public object DeepCopy(object value)
    {
        return value;
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object Assemble(object cached, object owner)
    {
        return cached;
    }

    public object Disassemble(object value)
    {
        return value;
    }

    public SqlType[] SqlTypes => new[] { new SqlType(DbType.Date) };
    public Type ReturnedType => typeof(DateOnly);
    public bool IsMutable => false;
}