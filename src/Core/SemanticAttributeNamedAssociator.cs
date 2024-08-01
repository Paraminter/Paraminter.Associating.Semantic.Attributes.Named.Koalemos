namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Common;

using System;

/// <summary>Associates semantic named attribute arguments.</summary>
public sealed class SemanticAttributeNamedAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>>
{
    private readonly ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand> Recorder;

    /// <summary>Instantiates a <see cref="SemanticAttributeNamedAssociator"/>, associating semantic named attribute arguments.</summary>
    /// <param name="recorder">Records associated semantic named attribute arguments.</param>
    public SemanticAttributeNamedAssociator(
        ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand> recorder)
    {
        Recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
    }

    void ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>>.Handle(
        IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData> command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        foreach (var association in command.Data.Associations)
        {
            var parameterName = association.Key;
            var argument = association.Value;

            var recordCommand = new RecordSemanticAttributeNamedAssociationCommand(parameterName, argument);

            Recorder.Handle(recordCommand);
        }
    }
}
