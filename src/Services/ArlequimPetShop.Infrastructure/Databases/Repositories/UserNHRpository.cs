using ArlequimPetShop.Domain.Users;
using ArlequimPetShop.Domain.Users.Services;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    public class UserNHRpository : EventBusRepository<User>, IUserRepository
    {
        public UserNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }

        public async Task<bool> IsCredentialsValid(string email, string? password)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<User>()
                                     .Where(u => u.Email == email
                                              && u.Password == password)
                                     .AnyAsync();

            return query;
        }

        public async Task<bool> HasUserByEmail(string email)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<User>()
                                     .Where(u => u.Email == email)
                                     .AnyAsync();

            return query;
        }

        public async Task<User> GetAsyncByEmail(string email)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;
            var query = await session.Query<User>()
                                     .Where(u => u.Email == email)
                                     .FirstOrDefaultAsync();

            return query;
        }
    }
}