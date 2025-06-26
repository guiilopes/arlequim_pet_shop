using ArlequimPetShop.Domain.Products;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Constructor_ShouldInitializeProperties()
        {
            var id = Guid.NewGuid();
            var product = new Product(id, "123", "Ração", "Ração de peixe", 10.5m, DateTime.Today);

            Assert.AreEqual(id, product.Id);
            Assert.AreEqual("123", product.Barcode);
            Assert.AreEqual("Ração", product.Name);
            Assert.AreEqual("Ração de peixe", product.Description);
            Assert.AreEqual(10.5m, product.Price);
            Assert.AreEqual(DateTime.Today, product.ExpirationDate);
            Assert.IsNotNull(product.Stocks);
            Assert.IsNotNull(product.Histories);
        }

        [Test]
        public void AddStock_ShouldAddNewStock()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddStock(5);

            Assert.AreEqual(1, product.Stocks.Count);
            Assert.AreEqual(5, product.Stocks[0].Quantity);
        }

        [Test]
        public void UpdateStock_ShouldUpdateStock()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddStock(5);
            product.UpdateStock(10);

            Assert.AreEqual(10, product.Stocks[0].Quantity);
        }

        [Test]
        public void RemoveStock_ShouldDecreaseQuantity()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddStock(10);
            product.RemoveStock(4);

            Assert.AreEqual(6, product.Stocks[0].Quantity);
        }

        [Test]
        public void AddHistory_ShouldCreateHistoryRecord()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddHistory("Ração", "Nova Desc", 2, "1234");

            Assert.AreEqual(1, product.Histories.Count);
            Assert.IsTrue(product.Histories[0].Description.Contains("descrição"));
        }

        [Test]
        public void Update_ShouldChangeProperties()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.Update("321", "NovoNome", "NovaDesc", 15m, DateTime.Today.AddDays(5));

            Assert.AreEqual("321", product.Barcode);
            Assert.AreEqual("NovoNome", product.Name);
            Assert.AreEqual("NovaDesc", product.Description);
            Assert.AreEqual(15m, product.Price);
        }

        [Test]
        public void Delete_ShouldSetDeletedOn()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.Delete();

            Assert.IsNotNull(product.DeletedOn);
        }

        [Test]
        public void HasSufficientStock_ShouldReturnFalse_WhenInsufficient()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddStock(2);

            Assert.IsTrue(product.HasSufficientStock(5));
        }

        [Test]
        public void HasSufficientStock_ShouldReturnTrue_WhenSufficient()
        {
            var product = new Product(Guid.NewGuid(), "123", "Ração", "Desc", 10m, DateTime.Today);
            product.AddStock(10);

            Assert.IsFalse(product.HasSufficientStock(5));
        }
    }
}