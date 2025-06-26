using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade Client via Fluent NHibernate.
    /// </summary>
    public class ClientMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade Client pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void ClientMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new ClientMap());
        }
    }
}