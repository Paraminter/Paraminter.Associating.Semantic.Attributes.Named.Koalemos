namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullIndividualAssociator_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>());

        Assert.NotNull(result);
    }

    private static SemanticAttributeNamedAssociator Target(
        ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> individualAssociator)
    {
        return new SemanticAttributeNamedAssociator(individualAssociator);
    }
}
