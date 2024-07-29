namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Associators.Queries;
using Paraminter.Queries.Handlers;
using Paraminter.Semantic.Attributes.Named.Koalemos.Common;
using Paraminter.Semantic.Attributes.Named.Koalemos.Queries;
using Paraminter.Semantic.Attributes.Named.Queries.Handlers;

using System;

/// <summary>Associates semantic named attribute arguments.</summary>
public sealed class SemanticAttributeNamedAssociator
    : IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseHandler>
{
    /// <summary>Instantiates a <see cref="SemanticAttributeNamedAssociator"/>, associating semantic named attribute arguments.</summary>
    public SemanticAttributeNamedAssociator() { }

    void IQueryHandler<IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData>, IAssociateSemanticAttributeNamedQueryResponseHandler>.Handle(
        IAssociateArgumentsQuery<IAssociateSemanticAttributeNamedData> query,
        IAssociateSemanticAttributeNamedQueryResponseHandler queryResponseHandler)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        if (queryResponseHandler is null)
        {
            throw new ArgumentNullException(nameof(queryResponseHandler));
        }

        foreach (var association in query.Data.Associations)
        {
            var parameterName = association.Key;
            var argument = association.Value;

            var command = new AddSemanticAttributeNamedAssociationCommand(parameterName, argument);

            queryResponseHandler.AssociationCollector.Handle(command);
        }
    }
}
