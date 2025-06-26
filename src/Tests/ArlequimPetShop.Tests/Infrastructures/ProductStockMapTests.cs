using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade ProductStock via Fluent NHibernate.
    /// </summary>
    public class ProductStockMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade ProductStock pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void ProductStockMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new ProductStockMap());
        }
    }
}