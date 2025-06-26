using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade SaleProduct via Fluent NHibernate.
    /// </summary>
    public class SaleProductMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade SaleProduct pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void SaleProductMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new SaleProductMap());
        }
    }
}