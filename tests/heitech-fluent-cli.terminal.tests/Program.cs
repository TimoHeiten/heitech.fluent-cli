using heitech_fluent_cli;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.Parse;

var cmdArgsType = typeof(CommandArgs);
Console.WriteLine($"Run tests with '{cmdArgsType.Name}':");
var msg = string.Join(", ", cmdArgsType.GetProperties().Select(x => x.Name));
Console.WriteLine(msg);

// ---------------------------------------------------------------------------------------------------------
// define the arguments
// ---------------------------------------------------------------------------------------------------------
var definition = new Define<CommandArgs>()
    .Argument(x => x.Name, "name", 'n')
    .Argument(x => x.Value, "value", 'v')
    .OptionalArgument(x => x.Optional, "optional", 'o')
    .Switch(x => x.IsEnabled, "enabled", 'e');
// ---------------------------------------------------------------------------------------------------------

// ---------------------------------------------------------------------------------------------------------
// parse and evaluate the arguments, given by the stdin
// ---------------------------------------------------------------------------------------------------------
var result = definition.ParseArgs(args);
result.On(cmdArgs =>
{
    var givenArgs = string.Join(", ", cmdArgs!.GetType().GetProperties().Select(x => $"{x.Name}: {x.GetValue(cmdArgs)}"));
    Console.WriteLine("the given arguments: ");
    Console.WriteLine(givenArgs);
}, () => Console.WriteLine("something did not work :("));
// ---------------------------------------------------------------------------------------------------------

Console.ReadLine();
public sealed record CommandArgs
{
    public string Name { get; set; } = default!;
    public int Value { get; set; }
    public bool IsEnabled { get; set; }
    public string Optional { get; set; } = default!;
    
    [IgnoreMemberAsArgument]
    public object Ignored { get; set; } = default!;

    /// <summary>
    /// ctor for Definition
    /// </summary>
    public CommandArgs()
    { }
}