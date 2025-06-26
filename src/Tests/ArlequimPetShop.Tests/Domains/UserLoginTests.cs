using ArlequimPetShop.Domain.Users;

namespace ArlequimPetShop.Tests.Domains
{
    /// <summary>
    /// Testes unitários para a entidade <see cref="UserLogin"/>.
    /// </summary>
    public class UserLoginTests
    {
        /// <summary>
        /// Verifica se o construtor preenche os campos obrigatórios corretamente.
        /// </summary>
        [Test]
        public void Constructor_ShouldInitializeWithUserAndEmail()
        {
            // Arrange
            var user = new User();
            var email = "user@email.com";

            // Act
            var login = new UserLogin(user, email);

            // Assert
            Assert.AreEqual(user, login.User);
            Assert.AreEqual(email, login.Email);
            Assert.That(login.CreatedOn, Is.EqualTo(login.UpdatedOn).Within(TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Verifica se o construtor padrão pode ser chamado sem erro.
        /// </summary>
        [Test]
        public void ParameterlessConstructor_ShouldCreateEmptyInstance()
        {
            // Act
            var login = new UserLogin();

            // Assert
            Assert.IsNotNull(login);
        }
    }
}