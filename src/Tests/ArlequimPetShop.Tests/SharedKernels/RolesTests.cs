using ArlequimPetShop.SharedKernel;

namespace ArlequimPetShop.Tests.SharedKernels
{
    /// <summary>
    /// Testes unitários para a classe <see cref="Roles"/>, validando os papéis padrão definidos no sistema.
    /// </summary>
    [TestFixture]
    public class RolesTests
    {
        /// <summary>
        /// Garante que o valor da constante <see cref="Roles.Admin"/> seja "Administrador".
        /// </summary>
        [Test]
        public void AdminRole_ShouldBeAdministrador()
        {
            var adminRole = Roles.Admin;

            Assert.AreEqual("Administrador", adminRole);
        }

        /// <summary>
        /// Garante que o valor da constante <see cref="Roles.Seller"/> seja "Vendedor".
        /// </summary>
        [Test]
        public void SellerRole_ShouldBeVendedor()
        {
            var sellerRole = Roles.Seller;

            Assert.AreEqual("Vendedor", sellerRole);
        }
    }
}