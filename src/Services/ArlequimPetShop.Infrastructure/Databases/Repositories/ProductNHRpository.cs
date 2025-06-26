using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Products.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    public class ProductNHRpository : EventBusRepository<Product>, IProductRepository
    {
        public ProductNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }

        public async Task<Product> GetAsyncByBarcode(string? barcode)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<Product>()
                                     .Where(p => p.Barcode == barcode)
                                     .FirstOrDefaultAsync();

            return query;
        }

        public async Task<bool> HasByBarcode(string barcode)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<Product>()
                                     .Where(p => p.Barcode == barcode)
                                     .AnyAsync();

            return query;
        }
    }
}