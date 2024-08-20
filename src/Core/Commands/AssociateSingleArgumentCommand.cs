namespace Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Parameters.Named.Models;

internal sealed class AssociateSingleArgumentCommand
    : IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>
{
    private readonly INamedParameter Parameter;
    private readonly ISemanticAttributeNamedArgumentData ArgumentData;

    public AssociateSingleArgumentCommand(
        INamedParameter parameter,
        ISemanticAttributeNamedArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    INamedParameter IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.Parameter => Parameter;
    ISemanticAttributeNamedArgumentData IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.ArgumentData => ArgumentData;
}
