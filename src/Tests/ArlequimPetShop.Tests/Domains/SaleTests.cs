using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Sales;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class SaleTests
    {
        /// <summary>
        /// Deve criar a instância de Sale com ID e cliente válidos.
        /// </summary>
        [Test]
        public void Constructor_ShouldInitializeWithValidData()
        {
            var client = new Client(Guid.NewGuid(), "Cliente Teste", "12345678900");
            var sale = new Sale(Guid.NewGuid(), client);

            Assert.NotNull(sale.Id);
            Assert.NotNull(sale.Client);
            Assert.NotNull(sale.Products);
            Assert.AreEqual(0M, sale.TotalPrice);
            Assert.That(sale.CreatedOn, Is.EqualTo(sale.UpdatedOn).Within(TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Deve adicionar produto à venda e atualizar o total corretamente.
        /// </summary>
        [Test]
        public void AddProduct_ShouldAddProductToListAndUpdateTotal()
        {
            var client = new Client(Guid.NewGuid(), "Cliente Teste", "12345678900");
            var sale = new Sale(Guid.NewGuid(), client);

            var product = new Product { Price = 100.0m };
            var quantity = 2m;
            var discount = 10.0m; // 10%
            var expectedNetPrice = 90.0m;

            sale.AddProduct(product, quantity, discount, expectedNetPrice);

            Assert.AreEqual(1, sale.Products.Count);
            Assert.AreEqual(expectedNetPrice, sale.TotalPrice);
            Assert.AreEqual(product, sale.Products.First().Product);
        }

        /// <summary>
        /// Deve adicionar múltiplos produtos e somar corretamente o TotalPrice.
        /// </summary>
        [Test]
        public void AddProduct_ShouldAccumulateTotalPrice()
        {
            var client = new Client(Guid.NewGuid(), "Cliente Teste", "12345678900");
            var sale = new Sale(Guid.NewGuid(), client);

            var product = new Product { Price = 50.0m };

            sale.AddProduct(product, 1, 0, 50.0m);
            sale.AddProduct(product, 2, 10, 90.0m); // R$45 cada

            Assert.AreEqual(2, sale.Products.Count);
            Assert.AreEqual(140.0m, sale.TotalPrice);
        }
    }
}