namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;

using System;

using Xunit;

public sealed class Constructor
{
    [Fact]
    public void NullRecorder_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void ValidArguments_ReturnsAssociator()
    {
        var result = Target(Mock.Of<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>>());

        Assert.NotNull(result);
    }

    private static SemanticAttributeNamedAssociator Target(
        ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand> recorder)
    {
        return new SemanticAttributeNamedAssociator(recorder);
    }
}
