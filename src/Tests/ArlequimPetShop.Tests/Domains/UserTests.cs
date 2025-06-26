using ArlequimPetShop.Domain.Users;
using ArlequimPetShop.SharedKernel.Enums;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Tests.Domains
{
    /// <summary>
    /// Testes unitários da entidade <see cref="User"/>.
    /// </summary>
    [TestFixture]
    public class UserTests
    {
        private const string ValidPassword = "Test@123";
        private const string SecretKey = "uV1D6J37OeqP9dq6nmW+v3QkH5x+/l1KnZwEX1vGxzw=";

        /// <summary>
        /// Verifica se o construtor cria um usuário válido com dados mínimos.
        /// </summary>
        [Test]
        public void Constructor_ShouldCreateValidUser()
        {
            // Arrange
            var id = Guid.NewGuid();
            var user = new User(id, UserTypes.ADMINISTRATOR, "Admin Guilherme", "guilherme@email.com", ValidPassword);

            // Assert
            Assert.AreEqual(id, user.Id);
            Assert.AreEqual(UserTypes.ADMINISTRATOR, user.Type);
            Assert.AreEqual("Admin Guilherme", user.Name);
            Assert.AreEqual("guilherme@email.com", user.Email);
            Assert.AreEqual(0, user.Logins.Count);
            Assert.That(user.CreatedOn, Is.EqualTo(user.UpdatedOn).Within(TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Verifica se a senha fraca lança exceção de validação.
        /// </summary>
        [Test]
        public void SetPassword_WithWeakPassword_ShouldThrowBusinessException()
        {
            var user = new User();
            var ex = Assert.Throws<BusinessException>(() => user.SetPassword("weak"));

            StringAssert.Contains("A senha deve ter pelo menos 6 caracteres", ex.Message);
        }

        /// <summary>
        /// Verifica se o token gerado no login é válido e logins são registrados.
        /// </summary>
        [Test]
        public void Login_ShouldReturnToken_AndRegisterLogin()
        {
            var user = new User(Guid.NewGuid(), UserTypes.ADMINISTRATOR, "Guilherme", "guilherme@email.com", ValidPassword);
            var token = user.Login("guilherme@email.com", SecretKey);

            StringAssert.StartsWith("Bearer ", token);
            Assert.AreEqual(1, user.Logins.Count);
            Assert.AreEqual("guilherme@email.com", user.Logins[0].Email);
        }

        /// <summary>
        /// Verifica se a atualização de nome/email/senha modifica os campos corretamente.
        /// </summary>
        [Test]
        public void Update_ShouldChangeUserInfoAndUpdateDate()
        {
            var user = new User(Guid.NewGuid(), UserTypes.ADMINISTRATOR, "Old", "old@email.com", ValidPassword);
            var beforeUpdate = user.UpdatedOn;
            user.Update("Novo Nome", "novo@email.com", "New@1234");

            Assert.AreEqual("Novo Nome", user.Name);
            Assert.AreEqual("novo@email.com", user.Email);
            Assert.That(user.UpdatedOn, Is.GreaterThan(beforeUpdate));
        }

        /// <summary>
        /// Verifica se o método Delete define DeletedOn com a data atual.
        /// </summary>
        [Test]
        public void Delete_ShouldSetDeletedOn()
        {
            var user = new User(Guid.NewGuid(), UserTypes.ADMINISTRATOR, "Guilherme", "guilherme@email.com", ValidPassword);
            user.Delete();

            Assert.IsNotNull(user.DeletedOn);
            Assert.That(user.DeletedOn.Value.Date, Is.EqualTo(DateTime.Now.Date));
        }
    }
}