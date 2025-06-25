using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Dtos;
using ArlequimPetShop.Domain.Products.Services;
using SrShut.Common;
using System.Xml.Serialization;

namespace ArlequimPetShop.Infrastructure.Services.Products
{
    public class ProductDocumentFiscalImportService : IProductDocumentFiscalImportService
    {
        private readonly IProductRepository _productRepository;

        public ProductDocumentFiscalImportService(IProductRepository productRepository)
        {
            Throw.ArgumentIsNull(productRepository);

            _productRepository = productRepository;
        }

        public async Task Execute(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(ProductDocumentFiscalDto));

            // Deserializa o XML do stream para o objeto
            var documentFiscal = (ProductDocumentFiscalDto)serializer.Deserialize(stream);
            var documentFiscalNumber = documentFiscal.NFe.InfNFe.Id;

            foreach (var prod in documentFiscal.NFe.InfNFe.Det.GroupBy(d => d.Prod.BarCode).ToList())
            {
                var product = await _productRepository.GetAsyncByBarcode(prod.Key);
                var quantity = prod.Count();
                var item = prod.First();

                if (product == null)
                {
                    product = new Product(Guid.NewGuid(), item.Prod.BarCode, item.Prod.Name, item.Prod.Name, item.Prod.Price, null);
                    await _productRepository.AddAsync(product);
                }

                product.AddHistory(item.Prod.Name, item.Prod.Name, quantity, documentFiscalNumber);
                product.AddStock(quantity);

                await _productRepository.UpdateAsync(product);
            }
        }
    }
}