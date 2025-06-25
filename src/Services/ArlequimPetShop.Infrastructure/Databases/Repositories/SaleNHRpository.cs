using ArlequimPetShop.Domain.Sales;
using ArlequimPetShop.Domain.Sales.Services;
using Microsoft.Extensions.Configuration;
using NHibernate;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    public class SaleNHRpository : EventBusRepository<Sale>, ISaleRepository
    {
        public SaleNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }
    }
}