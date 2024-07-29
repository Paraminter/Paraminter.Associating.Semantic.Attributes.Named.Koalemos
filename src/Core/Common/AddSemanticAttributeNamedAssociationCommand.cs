namespace Paraminter.Semantic.Attributes.Named.Koalemos.Common;

using Microsoft.CodeAnalysis;

using Paraminter.Semantic.Attributes.Named.Commands;

internal sealed class AddSemanticAttributeNamedAssociationCommand
    : IAddSemanticAttributeNamedAssociationCommand
{
    private readonly string ParameterName;
    private readonly TypedConstant Argument;

    public AddSemanticAttributeNamedAssociationCommand(
        string parameterName,
        TypedConstant argument)
    {
        ParameterName = parameterName;
        Argument = argument;
    }

    string IAddSemanticAttributeNamedAssociationCommand.ParameterName => ParameterName;
    TypedConstant IAddSemanticAttributeNamedAssociationCommand.Argument => Argument;
}
