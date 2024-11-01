using System.Reflection;

namespace heitech_fluent_cli;

internal static class Validate
{
    private static Type[] s_allowedTypes = 
    {
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(byte), typeof(sbyte),
        typeof(ushort), typeof(uint), typeof(ulong),
        typeof(short), typeof(int), typeof(long),
        typeof(double), typeof(float), typeof(decimal)
    };

    internal static void Definition(Description validateThis, IReadOnlyList<Description> existingArgs, Type[]? allowedTypes = null)
    {
        var propertyIsAssigned = existingArgs.Any(x => x.PropertyName == validateThis.PropertyName);
        if (propertyIsAssigned)
        {
            throw new DefinitionException($"Property '{validateThis.PropertyName}' is already assigned");
        }

        var shortNameIsAssigned = existingArgs.Any(x => x.ShortName == validateThis.ShortName);
        if (shortNameIsAssigned)
        {
            throw new DefinitionException($"ShortName '{validateThis.ShortName}' is already assigned");
        }

        var longNameIsAssigned = existingArgs.Any(x => x.LongName == validateThis.LongName);
        if (longNameIsAssigned)
        {
            throw new DefinitionException($"LongName '{validateThis.LongName}' is already assigned");
        }

        var isAllowedType = (allowedTypes ?? s_allowedTypes).Any(x => x == validateThis.PropertyType);
        if (!isAllowedType)
        {
            throw new DefinitionException($"Type '{validateThis.PropertyType.Name}' is not allowed");
        }
    }

    public static void AllPropertiesAreDefined<T>(IReadOnlyList<Description> toList, IReadOnlyList<Description> switches) where T : new()
    {
        var properties = typeof(T).GetProperties().Where(x => x.GetCustomAttribute<IgnoreMemberAsArgumentAttribute>() == null).ToArray();
        var count = properties.Length;
        var defined = toList.Count + switches.Count;
        
        if (count != defined)
        {
            throw new DefinitionException($"Not all properties are defined. Expected: {count} but got: {defined}");
        }
    }
}