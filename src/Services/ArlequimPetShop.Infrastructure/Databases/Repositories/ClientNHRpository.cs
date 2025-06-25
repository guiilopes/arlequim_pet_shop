using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.Domain.Clients.Services;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    public class ClientNHRpository : EventBusRepository<Client>, IClientRepository
    {
        public ClientNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }

        public async Task<Client> GetAsyncByDocument(string? document)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<Client>()
                                     .Where(c => c.Document == document)
                                     .FirstOrDefaultAsync();

            return query;
        }
    }
}