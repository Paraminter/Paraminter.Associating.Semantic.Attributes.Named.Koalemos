namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

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

        Mock<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns(attribute.NamedArguments);

        Target(commandMock.Object);

        Fixture.IndividualAssociatorMock.Verify(static (associator) => associator.Handle(It.IsAny<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>()), Times.Exactly(3));
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualCommand(parameterNames[0], arguments[0]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualCommand(parameterNames[1], arguments[1]), Times.Once());
        Fixture.IndividualAssociatorMock.Verify(AssociateIndividualCommand(parameterNames[2], arguments[2]), Times.Once());
    }

    private static Expression<Action<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>>> AssociateIndividualCommand(
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
