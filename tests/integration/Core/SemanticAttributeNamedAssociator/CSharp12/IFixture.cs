namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> RecorderMock { get; }
}
