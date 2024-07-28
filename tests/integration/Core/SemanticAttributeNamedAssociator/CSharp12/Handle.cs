namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Collectors;

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

        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Associations).Returns(associations);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(associations[0].Key, associations[0].Value), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(associations[1].Key, associations[1].Value), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(associations[2].Key, associations[2].Value), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(It.IsAny<string>(), It.IsAny<TypedConstant>()), Times.Exactly(3));
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData> query,
        IAssociateSemanticAttributeNamedQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
