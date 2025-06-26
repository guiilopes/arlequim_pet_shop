using ArlequimPetShop.Infrastructure.Databases.Mappings;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para validar o mapeamento da entidade Sale via Fluent NHibernate.
    /// </summary>
    public class SaleMapTests
    {
        /// <summary>
        /// Verifica se o mapeamento da entidade Sale pode ser instanciado sem lançar exceções.
        /// </summary>
        [Test]
        public void SaleMap_ShouldInstantiateWithoutException()
        {
            Assert.DoesNotThrow(() => new SaleMap());
        }
    }
}