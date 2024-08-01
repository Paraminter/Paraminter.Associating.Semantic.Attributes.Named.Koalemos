namespace Paraminter.Semantic.Attributes.Named.Koalemos.Common;

using Microsoft.CodeAnalysis;

using Paraminter.Semantic.Attributes.Named.Commands;

internal sealed class RecordSemanticAttributeNamedAssociationCommand
    : IRecordSemanticAttributeNamedAssociationCommand
{
    private readonly string ParameterName;
    private readonly TypedConstant Argument;

    public RecordSemanticAttributeNamedAssociationCommand(
        string parameterName,
        TypedConstant argument)
    {
        ParameterName = parameterName;
        Argument = argument;
    }

    string IRecordSemanticAttributeNamedAssociationCommand.ParameterName => ParameterName;
    TypedConstant IRecordSemanticAttributeNamedAssociationCommand.Argument => Argument;
}
