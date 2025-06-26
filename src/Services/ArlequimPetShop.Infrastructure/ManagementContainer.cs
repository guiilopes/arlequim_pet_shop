using ArlequimPetShop.Application.CommandHandlers;
using ArlequimPetShop.Application.QueryHandlers;
using ArlequimPetShop.Contracts.Commands.Products;
using ArlequimPetShop.Contracts.Commands.Sales;
using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Contracts.Queries.Products;
using ArlequimPetShop.Contracts.Queries.Sales;
using ArlequimPetShop.Contracts.Queries.Users;
using ArlequimPetShop.Domain.Clients.Services;
using ArlequimPetShop.Domain.Products.Services;
using ArlequimPetShop.Domain.Sales.Services;
using ArlequimPetShop.Domain.Users.Services;
using ArlequimPetShop.Infrastructure.Databases.Mappings;
using ArlequimPetShop.Infrastructure.Databases.Repositories;
using ArlequimPetShop.Infrastructure.Services.Products;
using ArlequimPetShop.SharedKernel;
using ArlequimPetShop.SharedKernel.Options;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using SrShut.Cache;
using SrShut.Cqrs.Bus;
using SrShut.Cqrs.Bus.DependencyInjections;
using SrShut.Cqrs.Commands;
using SrShut.Cqrs.Requests;
using SrShut.Cqrs.Traces.DependencyInjection;
using SrShut.Data.ConnectionStrings;
using SrShut.Data.Nhibernate;
using SrShut.Security.Core;
using System.Data;

namespace ArlequimPetShop.Infrastructure
{
    /// <summary>
    /// Responsável por registrar e configurar todos os serviços, repositórios, comandos, queries e infraestrutura da aplicação.
    /// </summary>
    public class ManagementContainer
    {
        /// <summary>
        /// Método principal que realiza o bootstrapping dos serviços e infraestrutura da aplicação.
        /// </summary>
        /// <param name="configuration">Instância de <see cref="IConfiguration"/> contendo as configurações da aplicação.</param>
        /// <param name="services">Coleção de serviços <see cref="IServiceCollection"/> que será populada com as dependências.</param>
        public static void Install(IConfiguration configuration, IServiceCollection services)
        {
            var nhFactory = CreateNHFactory(configuration, "DefaultConnectionString");

            services.AddNhibernate(nhFactory);
            services.AddTrace().WithGuidProvider();
            services.AddCache(new CacheOptions(CacheType.LocalMemory));

            RegisterServices(configuration, services);
            RegisterRepositories(services);

            RegisterBus(services);
            RegisterCommands(services);
            RegisterQueries(services);
            RegisterEvents(services);
        }

        /// <summary>
        /// Registra os serviços de aplicação e configurações de segurança e appSettings.
        /// </summary>
        private static IServiceCollection RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<AppSettingsOptions>(configuration.GetSection("AppSettings"));
            services.Configure<ArlequimSecurityOptions>(configuration.GetSection("Security"));

            services.AddTransient<HttpClientSecurityDelegatingHandler>();
            services.AddTransient<BaseHttpClient>();

            services.AddSingleton<IProductStockInventoryService, ProductStockInventoryService>();
            services.AddSingleton<IProductDocumentFiscalImportService, ProductDocumentFiscalImportService>();

            return services;
        }

        /// <summary>
        /// Registra os repositórios de acesso a dados utilizados na aplicação.
        /// </summary>
        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IClientRepository, ClientNHRpository>();
            services.AddSingleton<IProductRepository, ProductNHRpository>();
            services.AddSingleton<ISaleRepository, SaleNHRpository>();
            services.AddSingleton<IUserRepository, UserNHRpository>();
        }

        /// <summary>
        /// Registra o barramento de comunicação de comandos e queries.
        /// </summary>
        private static void RegisterBus(IServiceCollection services)
        {
            services.AddBus(a =>
            {
                a.AddMemory();
            });
        }

        /// <summary>
        /// Registra os manipuladores de queries e associa as queries ao barramento.
        /// </summary>
        private static void RegisterQueries(IServiceCollection services)
        {
            services.AddSingleton<ProductQueryHandler>();
            services.AddSingleton<SaleQueryHandler>();
            services.AddSingleton<UserQueryHandler>();

            services.AddSingleton<IRequestBus>(a =>
            {
                var queryBus = a.GetService<MemoryContainerBus>() ?? throw new Exception("queryBus is not found");

                var productHandler = a.GetService<ProductQueryHandler>();
                queryBus.Register<ProductQuery, ProductQueryResult>(productHandler);
                queryBus.Register<ProductByIdQuery, ProductByIdQueryResult>(productHandler);
                queryBus.Register<ProductStockQuery, ProductStockQueryResult>(productHandler);

                var saleHandler = a.GetService<SaleQueryHandler>();
                queryBus.Register<SaleQuery, SaleQueryResult>(saleHandler);
                queryBus.Register<SaleByIdQuery, SaleByIdQueryResult>(saleHandler);

                var userHandler = a.GetService<UserQueryHandler>();
                queryBus.Register<UserQuery, UserQueryResult>(userHandler);

                return queryBus;
            });
        }

        /// <summary>
        /// Registra os manipuladores de comandos e associa os comandos ao barramento.
        /// </summary>
        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddSingleton<ProductCommandHandler>();
            services.AddSingleton<SaleCommandHandler>();
            services.AddSingleton<UserCommandHandler>();

            services.AddSingleton<ICommandBus>(a =>
            {
                var commandBus = a.GetService<MemoryContainerBus>() ?? throw new Exception("commandBus is not found");

                var productHandler = a.GetService<ProductCommandHandler>();
                commandBus.Register<ProductCreateCommand>(productHandler);
                commandBus.Register<ProductUpdateCommand>(productHandler);
                commandBus.Register<ProductDeleteCommand>(productHandler);
                commandBus.Register<ProductStockInventoryCommand>(productHandler);
                commandBus.Register<ProductDocumentFiscalImportCommand>(productHandler);

                var saleHandler = a.GetService<SaleCommandHandler>();
                commandBus.Register<SaleCreateCommand>(saleHandler);

                var userHandler = a.GetService<UserCommandHandler>();
                commandBus.Register<UserCreateCommand>(userHandler);
                commandBus.Register<UserLoginCommand>(userHandler);

                return commandBus;
            });
        }

        /// <summary>
        /// (Reservado para futuro uso) Registra os eventos e seus manipuladores no barramento de eventos.
        /// </summary>
        private static void RegisterEvents(IServiceCollection services)
        {
            // Evento ainda não implementado
        }

        /// <summary>
        /// Cria e configura a fábrica de sessões do NHibernate com base na connection string fornecida.
        /// </summary>
        /// <param name="configuration">Configurações da aplicação.</param>
        /// <param name="connectionStringName">Nome da connection string a ser usada.</param>
        /// <returns>Instância de <see cref="ISessionFactory"/> pronta para uso.</returns>
        private static ISessionFactory CreateNHFactory(IConfiguration configuration, string connectionStringName)
        {
            var connection = configuration.ConnectionString(connectionStringName);

            return Fluently.Configure()
                           .Database(MsSqlConfiguration.MsSql2012.IsolationLevel(IsolationLevel.ReadCommitted)
                           .ConnectionString(connection.ConnectionString))
                           .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>()
                                           .Conventions.Add(DefaultLazy.Never()))
                           .BuildSessionFactory();
        }
    }
}