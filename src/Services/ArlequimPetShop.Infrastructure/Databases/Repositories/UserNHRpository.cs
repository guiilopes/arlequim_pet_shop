using ArlequimPetShop.Domain.Users;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using SrShut.Common;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    public class UserNHRpository : EventBusRepository<User>, IUserRepository
    {
        public UserNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }

        public async Task<bool> HasUserByEmail(string email)
        {
            Throw.ArgumentIsNull(email);

            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<User>()
                                     .Where(a => a.Email == email)
                                     .AnyAsync();

            return query;
        }
    }
}