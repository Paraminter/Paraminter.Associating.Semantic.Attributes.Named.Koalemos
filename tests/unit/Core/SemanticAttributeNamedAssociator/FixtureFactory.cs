namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Associators.Commands;
using Paraminter.Commands.Handlers;
using Paraminter.Semantic.Attributes.Named.Commands;
using Paraminter.Semantic.Attributes.Named.Koalemos.Commands;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>> recorderMock = new();

        SemanticAttributeNamedAssociator sut = new(recorderMock.Object);

        return new Fixture(sut, recorderMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> Sut;

        private readonly Mock<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>> RecorderMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> sut,
            Mock<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>> recorderMock)
        {
            Sut = sut;

            RecorderMock = recorderMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IRecordSemanticAttributeNamedAssociationCommand>> IFixture.RecorderMock => RecorderMock;
    }
}
