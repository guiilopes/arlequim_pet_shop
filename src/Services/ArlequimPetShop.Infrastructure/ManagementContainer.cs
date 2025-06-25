using ArlequimPetShop.Application.CommandHandlers;
using ArlequimPetShop.Application.QueryHandlers;
using ArlequimPetShop.Contracts.Commands.Products;
using ArlequimPetShop.Contracts.Commands.Sales;
using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Contracts.Queries.Products;
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
    public class ManagementContainer
    {
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

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IClientRepository, ClientNHRpository>();
            services.AddSingleton<IProductRepository, ProductNHRpository>();
            services.AddSingleton<ISaleRepository, SaleNHRpository>();
            services.AddSingleton<IUserRepository, UserNHRpository>();
        }

        private static void RegisterBus(IServiceCollection services)
        {
            services.AddBus(a =>
            {
                a.AddMemory();
            });
        }

        private static void RegisterQueries(IServiceCollection services)
        {
            services.AddSingleton<ProductQueryHandler>();
            services.AddSingleton<UserQueryHandler>();

            services.AddSingleton<IRequestBus>(a =>
            {
                var queryBus = a.GetService<MemoryContainerBus>() ?? throw new Exception("queryBus is not found");

                var productHandler = a.GetService<ProductQueryHandler>();
                queryBus.Register<ProductQuery, ProductQueryResult>(productHandler);
                queryBus.Register<ProductByIdQuery, ProductByIdQueryResult>(productHandler);
                queryBus.Register<ProductStockQuery, ProductStockQueryResult>(productHandler);

                var userHandler = a.GetService<UserQueryHandler>();
                queryBus.Register<UserQuery, UserQueryResult>(userHandler);

                return queryBus;
            });
        }

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

        private static void RegisterEvents(IServiceCollection services)
        {

        }

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