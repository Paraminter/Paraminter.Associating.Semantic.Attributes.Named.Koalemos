namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullPairer_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>());

        Assert.NotNull(result);
    }

    private static SemanticAttributeNamedAssociator Target(
        ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> pairer)
    {
        return new SemanticAttributeNamedAssociator(pairer);
    }
}
