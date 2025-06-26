using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade User via Fluent NHibernate.
    /// </summary>
    public class UserMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade User pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void UserMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new UserMap());
        }
    }
}