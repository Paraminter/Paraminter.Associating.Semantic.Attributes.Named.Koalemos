﻿namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

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

        Mock<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> commandMock = new();

        commandMock.Setup(static (command) => command.Data.Associations).Returns(attribute.NamedArguments);

        Target(commandMock.Object);

        Fixture.RecorderMock.Verify(static (recorder) => recorder.Handle(It.IsAny<IRecordSemanticAttributeNamedAssociationCommand>()), Times.Exactly(3));
        Fixture.RecorderMock.Verify(RecordExpression(parameterNames[0], arguments[0]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameterNames[1], arguments[1]), Times.Once());
        Fixture.RecorderMock.Verify(RecordExpression(parameterNames[2], arguments[2]), Times.Once());
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
