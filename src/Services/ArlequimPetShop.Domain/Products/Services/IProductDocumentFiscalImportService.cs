namespace ArlequimPetShop.Domain.Products.Services
{
    public interface IProductDocumentFiscalImportService
    {
        Task Execute(Stream stream);
    }
}