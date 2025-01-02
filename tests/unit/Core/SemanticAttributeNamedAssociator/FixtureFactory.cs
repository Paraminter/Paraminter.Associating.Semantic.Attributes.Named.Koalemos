namespace Paraminter.Associating.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Associating.Commands;
using Paraminter.Associating.Semantic.Attributes.Named.Koalemos.Models;
using Paraminter.Pairing.Commands;
using Paraminter.Parameters.Named.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> pairerMock = new();

        SemanticAttributeNamedAssociator sut = new(pairerMock.Object);

        return new Fixture(sut, pairerMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> PairerMock;

        public Fixture(
            ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> sut,
            Mock<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> pairerMock)
        {
            Sut = sut;

            PairerMock = pairerMock;
        }

        ICommandHandler<IAssociateArgumentsCommand<IAssociateSemanticAttributeNamedArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IPairArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> IFixture.PairerMock => PairerMock;
    }
}
