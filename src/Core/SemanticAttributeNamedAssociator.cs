namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;
using Paraminter.Cqs.Handlers;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

using System;

/// <summary>Associates semantic named attribute arguments with parameters.</summary>
public sealed class SemanticAttributeNamedAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>>
{
    private readonly ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> Pairer;

    /// <summary>Instantiates an associator of semantic named attribute arguments with parameters.</summary>
    /// <param name="pairer">Pairs semantic named attribute arguments with parameters.</param>
    public SemanticAttributeNamedAssociator(
        ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> pairer)
    {
        Pairer = pairer ?? throw new ArgumentNullException(nameof(pairer));
    }

    void ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        foreach (var association in command.Data.Associations)
        {
            PairArgument(association.Key, association.Value);
        }
    }

    private void PairArgument(
        string parameterName,
        TypedConstant argument)
    {
        var parameter = new NamedParameter(parameterName);
        var argumentData = new SemanticAttributeNamedArgumentData(argument);

        var command = new PairArgumentCommand(parameter, argumentData);

        Pairer.Handle(command);
    }
}
