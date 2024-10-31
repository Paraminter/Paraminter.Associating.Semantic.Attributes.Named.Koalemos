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
using System.Threading;
using System.Threading.Tasks;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public async Task NullCommand_ThrowsArgumentNullException()
    {
        var result = await Record.ExceptionAsync(() => Target(null!, CancellationToken.None));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public async Task NoAssociations_PairsNone()
    {
        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([]);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Fact]
    public async Task SomeAssociations_PairsAll()
    {
        var parameter1Name = "Name1";
        var parameter2Name = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1Name, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2Name, argument2);

        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns([association1, association2]);

        await Target(commandMock.Object, CancellationToken.None);

        Fixture.PairerMock.Verify(PairArgumentExpression(parameter1Name, argument1, It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameter2Name, argument2, It.IsAny<CancellationToken>()), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
    }

    private static Expression<Func<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>, Task>> PairArgumentExpression(
        string parameterName,
        TypedConstant argument,
        CancellationToken cancellationToken)
    {
        return (handler) => handler.Handle(It.Is(MatchPairArgumentCommand(parameterName, argument)), cancellationToken);
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

    private async Task Target(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData> command,
        CancellationToken cancellationToken)
    {
        await Fixture.Sut.Handle(command, cancellationToken);
    }
}
