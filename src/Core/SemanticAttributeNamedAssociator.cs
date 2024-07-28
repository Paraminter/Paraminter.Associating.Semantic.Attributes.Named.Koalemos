namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Collectors;

using System;

/// <summary>Associates semantic named attribute arguments.</summary>
public sealed class SemanticAttributeNamedAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector>
{
    /// <summary>Instantiates a <see cref="SemanticAttributeNamedAssociator"/>, associating semantic named attribute arguments.</summary>
    public SemanticAttributeNamedAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseCollector>.Handle(
        IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData> query,
        IAssociateSemanticAttributeNamedQueryResponseCollector queryResponseCollector)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseCollector is null)
        {
            throw new ArgumentNullException(nameof(queryResponseCollector));
        }

        foreach (var association in query.Data.Associations)
        {
            var parameter = association.Key;
            var argumentData = association.Value;

            queryResponseCollector.Associations.Add(parameter, argumentData);
        }
    }
}
