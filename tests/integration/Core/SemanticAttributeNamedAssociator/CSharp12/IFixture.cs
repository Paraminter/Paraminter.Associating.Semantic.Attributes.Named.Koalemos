namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Collectors;

internal interface IFixture
{
    public abstract IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector> Sut { get; }
}
