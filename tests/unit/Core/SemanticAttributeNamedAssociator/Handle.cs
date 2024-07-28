namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Moq;

using Paraminter.Associators.Queries;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Collectors;

using System;
using System.Collections.Generic;

using Xunit;

public sealed class Handle
{
    private readonly IFixture Fixture = FixtureFactory.Create();

    [Fact]
    public void NullQuery_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(null!, Mock.Of<IAssociateSemanticAttributeNamedQueryResponseCollector>()));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NullQueryResponseCollector_ThrowsArgumentNullException()
    {
        var result = Record.Exception(() => Target(Mock.Of<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>>(), null!));

        Assert.IsType<ArgumentNullException>(result);
    }

    [Fact]
    public void NoAssociations_AddsNone()
    {
        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup(static (query) => query.Data.Associations).Returns([]);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify(static (collector) => collector.Associations.Add(It.IsAny<string>(), It.IsAny<TypedConstant>()), Times.Never());
    }

    [Fact]
    public void SomeAssociations_AddsAllPairwise()
    {
        var parameter1 = "Name1";
        var parameter2 = "Name2";

        var argument1 = TypedConstantStore.GetNext();
        var argument2 = TypedConstantStore.GetNext();

        var association1 = new KeyValuePair<string, TypedConstant>(parameter1, argument1);
        var association2 = new KeyValuePair<string, TypedConstant>(parameter2, argument2);

        Mock<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>> queryMock = new();
        Mock<IAssociateSemanticAttributeNamedQueryResponseCollector> queryResponseCollectorMock = new() { DefaultValue = DefaultValue.Mock };

        queryMock.Setup((query) => query.Data.Associations).Returns([association1, association2]);

        Target(queryMock.Object, queryResponseCollectorMock.Object);

        queryResponseCollectorMock.Verify(static (collector) => collector.Associations.Add(It.IsAny<string>(), It.IsAny<TypedConstant>()), Times.Exactly(2));
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(parameter1, argument1), Times.Once());
        queryResponseCollectorMock.Verify((collector) => collector.Associations.Add(parameter2, argument2), Times.Once());
    }

    private void Target(
        IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData> query,
        IAssociateSemanticAttributeNamedQueryResponseCollector queryResponseCollector)
    {
        Fixture.Sut.Handle(query, queryResponseCollector);
    }
}
