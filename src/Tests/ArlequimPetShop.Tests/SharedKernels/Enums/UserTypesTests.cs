using ArlequimPetShop.SharedKernel.Enums;
using SrShut.Common;

namespace ArlequimPetShop.Tests.SharedKernels.Enums
{
    /// <summary>
    /// Testes para o enum UserTypes.
    /// </summary>
    [TestFixture]
    public class UserTypesTests
    {
        [TestCase(UserTypes.ADMINISTRATOR, "Administrador")]
        [TestCase(UserTypes.SELLER, "Vendedor")]
        public void UserTypes_ShouldHaveCorrectDescription(UserTypes userType, string expectedDescription)
        {
            var description = Text.EnumDescription(userType);

            Assert.AreEqual(expectedDescription, description);
        }
    }
}