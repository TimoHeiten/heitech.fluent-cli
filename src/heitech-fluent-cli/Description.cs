namespace heitech_fluent_cli;

/// <summary>
/// Describes a switch or an argument
/// </summary>
/// <param name="ShortName"></param>
/// <param name="LongName"></param>
/// <param name="PropertyName"></param>
/// <param name="PropertyType"></param>
internal sealed record Description(char ShortName, string LongName, string PropertyName, Type PropertyType);