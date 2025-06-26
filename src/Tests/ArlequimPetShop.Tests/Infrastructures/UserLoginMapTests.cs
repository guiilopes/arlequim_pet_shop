using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade UserLogin via Fluent NHibernate.
    /// </summary>
    public class UserLoginMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade UserLogin pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void UserLogingMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new UserLoginMap());
        }
    }
}