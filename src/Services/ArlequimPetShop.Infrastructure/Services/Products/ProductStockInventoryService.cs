using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Dtos;
using ArlequimPetShop.Domain.Products.Services;
using CsvHelper;
using CsvHelper.Configuration;
using SrShut.Common;
using System.Globalization;

namespace ArlequimPetShop.Infrastructure.Services.Products
{
    public class ProductStockInventoryService : IProductStockInventoryService
    {
        private readonly IProductRepository _productRepository;

        public ProductStockInventoryService(IProductRepository productRepository)
        {
            Throw.ArgumentIsNull(productRepository);

            _productRepository = productRepository;
        }

        public async Task Execute(Stream stream)
        {
            Throw.ArgumentIsNull(stream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, config))
            {
                var dtos = csv.GetRecords<ProductStockInventoryDto>();

                foreach (var dto in dtos.ToList().GroupBy(d => d.Barcode).ToList())
                {
                    var product = await _productRepository.GetAsyncByBarcode(dto.Key);
                    var item = dto.First();
                    var quantity = dto.Sum(i => i.Quantity);

                    if (product == null)
                    {
                        product = new Product(Guid.NewGuid(), item.Barcode, item.Name, item.Description, item.Price, item.ExpirationDate);
                        await _productRepository.AddAsync(product);
                    }

                    product.AddHistory(item.Name, item.Description, item.Quantity, string.Empty);
                    product.UpdateStock(quantity);

                    await _productRepository.UpdateAsync(product);
                }
            }
        }
    }
}