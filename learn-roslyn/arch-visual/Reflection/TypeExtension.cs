namespace Reflection;

public static class TypeExtension {

    public static string GetHumanReadableName(this Type type)
    {
        if (type.IsGenericType)
        {
            return $"{type.Name[..^2]}<{String.Join(", ", type.GenericTypeArguments.Select(a => a.Name))}>";
        }

        return type.Name;
    }
}
