using ArlequimPetShop.Domain.Users;
using ArlequimPetShop.Domain.Users.Services;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Linq;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    /// <summary>
    /// Repositório NHibernate para operações relacionadas a usuários.
    /// </summary>
    public class UserNHRpository : EventBusRepository<User>, IUserRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de <see cref="UserNHRpository"/>.
        /// </summary>
        /// <param name="configuration">Instância de configuração da aplicação.</param>
        /// <param name="sessionManager">Gerenciador de UnitOfWork para NHibernate.</param>
        /// <param name="serviceProvider">Provider para resolução de dependências.</param>
        public UserNHRpository(
            IConfiguration configuration,
            IUnitOfWorkFactory<ISession> sessionManager,
            IServiceProvider serviceProvider)
            : base(configuration, sessionManager, serviceProvider)
        {
        }

        /// <summary>
        /// Verifica se existe um usuário com as credenciais informadas.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <param name="password">Senha do usuário.</param>
        /// <returns>Verdadeiro se as credenciais forem válidas, falso caso contrário.</returns>
        public async Task<bool> IsCredentialsValid(string email, string? password)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;

            return await session.Query<User>()
                                .Where(u => u.Email == email && u.Password == password)
                                .AnyAsync();
        }

        /// <summary>
        /// Verifica se já existe um usuário cadastrado com o email informado.
        /// </summary>
        /// <param name="email">Email a ser verificado.</param>
        /// <returns>Verdadeiro se o usuário existir, falso caso contrário.</returns>
        public async Task<bool> HasUserByEmail(string email)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;

            return await session.Query<User>()
                                .Where(u => u.Email == email)
                                .AnyAsync();
        }

        /// <summary>
        /// Obtém um usuário pelo seu email.
        /// </summary>
        /// <param name="email">Email do usuário.</param>
        /// <returns>Instância de <see cref="User"/> se encontrado; caso contrário, null.</returns>
        public async Task<User> GetAsyncByEmail(string email)
        {
            using var unitOfWork = UnitOfWorkFactory.Get();
            var session = unitOfWork.Context;

            return await session.Query<User>()
                                .Where(u => u.Email == email)
                                .FirstOrDefaultAsync();
        }
    }
}