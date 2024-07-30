namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Handlers;

using System;
using System.Linq;
using System.Linq.Expressions;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void AttributeUsage_AssociatesAll()
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

        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseHandler> queryResponseHandlerMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Associations).Returns(attribute.NamedArguments);

        Target(queryMock.Object, queryResponseHandlerMock.Object);

        queryResponseHandlerMock.Verify(static (handler) => handler.AssociationCollector.Handle(It.IsAny<IAddSemanticAttributeNamedAssociationCommand>()), Times.Exactly(3));
        queryResponseHandlerMock.Verify(AssociationExpression(parameterNames[0], arguments[0]), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameterNames[1], arguments[1]), Times.Once());
        queryResponseHandlerMock.Verify(AssociationExpression(parameterNames[2], arguments[2]), Times.Once());
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
