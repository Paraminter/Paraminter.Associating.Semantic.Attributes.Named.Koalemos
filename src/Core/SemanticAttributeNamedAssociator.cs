namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

using System;

/// <summary>Associates semantic named attribute arguments.</summary>
public sealed class SemanticAttributeNamedAssociator
    : ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>>
{
    private readonly ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> Recorder;

    /// <summary>Instantiates a <see cref="SemanticAttributeNamedAssociator"/>, associating semantic named attribute arguments.</summary>
    /// <param name="recorder">Records associated semantic named attribute arguments.</param>
    public SemanticAttributeNamedAssociator(
        ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>> recorder)
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
            var parameter = new NamedParameter(association.Key);
            var argumentData = new SemanticAttributeNamedArgumentData(association.Value);

            var recordCommand = new RecordSemanticAttributeNamedAssociationCommand(parameter, argumentData);

            Recorder.Handle(recordCommand);
        }
    }
}
