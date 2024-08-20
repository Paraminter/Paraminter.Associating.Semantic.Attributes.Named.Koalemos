namespace Paraminter.Semantic.Attributes.Named.Koalemos;

using Moq;

using Paraminter.Arguments.Semantic.Attributes.Named.Models;
using Paraminter.Commands;
using Paraminter.Cqs.Handlers;
using Paraminter.Parameters.Named.Models;
using Paraminter.Semantic.Attributes.Named.Koalemos.Models;

internal static class FixtureFactory
{
    public static IFixture Create()
    {
        Mock<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> individualAssociatorMock = new();

        SemanticAttributeNamedAssociator sut = new(individualAssociatorMock.Object);

        return new Fixture(sut, individualAssociatorMock);
    }

    private sealed class Fixture
        : IFixture
    {
        private readonly ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> Sut;

        private readonly Mock<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> IndividualAssociatorMock;

        public Fixture(
            ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> sut,
            Mock<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> individualAssociatorMock)
        {
            Sut = sut;

            IndividualAssociatorMock = individualAssociatorMock;
        }

        ICommandHandler<IAssociateAllArgumentsCommand<IAssociateAllSemanticAttributeNamedArgumentsData>> IFixture.Sut => Sut;

        Mock<ICommandHandler<IAssociateSingleArgumentCommand<INamedParameter, ISemanticAttributeNamedArgumentData>>> IFixture.IndividualAssociatorMock => IndividualAssociatorMock;
    }
}
