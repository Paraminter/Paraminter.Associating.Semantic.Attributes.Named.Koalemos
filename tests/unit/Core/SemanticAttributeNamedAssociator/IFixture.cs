namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

internal interface IFixture
{
    public abstract ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> Sut { get; }

    public abstract Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> RecorderMock { get; }
}
