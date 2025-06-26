using ArlequimPetShop.Domain.Products;

namespace ArlequimPetShop.Tests.Domains
{
    [TestFixture]
    public class ProductStockTests
    {
        private Product _product;

        [SetUp]
        public void Setup()
        {
            _product = new Product
            {
                Price = 100.0m,
                Name = "Produto Teste",
                Barcode = "123456789",
                Description = "Descrição"
            };
        }

        [Test]
        public void Constructor_ShouldInitializeWithQuantity()
        {
            var stock = new ProductStock(_product, 10);

            Assert.AreEqual(10, stock.Quantity);
            Assert.NotNull(stock.CreatedOn);
            Assert.AreEqual(stock.CreatedOn, stock.UpdatedOn);
            Assert.AreEqual(_product, stock.Product);
        }

        [Test]
        public void Add_ShouldIncreaseQuantityAndUpdateTimestamp()
        {
            var stock = new ProductStock(_product, 5);
            var initialTime = stock.UpdatedOn;

            stock.Add(3);

            Assert.AreEqual(8, stock.Quantity);
            Assert.Greater(stock.UpdatedOn, initialTime);
        }

        [Test]
        public void Update_ShouldSetQuantityAndUpdateTimestamp()
        {
            var stock = new ProductStock(_product, 5);
            var initialTime = stock.UpdatedOn;

            stock.Update(15);

            Assert.AreEqual(15, stock.Quantity);
            Assert.Greater(stock.UpdatedOn, initialTime);
        }

        [Test]
        public void Remove_ShouldDecreaseQuantityAndUpdateTimestamp()
        {
            var stock = new ProductStock(_product, 10);
            var initialTime = stock.UpdatedOn;

            stock.Remove(4);

            Assert.AreEqual(6, stock.Quantity);
            Assert.Greater(stock.UpdatedOn, initialTime);
        }

        [Test]
        public void Delete_ShouldSetDeletedOn()
        {
            var stock = new ProductStock(_product, 10);
            Assert.IsNull(stock.DeletedOn);

            stock.Delete();

            Assert.IsNotNull(stock.DeletedOn);
            Assert.LessOrEqual((DateTime.Now - stock.DeletedOn.Value).TotalSeconds, 1);
        }
    }
}