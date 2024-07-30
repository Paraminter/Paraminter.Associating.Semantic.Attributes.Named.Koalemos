namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Handlers;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullQuery_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<IAssociateSemanticAttributeNamedQueryResponseHandler>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullQueryResponseHandler_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NoAssociations_AddsNone()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Associations).Returns([]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticAttributeNamedAssociationCommand>()), Times.Never());
    }

    [Fact]
    public void SomeAssociations_AddsAllPairwise()
    {
        var parameter1Name = "Name1";
        var parameter2Name = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1Name, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2Name, argument2);

        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Associations).Returns([association1, association2]);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticAttributeNamedAssociationCommand>()), Times.Exactly(2));
        queryResponseHandlerMock.Verify(AssociationExpression(parameter1Name, argument1), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameter2Name, argument2), Times.Once());
    }

    private static Expression<Action<IAssociateSemanticAttributeNamedQueryResponseHandler>> AssociationExpression(
        string parameterName,
        TypedConstant argument)
    {
        return (handler) => handler.AssociationCollector.Handle(It.Is(MatchAssociationCommand(parameterName, argument)));
    }

    private static Expression<Func<IAddSemanticAttributeNamedAssociationCommand, bool>> MatchAssociationCommand(
        string parameterName,
        TypedConstant argument)
    {
        return (command) => Equals(command.ParameterName, parameterName) && Equals(command.Argument, argument);
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData> query,
        IAssociateSemanticAttributeNamedQueryResponseHandler queryResponseHandler)
    {
        Fixture.Sut.Handle(query, queryResponseHandler);
    }
}
