using ArlequimPetShop.Contracts.Commands.Sales;
using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.Domain.Clients.Services;
using ArlequimPetShop.Domain.Products.Services;
using ArlequimPetShop.Domain.Sales;
using ArlequimPetShop.Domain.Sales.Services;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Data;

namespace ArlequimPetShop.Application.CommandHandlers
{
    public class SaleCommandHandler : ICommandHandler<SaleCreateCommand>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;
        private readonly IProductStockInventoryService _productStockInventoryService;
        private readonly IProductDocumentFiscalImportService _productDocumentFiscalImportService;

        public SaleCommandHandler(IProductRepository productRepository, IUnitOfWorkFactory uofwFactory, IProductStockInventoryService productStockInventoryService, IProductDocumentFiscalImportService productDocumentFiscalImportService, ISaleRepository saleRepository, IClientRepository clientRepository)
        {
            Throw.ArgumentIsNull(saleRepository);
            Throw.ArgumentIsNull(productRepository);
            Throw.ArgumentIsNull(uofwFactory);
            Throw.ArgumentIsNull(productStockInventoryService);
            Throw.ArgumentIsNull(productDocumentFiscalImportService);
            Throw.ArgumentIsNull(clientRepository);

            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _clientRepository = clientRepository;
            _uofwFactory = uofwFactory;
            _productStockInventoryService = productStockInventoryService;
            _productDocumentFiscalImportService = productDocumentFiscalImportService;
        }

        public async Task HandleAsync(SaleCreateCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();
            var document = Document.PutMask(command.Document);
            var client = await _clientRepository.GetAsyncByDocument(document);

            if (client == null)
            {
                client = new Client(Guid.NewGuid(), command.Name, document);

                await _clientRepository.AddAsync(client);
            }
            else
            {
                await _clientRepository.UpdateAsync(client);
            }

            var sale = new Sale(command.Id, client);

            foreach (var item in command.Products)
            {
                var product = await _productRepository.GetAsyncByBarcode(item.Barcode);
                var priceWithDiscount = product.Price * (1 - item.Discount);

               Throw.IsTrue(product.HasSufficientStock(item.Quantity), "Product.Stock", $"Não há quantidade suficiente do produto; Código de barra: {product.Barcode} - Nome: {product.Name}.");
                
                sale.AddProduct(product, item.Quantity, item.Discount, priceWithDiscount);
                product.RemoveStock(item.Quantity);
            }

            await _saleRepository.AddAsync(sale);

            scope.Complete();
        }
    }
}