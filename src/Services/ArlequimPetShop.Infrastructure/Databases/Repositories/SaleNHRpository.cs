using ArlequimPetShop.Domain.Sales;
using ArlequimPetShop.Domain.Sales.Services;
using Microsoft.Extensions.Configuration;
using NHibernate;
using SrShut.Data;
using SrShut.Nhibernate;

namespace ArlequimPetShop.Infrastructure.Databases.Repositories
{
    /// <summary>
    /// Repositório NHibernate responsável por persistência e recuperação de dados da entidade Sale (venda).
    /// Herda funcionalidades de EventBusRepository e implementa a interface de domínio ISaleRepository.
    /// </summary>
    public class SaleNHRpository : EventBusRepository<Sale>, ISaleRepository
    {
        /// <summary>
        /// Inicializa uma nova instância de SaleNHRpository.
        /// Utiliza injeção de dependência para configuração, controle de sessão NHibernate e serviços do domínio.
        /// </summary>
        /// <param name="configuration">Configuração do sistema (IConfiguration).</param>
        /// <param name="sessionManager">Gerenciador de unidade de trabalho NHibernate (IUnitOfWorkFactory).</param>
        /// <param name="serviceProvider">Provedor de serviços para injeção de dependências.</param>
        public SaleNHRpository(IConfiguration configuration, IUnitOfWorkFactory<ISession> sessionManager, IServiceProvider serviceProvider) : base(configuration, sessionManager, serviceProvider)
        {
        }
    }
}