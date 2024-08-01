namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>> RecorderMock { get; }
}
