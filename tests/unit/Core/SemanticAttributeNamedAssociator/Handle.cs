namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullCommand_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NoAssociations_RecordsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticAttributeNamedAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void SomeAssociations_RecordsAllPairwise()
    {
        var parameter1Name = "Name1";
        var parameter2Name = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1Name, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2Name, argument2);

        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([association1, association2]);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticAttributeNamedAssociationCommand>()), Times.Exactly(2));
        Fixture.RecorderMock.Verify(RecordExpression(parameter1Name, argument1), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameter2Name, argument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>>> RecordExpression(
        string parameterName,
        TypedConstant argument)
    {
        return (recorder) => recorder.Handle(It.Is(MatchRecordCommand(parameterName, argument)));
    }

    private static Expression<Func<IRecordSemanticAttributeNamedAssociationCommand, bool>> MatchRecordCommand(
        string parameterName,
        TypedConstant argument)
    {
        return (command) => Equals(command.ParameterName, parameterName) && Equals(command.Argument, argument);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
