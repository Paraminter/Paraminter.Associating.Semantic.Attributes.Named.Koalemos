namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

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
    public void NoAssociations_AssociatesNone()
    {
        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([]);

        Target(commandMock.Object);

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Never());
    }

    [Fact]
    public void SomeAssociations_AssociatesAllPairwise()
    {
        var parameter1Name = "Name1";
        var parameter2Name = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1Name, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2Name, argument2);

        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([association1, association2]);

        Target(commandMock.Object);

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Exactly(2));
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameter1Name, argument1), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualExpression(parameter2Name, argument2), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>> AssociateIndividualExpression(
        string parameterName,
        TypedConstant argument)
    {
        return (associator) => associator.Handle(It.Is(MatchAssociateIndividualCommand(parameterName, argument)));
    }

    private static Expression<Func<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>, bool>> MatchAssociateIndividualCommand(
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
        IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData> command)
    {
        Fixture.Sut.Handle(command);
    }
}
