using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade ProductHistory via Fluent NHibernate.
    /// </summary>
    public class ProductHistoryMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade ProductHistory pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void ProductHistoryMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new ProductHistoryMap());
        }
    }
}