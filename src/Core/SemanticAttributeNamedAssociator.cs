namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

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

    async Task ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData> command,
        CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        foreach (var association in command.Data.Associations)
        {
            await PairArgument(association.Key, association.Value, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task PairArgument(
        string parameterName,
        TypedConstant argument,
        CancellationToken cancellationToken)
    {
        var parameter = new NamedParameter(parameterName);
        var argumentData = new SemanticAttributeNamedArgumentData(argument);

        var command = new PairArgumentCommand(parameter, argumentData);

        await Pairer.Handle(command, cancellationToken).ConfigureAwait(false);
    }
}
