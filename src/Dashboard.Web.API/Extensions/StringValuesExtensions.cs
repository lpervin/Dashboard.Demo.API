using Microsoft.Extensions.Primitives;

namespace Dashboard.Demo.API.Extensions;

public static class StringValuesExtensions
{
    public static int TryParseInt(this StringValues values, int? defaultValue = 0)
    {
        if (values.Count != 1)
            return defaultValue!.Value;

        bool canParse = int.TryParse(values.ToString(), out var parsed);
        return canParse ? parsed : defaultValue!.Value;
    }
}