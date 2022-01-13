namespace RestfulApp.Api.Extensions;

public static class StringExtensions
{
    public static string ToLowerCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return char.ToLowerInvariant(value[0]) + value[1..];
    }
}