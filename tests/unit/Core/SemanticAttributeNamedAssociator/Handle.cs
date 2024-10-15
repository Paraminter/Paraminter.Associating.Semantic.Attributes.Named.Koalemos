namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;
using Paraminter.Cqs;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

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
    public void NoAssociations_PairsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([]);

        Target(commandMock.Object);

        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SomeAssociations_PairsAll()
    {
        var parameter1Name = "Name1";
        var parameter2Name = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1Name, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2Name, argument2);

        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([association1, association2]);

        Target(commandMock.Object);

        Fixture.PairerMock.Verify(PairArgumentExpression(parameter1Name, argument1), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameter2Name, argument2), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Exactly(2));
    }

    private static Expression<Action<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>> PairArgumentExpression(
        string parameterName,
        TypedConstant argument)
    {
        return (handler) => handler.Handle(It.Is(MatchPairArgumentCommand(parameterName, argument)));
    }

    private static Expression<Func<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>, bool>> MatchPairArgumentCommand(
        string parameterName,
        TypedConstant argument)
    {
        return (command) => MatchParameter(parameterName, command.Parameter) && MatchArgumentData(argument, command.ArgumentData);
    }

    private static bool MatchParameter(
        string parameterName,
        INamedParameter parameter)
    {
        return Equals(parameterName, parameter.Name);
    }

    private static bool MatchArgumentData(
        TypedConstant argument,
        ISemanticAttributeNamedArgumentData argumentData)
    {
        return Equals(argument, argumentData.Argument);
    }

    private void Target(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
