using FluentAssertions;
using heitech_fluent_cli.DefineArgs;
using heitech_fluent_cli.Parse;

namespace heitech_fluent_cli.Tests;

public sealed class ErrrorCaseTests
{
    private sealed class ErrorArgs
    {
        public string Message { get; set; } = default!;
        public int ErrorCode { get; set; }
        public string? InnerError { get; set; }
        public bool ShouldSerialize { get; set; }

        [IgnoreMemberAsArgument]
        public object NotSupported { get; set; } = default!;
    }

    private Define<ErrorArgs> DefineErrorArgs()
    {
        return new Define<ErrorArgs>()
            .Argument(x => x.Message, "message")
            .Argument(x => x.ErrorCode, "code")
            .OptionalArgument(x => x.InnerError, "inner-error")
            .Switch(x => x.ShouldSerialize, "serialize");
    }

    [Fact]
    public void NotAll_Properties_Are_Defined_On_Object_Returns_Error()
    {
        // Arrange
        var definition = new Define<ErrorArgs>()
            .Argument(x => x.ErrorCode, "code")
            .OptionalArgument(x => x.InnerError, "inner-error")
            .Switch(x => x.ShouldSerialize, "serialize");

        // Act
        var act = () => definition.ParseArgs(new []
        {
            "--message", "hello",
            "--code", "400",
            "--inner-error", "inner",
            "--serialize"
        });

        // Assert
        act.Should().Throw<DefinitionException>();
    }
    
    [Fact]
    public void NotAll_Properties_Are_Defined_On_Object_Returns_NO_Error_If_IgnoreFlag_Is_Set()
    {
        // Arrange
        var definition = new Define<ErrorArgs>()
            .Argument(x => x.ErrorCode, "code")
            .OptionalArgument(x => x.InnerError, "inner-error")
            .Switch(x => x.ShouldSerialize, "serialize");

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--code", "400",
            "--inner-error", "inner",
            "--serialize"
        }, allPropertiesMustBeDefined: false);

        // Assert
        parsed.On(d => Assert.True(true), error: () => Assert.True(false));
    }

    [Fact]
    public void Not_All_Properties_Where_Set()
    {
        // Arrange
        var definition = new Define<ErrorArgs>()
            .Argument(x => x.Message, "message")
            .OptionalArgument(x => x.InnerError, "inner-error")
            .Switch(x => x.ShouldSerialize, "serialize");

        // Act
        var act = () => definition.ParseArgs(new []
        {
            "--message", "hello",
            "--code", "400",
            "--inner-error", "inner",
            "--serialize"
        });

        // Assert
        act.Should().Throw<DefinitionException>();
    }
    
    [Fact]
    public void Not_All_Properties_Where_Set_but_with_Flag_is_Ignored()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--code", "400",
            "--inner-error", "inner",
            "--serialize"
        }, ignoreAllPropertiesSet: true);

        // Assert
        parsed.On(d => Assert.True(true), error: () => Assert.True(false));
    }

    [Fact]
    public void ErrorCase_Definition_ArgDoesExistAlready_Throws()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var act = () => definition.Argument(x => x.Message, "message2");

        // Assert
        act.Should().Throw<DefinitionException>();
    }

    [Fact]
    public void ErrorCase_Definition_NotSupportedType_Throws()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var act = () => definition.Argument(x => x.NotSupported, "no-support-mate");

        // Assert
        act.Should().Throw<DefinitionException>();
    }

    [Fact]
    public void ErrorCase_Definition_LongNameWithWhitespace_Throws()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var act = () => definition.Argument(x => x.NotSupported, "no-support-mate");

        // Assert
        act.Should().Throw<DefinitionException>();
    }
    
    [Fact]
    public void ErrorCase_Definition_MultipleArgsToSameProperty_Throws()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var act = () => definition.Argument(x => x.NotSupported, "no-support-mate");

        // Assert
        act.Should().Throw<DefinitionException>();
    }

    [Fact]
    public void ErrorCase_Parsing_Arg_Not_Defined_Invokes_OnError()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--message", "hello",
            "--code", "400",
            "--inner-error", "inner",
            "-o", "outFile x",
            "--serialize"
        });

        // Assert
        parsed.On(d => Assert.True(false), error: () => Assert.True(true));
    }
    
    [Fact]
    public void ErrorCase_Parsing_Arg_Is_Given_Multiple_Times_Invokes_OnError()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--message", "hello",
            "--code", "400",
            "--inner-error", "inner",
            "--inner-error", "inner2",
            "--serialize"
        });

        // Assert
        parsed.On(d => Assert.True(false), error: () => Assert.True(true));
    }
    
    [Fact]
    public void ErrorCase_Parsing_Arg_Is_Defined_Multiple_Times_With_Switch_Invokes_OnError()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--message", "hello",
            "--code", "400",
            "--inner-error", "inner",
            "--serialize",
            "--serialize"
        });

        // Assert
        parsed.On(d => Assert.True(false), error: () => Assert.True(true));
    }
    
    [Fact]
    public void ErrorCase_Parsing_Arg_has_No_Value_Attached_Invokes_OnError()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--message",
            "--code", "400",
            "--inner-error",
            "--serialize"
        });

        // Assert
        parsed.On(d => Assert.True(false), error: () => Assert.True(true));
    }
    
    [Fact]
    public void ErrorCase_Parsing_Arg_WrongType_OnArg_Invokes_OnError()
    {
        // Arrange
        var definition = DefineErrorArgs();

        // Act
        var parsed = definition.ParseArgs(new []
        {
            "--message", "lel",
            "--code", "is no int",
            "--inner-error",
            "--serialize"
        });

        // Assert
        parsed.On(d => Assert.True(false), error: () => Assert.True(true));
    }
}