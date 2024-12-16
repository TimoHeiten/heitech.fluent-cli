using FluentAssertions;
using heitech_fluent_cli.DefineArgs;

namespace heitech_fluent_cli.Tests;

public sealed class DefinitionTests
{
    public sealed class Args
    {
        public int Size { get; set; } = default;
        public string Path { get; set; } = default!;
        public bool IsVerbose { get; set; } = default;
    }

    [Fact]
    public void Basic_Definition_Of_Args_Works()
    {
        var args = new[] {"-s", "400", "--path", "C:\\home", "--verbose"};

        // Act
        var definition = new Define<Args>()
            .Argument(x => x.Size, "size")
            .Argument(x => x.Path, "path")
            .Switch(x => x.IsVerbose, "verbose");

        // Assert
        definition.ParseArgs(args).On(x =>
        {
            x.Should().BeOfType<Args>();
            x.Should().BeEquivalentTo(new
            {
                Size = 400,
                Path = "C:\\home",
                IsVerbose = true
            });
        }, () => Assert.True(false));
    }
    
    [Fact]
    public void Basic_Definition_Of_Args_Works_withoutSwitch()
    {
        var args = new[] {"-s", "400", "--path", "C:\\home"};

        // Act
        var definition = new Define<Args>()
            .Argument(x => x.Size, "size")
            .Argument(x => x.Path, "path")
            .Switch(x => x.IsVerbose, "verbose");

        // Assert
        definition.ParseArgs(args).On(x =>
        {
            x.Should().BeOfType<Args>();
            x.Should().BeEquivalentTo(new
            {
                Size = 400,
                Path = "C:\\home",
                IsVerbose = false
            });
        }, () => Assert.True(false));
    }

    private sealed record ArgsWithHyphens
    {
        public string Abc { get; set; } = default!;
        public ArgsWithHyphens()
        { }
    }

    [Fact]
    public void Double_Hyphen_works_in_definition()
    {
        // Arrange
        var args = new [] { "--a-b-c", "value" };

        // Act
        var definition = new Define<ArgsWithHyphens>()
            .Argument(x => x.Abc, "a-b-c", 'b');

        // Assert
        definition.ParseArgs(args).On(x =>
        {
            x.Should().BeEquivalentTo(new
            {
                Abc = "value"
            });
        }, () => Assert.True(false));
    }

    [Fact]
    public void Definition_of_optional_Argument_Works()
    {
        // Arrange
        var args = new[]
        {
            "--optional", "value1",
            "--non-optional", "value2",
            "--switch1",
            "-z"
        };

        // Act
        var definition = new Define<OptionalArgs>()
            .OptionalArgument(x => x.Optional, "optional")
            .Argument(x => x.NonOptional, "non-optional")
            .Switch(x => x.Switch1, "switch1", 's')
            .Switch(x => x.Switch2, "switch2", 'z');

        // Assert
        definition.ParseArgs(args).On(x =>
        {
            x.Should().BeEquivalentTo(new
            {
                Optional = "value1",
                NonOptional = "value2",
                Switch1 = true,
                Switch2 = true
            });
        }, () => Assert.True(false));
    }
    
    [Fact]
    public void Definition_of_optional_Argument_with_No_Arg_Works()
    {
        // Arrange
        var args = new[]
        {
            "--non-optional", "value1",
            "--switch1",
            "-z"
        };

        // Act
        var definition = new Define<OptionalArgs>()
            .OptionalArgument(x => x.Optional, "optional")
            .Argument(x => x.NonOptional, "non-optional")
            .Switch(x => x.Switch1, "switch1", 's')
            .Switch(x => x.Switch2, "switch2", 'z');

        // Assert
        definition.ParseArgs(args).On(x =>
        {
            x.Should().BeEquivalentTo(new
            {
                NonOptional = "value1",
                Switch1 = true,
                Switch2 = true
            });
        }, () => Assert.True(false));
    }
    
    private sealed record OptionalArgs
    {
        public string? Optional { get; set; }
        public string NonOptional { get; set; } = default!;
        public bool Switch1 { get; set; }
        public bool Switch2 { get; set; }
    }
}