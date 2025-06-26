using ArlequimPetShop.Domain.Products;
using SrShut.Common.Exceptions;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class ProductHistoryTests
    {
        private Product _product;

        [SetUp]
        public void Setup()
        {
            _product = new Product
            {
                Name = "Produto Teste",
                Barcode = "123456789",
                Description = "Produto de teste",
                Price = 100.0m
            };
        }

        [Test]
        public void Constructor_ShouldInitializeAllProperties()
        {
            var description = "Entrada de estoque";
            var quantity = 20m;
            var documentNumber = "NF123";

            var history = new ProductHistory(_product, description, quantity, documentNumber);

            Assert.AreEqual(_product, history.Product);
            Assert.AreEqual(description, history.Description);
            Assert.AreEqual(quantity, history.Quantity);
            Assert.AreEqual(documentNumber, history.DocumentFiscalNumber);
            Assert.AreEqual(history.CreatedOn, history.UpdatedOn);
            Assert.IsNull(history.DeletedOn);
        }

        [Test]
        public void Constructor_ShouldThrow_WhenQuantityIsNull()
        {
            Assert.Throws<BusinessException>(() =>
            {
                _ = new ProductHistory(_product, "Saída", null, "NF456");
            });
        }

        [Test]
        public void Constructor_ShouldThrow_WhenDocumentFiscalNumberIsNull()
        {
            Assert.Throws<BusinessException>(() =>
            {
                _ = new ProductHistory(_product, "Entrada", 5m, null);
            });
        }

        [Test]
        public void Constructor_ShouldThrow_WhenProductIsNull()
        {
            Assert.DoesNotThrow(() =>
            {
                var history = new ProductHistory(null, "Ajuste", 10, "NF789");
                Assert.IsNull(history.Product);
            });
        }
    }
}