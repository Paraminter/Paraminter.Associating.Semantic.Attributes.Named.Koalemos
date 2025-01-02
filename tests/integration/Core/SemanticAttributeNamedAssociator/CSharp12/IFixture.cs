namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> Sut { get; }

    public abstract Mock<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> PairerMock { get; }
}
