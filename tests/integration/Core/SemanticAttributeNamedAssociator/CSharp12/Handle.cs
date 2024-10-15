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
using System.Linq;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void AttributeUsage_PairsAll()
    {
        var source = """
            using System;

            [Test(A = 1, B = "", C = false)]
            public class Foo { }

            public class TestAttribute : Attribute
            {
                public int A { get; set; }
                public string B { get; set; }
                public bool C { get; set; }
            }
            """;

        var compilation = CompilationFactory.Create(source);

        var type = compilation.GetTypeByMetadataName("Foo")!;
        var attribute = type.GetAttributes()[0];

        var associations = attribute.NamedArguments;
        var parameterNames = associations.Select(static (association) => association.Key).ToArray();
        var arguments = associations.Select(static (association) => association.Value).ToArray();

        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns(attribute.NamedArguments);

        Target(commandMock.Object);

        Fixture.PairerMock.Verify(PairArgumentExpression(parameterNames[0], arguments[0]), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameterNames[1], arguments[1]), Times.Once());
        Fixture.PairerMock.Verify(PairArgumentExpression(parameterNames[2], arguments[2]), Times.Once());
        Fixture.PairerMock.Verify(static (handler) => handler.Handle(It.IsAny<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Exactly(3));
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
