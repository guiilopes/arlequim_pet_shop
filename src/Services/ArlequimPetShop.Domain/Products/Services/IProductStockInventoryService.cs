namespace ArlequimPetShop.Domain.Products.Services
{
    public interface IProductStockInventoryService
    {
        Task Execute(Stream stream, string documentFiscalNumber);
    }
}