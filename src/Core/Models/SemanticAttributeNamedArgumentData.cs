namespace Paraminter.Semantic.Attributes.Named.Koalemos.Models;

using Microsoft.CodeAnalysis;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;

internal sealed class SemanticAttributeNamedArgumentData
    : ISemanticAttributeNamedArgumentData
{
    private readonly TypedConstant Argument;

    public SemanticAttributeNamedArgumentData(
        TypedConstant argument)
    {
        Argument = argument;
    }

    TypedConstant ISemanticAttributeNamedArgumentData.Argument => Argument;
}
