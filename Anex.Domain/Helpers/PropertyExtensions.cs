using System.Linq.Expressions;

namespace Anex.Domain.Helpers;

public static class PropertyExtensions
{
    public static string GetName<T, TProperty>(this Expression<Func<T, TProperty>> exp)
    {
        if (exp.Body is not MemberExpression body)
        {
            UnaryExpression ubody = (UnaryExpression)exp.Body;
            body = (MemberExpression)ubody.Operand;
        }

        return body.Member.Name;
    }
}
