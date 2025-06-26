using SrShut.Data;

namespace ArlequimPetShop.Domain.Products.Services
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetAsyncByBarcode(string? barcode);

        Task<bool> HasByBarcode(string barcode);
    }
}