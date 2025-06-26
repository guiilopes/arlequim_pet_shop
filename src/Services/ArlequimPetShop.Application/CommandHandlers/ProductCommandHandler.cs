using ArlequimPetShop.Contracts.Commands.Products;
using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using SrShut.Common;
using SrShut.Cqrs.Commands;
using SrShut.Data;

namespace ArlequimPetShop.Application.CommandHandlers
{
    /// <summary>
    /// Manipulador de comandos relacionados a produtos.
    /// Responsável por orquestrar ações de criação, atualização, exclusão,
    /// importação de estoque via planilha e importação via XML fiscal.
    /// </summary>
    public class ProductCommandHandler : ICommandHandler<ProductCreateCommand>,
                                         ICommandHandler<ProductUpdateCommand>,
                                         ICommandHandler<ProductDeleteCommand>,
                                         ICommandHandler<ProductStockInventoryCommand>,
                                         ICommandHandler<ProductDocumentFiscalImportCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWorkFactory _uofwFactory;
        private readonly IProductStockInventoryService _productStockInventoryService;
        private readonly IProductDocumentFiscalImportService _productDocumentFiscalImportService;

        /// <summary>
        /// Inicializa o manipulador com as dependências necessárias.
        /// </summary>
        public ProductCommandHandler(IProductRepository productRepository, IUnitOfWorkFactory uofwFactory, IProductStockInventoryService productStockInventoryService, IProductDocumentFiscalImportService productDocumentFiscalImportService)
        {
            Throw.ArgumentIsNull(productRepository);
            Throw.ArgumentIsNull(uofwFactory);
            Throw.ArgumentIsNull(productStockInventoryService);
            Throw.ArgumentIsNull(productDocumentFiscalImportService);

            _productRepository = productRepository;
            _uofwFactory = uofwFactory;
            _productStockInventoryService = productStockInventoryService;
            _productDocumentFiscalImportService = productDocumentFiscalImportService;
        }

        /// <summary>
        /// Manipula o comando de criação de produto.
        /// </summary>
        public async Task HandleAsync(ProductCreateCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsTrue(await _productRepository.HasByBarcode(command.Barcode), "Product.HasByNameOrDescription", "Produto já cadastrado com esse código de barras.");

            using var scope = _uofwFactory.Get();

            var product = new Product(command.Id.Value, command.Barcode, command.Name, command.Description, command.Price, command.ExpirationDate);
            product.UpdateStock();

            await _productRepository.AddAsync(product);

            scope.Complete();
        }

        /// <summary>
        /// Manipula o comando de atualização de produto.
        /// </summary>
        public async Task HandleAsync(ProductUpdateCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();

            var product = await _productRepository.GetAsyncById(command.Id);
            Throw.IsNull(product, "Product.NotFound", "Produto não encontrado");

            product.AddHistory(command.Name, command.Description, 0, null);
            product.Update(command.Name, command.Description, command.Price, command.ExpirationDate);

            scope.Complete();
        }

        /// <summary>
        /// Manipula o comando de exclusão lógica de produto.
        /// </summary>
        public async Task HandleAsync(ProductDeleteCommand command)
        {
            Throw.ArgumentIsNull(command);

            using var scope = _uofwFactory.Get();

            var product = await _productRepository.GetAsyncById(command.Id);
            Throw.IsNull(product, "Product.NotFound", "Produto não encontrado");

            product.Delete();
            scope.Complete();
        }

        /// <summary>
        /// Manipula o comando de importação de estoque via planilha CSV.
        /// </summary>
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

        /// <summary>
        /// Manipula o comando de importação de produtos via XML de nota fiscal.
        /// </summary>
        public async Task HandleAsync(ProductDocumentFiscalImportCommand command)
        {
            Throw.ArgumentIsNull(command);
            Throw.IsNull(command.File, "ProductStockInventoryCommand.File", "Arquivo não encontrado.");

            using var scope = _uofwFactory.Get();

            var allowedExtensions = command.File.FileName.Contains(".xml");
            Throw.IsFalse(allowedExtensions, "OperationStockImport.InvalidExtension", "Tipo de arquivo inválido.");

            var stream = new MemoryStream();
            await command.File.CopyToAsync(stream);
            stream.Position = 0;

            await _productDocumentFiscalImportService.Execute(stream);
            scope.Complete();
        }
    }
}