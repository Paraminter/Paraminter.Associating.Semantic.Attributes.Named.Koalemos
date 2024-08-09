namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Recorders.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> recorderMock = new();

        SemanticAttributeNamedAssociator sut = new(recorderMock.Object);

        return new Fixture(sut, recorderMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> Sut;

        private readonly Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> RecorderMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> sut,
            Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> recorderMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordArgumentAssociationCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> IFixture.RecorderMock => RecorderMock;
    }
}
