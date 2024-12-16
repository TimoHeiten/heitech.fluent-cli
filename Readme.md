# Define your CLI arguments fluently

Motivation: I build a lot of CLI applications to help my development efforts.
So I needed a quick way to define some Arguments / Switches and StdIn via callbacks.

#### My requriments:
    - small interface
    - define fluently
    - support args, optional args, switches
    - use sync or async callbacks
    - type safety for my arguments

## Usage
## Example from the tests/terminal.tests assembly:
```csarp

var cmdArgsType = typeof(CommandArgs);
Console.WriteLine($"Run tests with '{cmdArgsType.Name}':");
var msg = string.Join(", ", cmdArgsType.GetProperties().Select(x => x.Name));
Console.WriteLine(msg);

// ---------------------------------------------------------------------------------------------------------
// define the argument
// ---------------------------------------------------------------------------------------------------------
var definitions = ArgumentDefinitions.Define<CommandArgs>(define =>
    define.Name("run")
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
```


### why another one?
I like this interface more, and mostly created it for myself. If you like it, feel free to use it.
It also allows for stdin, which most other libs I could find did not, and its a requirement a lot of my clis I am used to building have.

So TL;DR - scratchin´ my own itches here.
