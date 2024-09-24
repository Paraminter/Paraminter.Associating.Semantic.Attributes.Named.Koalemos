namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Commands;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

internal sealed class PairArgumentCommand
    : IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>
{
    private readonly INamedParameter Parameter;
    private readonly ISemanticAttributeNamedArgumentData ArgumentData;

    public PairArgumentCommand(
        INamedParameter parameter,
        ISemanticAttributeNamedArgumentData argumentData)
    {
        Parameter = parameter;
        ArgumentData = argumentData;
    }

    INamedParameter IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.Parameter => Parameter;
    ISemanticAttributeNamedArgumentData IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>.ArgumentData => ArgumentData;
}
