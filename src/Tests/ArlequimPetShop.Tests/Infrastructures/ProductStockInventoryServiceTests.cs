using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using ArlequimPetShop.Infrastructure.Services.Products;
using Moq;
using System.Text;

namespace ArlequimPetShop.Tests.Infrastructures
{
    /// <summary>
    /// Testes para o serviço de importação de inventário de estoque de produtos via CSV.
    /// </summary>
    [TestFixture]
    public class ProductStockInventoryServiceTests
    {
        private Mock<IProductRepository>? _productRepositoryMock;
        private ProductStockInventoryService? _service;

        /// <summary>
        /// Setup executado antes de cada teste.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _service = new ProductStockInventoryService(_productRepositoryMock.Object);
        }

        /// <summary>
        /// Testa se o serviço importa produtos corretamente de um arquivo CSV e atualiza o repositório.
        /// </summary>
        [Test]
        public async Task Execute_ShouldImportCsvAndUpdateRepository()
        {
            const string csvContent = "barcode;name;description;price;quantity;expirationdate\n" +
                                      "7891234567890;Produto Teste;Descrição;10.5;2;2025-12-31";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));

            _productRepositoryMock?.Setup(r => r.GetAsyncByBarcode("7891234567890"))
                                   .ReturnsAsync((Product)null!); 

            await _service?.Execute(stream)!;

            _productRepositoryMock?.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _productRepositoryMock?.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}