namespace Paraminter.Semantic.Attributes.Named.Koalemos.Common;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Parameters.Named.Models;
using Paraminter.Recorders.Commands;

internal sealed class RecordSemanticAttributeNamedAssociationCommand
    : IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>
{
    private readonly INamedParameter Parameter;
    private readonly ISemanticAttributeNamedArgumentData ArgumentData;

    public RecordSemanticAttributeNamedAssociationCommand(
        INamedParameter parameter,
        ISemanticAttributeNamedArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    INamedParameter IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.Parameter => Parameter;
    ISemanticAttributeNamedArgumentData IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.ArgumentData => ArgumentData;
}
