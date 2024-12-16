using heitech_fluent_cli;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.Parse;

var cmdArgsType = typeof(CommandArgs);
Console.WriteLine($"Run tests with '{cmdArgsType.Name}':");
var msg = string.Join(", ", cmdArgsType.GetProperties().Select(x => x.Name));
Console.WriteLine(msg);

// ---------------------------------------------------------------------------------------------------------
// define the argument
// ---------------------------------------------------------------------------------------------------------
var definitions = ArgumentDefinitions.Define<CommandArgs>(define =>
    define.Name("run", "run the test command")
        .Argument(x => x.Name, "name", 'n', "the name of the command")
        .Argument(x => x.Value, "value", 'v')
        .OptionalArgument(x => x.Optional, "optional", 'o')
        .Switch(x => x.IsEnabled, "enabled", 'e', "enables the command"));
// ---------------------------------------------------------------------------------------------------------

// ---------------------------------------------------------------------------------------------------------
// parse and evaluate the arguments, given by the stdin
// ---------------------------------------------------------------------------------------------------------
definitions
    .IsStandardIn(inArgs =>
    {
        var alLText = inArgs.IncomingStream.ReadToEnd();
        Console.WriteLine("stdin was: " + alLText);
    })
    .Is<CommandArgs>(args, 
(cmdArgs) =>
{
    var givenArgs = string.Join(", ", cmdArgs!.GetType().GetProperties().Select(x => $"{x.Name}: {x.GetValue(cmdArgs)}"));
    Console.WriteLine("the given arguments: ");
    Console.WriteLine(givenArgs);
});
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