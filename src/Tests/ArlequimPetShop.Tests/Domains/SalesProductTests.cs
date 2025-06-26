using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Sales;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class SaleProductTests
    {
        /// <summary>
        /// Deve instanciar corretamente um SaleProduct com valores válidos.
        /// </summary>
        [Test]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            var sale = new Sale();
            var product = new Product { Price = 10.0m };
            var quantity = 2.0m;
            var discount = 1.0m;
            var netPrice = 19.0m;

            var saleProduct = new SaleProduct(sale, product, quantity, discount, netPrice);

            Assert.AreEqual(sale, saleProduct.Sale);
            Assert.AreEqual(product, saleProduct.Product);
            Assert.AreEqual(quantity, saleProduct.Quantity);
            Assert.AreEqual(10.0m, saleProduct.Price);
            Assert.AreEqual(discount, saleProduct.Discount);
            Assert.AreEqual(netPrice, saleProduct.NetPrice);
            Assert.That(saleProduct.CreatedOn, Is.EqualTo(saleProduct.UpdatedOn).Within(TimeSpan.FromSeconds(1)));
        }

        /// <summary>
        /// Deve lançar exceção se quantidade for nula.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrow_WhenQuantityIsNull()
        {
            var sale = new Sale();
            var product = new Product { Price = 10.0m };
            decimal? quantity = null;

            Assert.Throws<BusinessException>(() => new SaleProduct(sale, product, quantity, 1.0m, 9.0m));
        }

        /// <summary>
        /// Deve lançar exceção se desconto for nulo.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrow_WhenDiscountIsNull()
        {
            var sale = new Sale();
            var product = new Product { Price = 10.0m };
            decimal? discount = null;

            Assert.Throws<BusinessException>(() => new SaleProduct(sale, product, 1.0m, discount, 9.0m));
        }

        /// <summary>
        /// Deve lançar exceção se preço líquido for nulo.
        /// </summary>
        [Test]
        public void Constructor_ShouldThrow_WhenNetPriceIsNull()
        {
            var sale = new Sale();
            var product = new Product { Price = 10.0m };
            decimal? netPrice = null;

            Assert.Throws<BusinessException>(() => new SaleProduct(sale, product, 1.0m, 1.0m, netPrice));
        }
    }
}