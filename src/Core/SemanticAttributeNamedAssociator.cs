namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

using System;

/// <summary>Associates semantic named attribute arguments with parameters.</summary>
public sealed class SemanticAttributeNamedAssociator
    : ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>>
{
    private readonly ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> IndividualAssociator;

    /// <summary>Instantiates an associator of semantic named attribute arguments with parameters.</summary>
    /// <param name="individualAssociator">Associates individual semantic named attribute arguments with parameters.</param>
    public SemanticAttributeNamedAssociator(
        ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> individualAssociator)
    {
        IndividualAssociator = individualAssociator ?? throw new ArgumentNullException(nameof(individualAssociator));
    }

    void ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>>.Handle(
        IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        foreach (var association in command.Data.Associations)
        {
            AssociateArgument(association.Key, association.Value);
        }
    }

    private void AssociateArgument(
        string parameterName,
        TypedConstant argument)
    {
        var parameter = new NamedParameter(parameterName);
        var argumentData = new SemanticAttributeNamedArgumentData(argument);

        var associateIndividualCommand = new AssociateSingleArgumentCommand(parameter, argumentData);

        IndividualAssociator.Handle(associateIndividualCommand);
    }
}
