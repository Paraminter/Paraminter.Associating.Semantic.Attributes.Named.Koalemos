namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Collectors;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        SemanticAttributeNamedAssociator sut = new();

        return new Fixture(sut);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector> Sut;

        public Fixture(
            IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector> sut)
        {
            Sut = sut;
        }

        IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector> IFixture.Sut => Sut;
    }
}
