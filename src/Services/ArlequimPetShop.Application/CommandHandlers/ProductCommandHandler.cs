using ArlequimPetShop.Contracts.Commands.Products;
using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Data;

namespace ArlequimPetShop.Application.CommandHandlers
{
    public class ProductCommandHandler : ICommandHandler<ProductCreateCommand>,
                                         ICommandHandler<ProductUpdateCommand>,
                                         ICommandHandler<ProductDeleteCommand>,
                                         ICommandHandler<ProductStockInventoryCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;
        private readonly IProductStockInventoryService _productStockInventoryService;

        public ProductCommandHandler(IProductRepository productRepository, IUnitOfWorkFactory uofwFactory, IProductStockInventoryService productStockInventoryService)
        {
            Throw.ArgumentIsNull(productRepository);
            Throw.ArgumentIsNull(uofwFactory);
            Throw.ArgumentIsNull(productStockInventoryService);

            _productRepository = productRepository;
            _uofwFactory = uofwFactory;
            _productStockInventoryService = productStockInventoryService;
        }

        public async Task HandleAsync(ProductStockInventoryCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsNull(command.File, "ProductStockInventoryCommand.File", "Arquivo não encontrado.");

            using var scope = _uofwFactory.Get();

            var allowedExtensions = command.File.FileName.Contains(".csv");
            Throw.IsFalse(allowedExtensions, "OperationStockImport.InvalidExtension", "Tipo de arquivo inválido.");

            var stream = new MemoryStream();
            await command.File.CopyToAsync(stream);
            stream.Position = 0;

            await _productStockInventoryService.Execute(stream);

            scope.Complete();
        }

        public async Task HandleAsync(ProductCreateCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsTrue(await _productRepository.HasByNameOrDescription(command.Name, command.Description), "Product.HasByNameOrDescription", "Produto já cadastrado com esse nome ou descrição.");

            using var scope = _uofwFactory.Get();

            var product = new Product(Guid.NewGuid(), command.Barcode, command.Name, command.Description, command.Price, command.ExpirationDate);

            product.AddStock();

            await _productRepository.AddAsync(product);

            scope.Complete();
        }

        public async Task HandleAsync(ProductUpdateCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();

            var product = await _productRepository.GetAsyncById(command.Id);
            Throw.IsNull(product, "Product.NotFound", "Produto não encontrado");

            product.Update(command.Barcode, command.Name, command.Description, command.Price, command.ExpirationDate);

            scope.Complete();
        }

        public async Task HandleAsync(ProductDeleteCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();

            var product = await _productRepository.GetAsyncById(command.Id);
            Throw.IsNull(product, "Product.NotFound", "Produto não encontrado");

            product.Delete();

            scope.Complete();
        }
    }
}