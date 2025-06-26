using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade Product via Fluent NHibernate.
    /// </summary>
    public class ProductMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade Product pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void ProductMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new ProductMap());
        }
    }
}