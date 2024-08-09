namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Recorders.Commands;

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
        var result = Target(Mock.Of<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>());

        Assert.NotNull(result);
    }

    private static SemanticAttributeNamedAssociator Target(
        ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> recorder)
    {
        return new SemanticAttributeNamedAssociator(recorder);
    }
}
