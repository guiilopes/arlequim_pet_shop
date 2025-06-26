using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using SrShut.Common;
using System.Xml.Serialization;

namespace ArlequimPetShop.Infrastructure.Services.Products
{
    /// <summary>
    /// Serviço responsável por importar dados fiscais de produtos a partir de um arquivo XML.
    /// </summary>
    public class ProductDocumentFiscalImportService : IProductDocumentFiscalImportService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Inicializa uma nova instância de <see cref="ProductDocumentFiscalImportService"/>.
        /// </summary>
        /// <param name="productRepository">Repositório de produtos.</param>
        public ProductDocumentFiscalImportService(IProductRepository productRepository)
        {
            Throw.ArgumentIsNull(productRepository);
            _productRepository = productRepository;
        }

        /// <summary>
        /// Executa o processo de importação de um XML fiscal contendo produtos.
        /// Caso o produto ainda não exista, ele será criado. Caso contrário, será atualizado.
        /// </summary>
        /// <param name="stream">Stream contendo o XML da nota fiscal eletrônica.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona.</returns>
        public async Task Execute(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(ProductDocumentFiscalDto));

            var documentFiscal = (ProductDocumentFiscalDto)serializer.Deserialize(stream);
            var documentFiscalNumber = documentFiscal.NFe.InfNFe.Id;

            var groupedProducts = documentFiscal.NFe.InfNFe.Det
                .GroupBy(d => d.Prod.BarCode)
                .ToList();

            foreach (var group in groupedProducts)
            {
                var barcode = group.Key;
                var quantity = group.Count();
                var item = group.First();

                var product = await _productRepository.GetAsyncByBarcode(barcode);

                if (product == null)
                {
                    product = new Product(id: Guid.NewGuid(), barcode: item.Prod.BarCode, name: item.Prod.Name, description: item.Prod.Name, price: item.Prod.Price, null);

                    await _productRepository.AddAsync(product);
                }

                product.AddHistory(item.Prod.Name, item.Prod.Name, quantity, documentFiscalNumber);
                product.AddStock(quantity);

                await _productRepository.UpdateAsync(product);
            }
        }
    }
}