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
var definitions = ArgumentDefinitions.Define<CommandArgs, Args2>(define =>
    define.Name("run", "run the test command")
        .Argument(x => x.Name, "name", 'n', "the name of the command")
        .Argument(x => x.Value, "value", 'v')
        .OptionalArgument(x => x.Optional, "optional", 'o')
        .Switch(x => x.IsEnabled, "enabled", 'e', "enables the command"),
    
    a2 => a2.Name("show", "show the stuff")
         .Switch(x => x.ShowStuff, "show", 's', "show the stuff")
);
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
    .Is<CommandArgs>(cmdArgs =>
    {
        var givenArgs = string.Join(", ", cmdArgs!.GetType().GetProperties().Select(x => $"{x.Name}: {x.GetValue(cmdArgs)}"));
        Console.WriteLine("the given arguments: ");
        Console.WriteLine(givenArgs);
    }).Is<Args2>(s =>
    {
        if (s.ShowStuff)
            Console.WriteLine("show stuff was set");
    });

// ---------------------------------------------------------------------------------------------------------

var runner = definitions.Build();
await runner.Run(args);

Console.ReadLine();

public sealed record Args2
{
    public bool ShowStuff { get; set; }
}

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