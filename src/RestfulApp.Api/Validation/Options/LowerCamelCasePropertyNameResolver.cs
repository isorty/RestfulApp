using FluentValidation.Internal;
using RestfulApp.Api.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace RestfulApp.Api.Validation.Options;

public class LowerCamelCasePropertyNameResolver
{

    public static string ResolvePropertyName(Type type, MemberInfo memberInfo, LambdaExpression expression)
    {
        if (expression != null)
        {
            var chain = PropertyChain.FromExpression(expression);
            if (chain.Count > 0)
                return chain.ToString().ToLowerCamelCase();
        }

        if (memberInfo != null)
        {
            return memberInfo.Name.ToLowerCamelCase();
        }

        return null;
    }
}