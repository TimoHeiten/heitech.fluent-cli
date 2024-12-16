using FluentAssertions;
using heitech_fluent_cli.DefineArgs;

namespace heitech_fluent_cli.Tests;

public sealed class DefineMultipleTests
{
    public sealed class Args1
    {
        public string Name { get; set; }
    }
    
    public sealed class Args2
    {
        public bool Sw { get; set; }
    }
    
    public sealed class Args3
    {
        public string? Optional { get; set; }
    }
    
    [Theory, MemberData(nameof(DefinedArgs))]
    public async Task Multiple_args_parses_the_command(string[] cliArgs, bool a1, bool a2, bool a3)
    {
        // Arrange
        var definedArguments = ArgumentDefinitions.Define<Args1, Args2, Args3>(
            arg1 => arg1.Name("a").Argument(x => x.Name, "name"),
            arg2 => arg2.Name("b").Switch(s => s.Sw, "name2"),
            arg3 => arg3.Name("c").OptionalArgument(x => x.Optional, "name3")
        );
        var actualA1 = false;
        var actualA2 = false;
        var actualA3 = false;
        
        // Act
        await definedArguments.Is<Args1>((_) => actualA1 = true)
            .Is<Args2>((_) => actualA2 = true)
            .Is<Args3>((_) => actualA3 = true)
            .Build().Run(cliArgs);
        

        // Assert
        actualA1.Should().Be(a1);
        actualA2.Should().Be(a2);
        actualA3.Should().Be(a3);
    }

    [Fact]
    public void NotAllDefined_Throws()
    {
        // Arrange
        var definedArguments = ArgumentDefinitions.Define<Args1>(
            arg1 => arg1.Name("a").Argument(x => x.Name, "name"));

        string[] cliArgs = {"a", "-n", "val"};
        
        // Act
        var act = () => definedArguments.Is<Args1>((_) => { })
            .Is<Args2>((_) => { });

        // Assert
        act.Should().Throw<DefinitionException>();
    }

    public static IEnumerable<object[]> DefinedArgs
    {
        get
        {
            yield return new object[]
            {
                new[] {"a", "-n", "val"},
                true, false, false
            };
            yield return new object[]
            {
                new[] {"b", "-n"},
                false, true, false
            };
            yield return new object[]
            {
                new[] {"c", "-n", "val"},
                false, false, true
            };
        }
    }
}