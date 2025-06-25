using ArlequimPetShop.Application.CommandHandlers;
using ArlequimPetShop.Application.QueryHandlers;
using ArlequimPetShop.Contracts.Commands.Users;
using ArlequimPetShop.Contracts.Queries.Users;
using ArlequimPetShop.Domain.Users;
using ArlequimPetShop.Infrastructure.Databases.Mappings;
using ArlequimPetShop.Infrastructure.Databases.Repositories;
using ArlequimPetShop.SharedKernel;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using Microsoft.AspNetCore.Diagnostics;
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
        public static void Install(IConfiguration configuration, IServiceCollection services, bool isLambda = false)
        {
            var nhFactory = CreateNHFactory(configuration, "DefaultConnectionString");

            services.AddNhibernate(nhFactory);
            services.AddTrace().WithGuidProvider();
            services.AddCache(new CacheOptions(CacheType.LocalMemory));

            RegisterServices(configuration, services);
            RegisterRepositories(services);

            RegisterBus(services, isLambda);

            RegisterCommands(services);
            RegisterQueries(services);
            RegisterEvents(services);
            RegisterWorkers(services);
            RegisterGlobalCriteriaProcessors(services);
            RegisterIndividualCriteriaProcessors(services);
        }

        private static IServiceCollection RegisterServices(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<AppSettingsOptions>(configuration.GetSection("AppSettings"));

            services.AddTransient<HttpClientSecurityDelegatingHandler>();
            services.AddTransient<BaseHttpClient>();

            AcquisitionServices(services);
            AutorizationServices(services);
            CalculatorServices(services);
            FileServices(services);
            OperationServices(services);

            return services;
        }

        private static void OperationServices(IServiceCollection services)
        {

        }

        private static void FileServices(IServiceCollection services)
        {

        }

        private static void CalculatorServices(IServiceCollection services)
        {

        }

        private static void AutorizationServices(IServiceCollection services)
        {

        }

        private static void AcquisitionServices(IServiceCollection services)
        {
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserNHRpository>();
        }

        private static void RegisterBus(IServiceCollection services, bool isLambda = false)
        {
            services.AddBus(a =>
            {
                a.AddMemory();

                if (isLambda)
                {
                    a.AddLoopback();
                    //a.AddKafka();
                }
                //else
                //    a.AddKafka();
            });
        }

        private static void RegisterQueries(IServiceCollection services)
        {
            services.AddSingleton<UserQueryHandler>();

            services.AddSingleton<IRequestBus>(a =>
            {
                var queryBus = a.GetService<MemoryContainerBus>() ?? throw new Exception("queryBus is not found");

                var userHandler = a.GetService<UserQueryHandler>();
                queryBus.Register<UserQuery, UserQueryResult>(userHandler);

                return queryBus;
            });
        }

        private static void RegisterCommands(IServiceCollection services)
        {
            services.AddSingleton<UserCommandHandler>();

            services.AddSingleton<ICommandBus>(a =>
            {
                var commandBus = a.GetService<MemoryContainerBus>() ?? throw new Exception("commandBus is not found");

                var userHandler = a.GetService<UserCommandHandler>();
                commandBus.Register<UserCreateCommand>(userHandler);

                return commandBus;
            });
        }

        private static void RegisterEvents(IServiceCollection services)
        {
            //services.AddSingleton<AcquisitionDomainEventHandler>();

            //services.AddSingleton<IEventBus>(a =>
            //{
            //    var memoryBus = a.GetService<MemoryContainerBus>();
            //    if (memoryBus == null) throw new Exception("memoryBus is not found");
            //    var kafkaBus = a.GetService<EventBusKafka>();
            //    if (kafkaBus == null) throw new Exception("kafkaBus is not found");
            //    var compositeEventBus = new CompositeEventBus(memoryBus, kafkaBus);

            //var domainEventHandler = a.GetService<AcquisitionDomainEventHandler>();
            //memoryBus.Register<AcquisitionRightRequestRegisterDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionStatusUpdateDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionRightAcquiredDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionRightStatusUpdateDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionSendNotificationDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionIndividualEligibilityCriteriaDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionGlobalEligibilityCriteriaDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionSendTermDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionAwaitingSettlementDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionReliabilityDomainEvent>(domainEventHandler);
            //memoryBus.Register<AcquisitionSentCollateralDomainEvent>(domainEventHandler);

            //    return compositeEventBus;
            //});
        }

        private static void RegisterWorkers(IServiceCollection services)
        {

        }

        private static void RegisterGlobalCriteriaProcessors(IServiceCollection services)
        {

        }

        private static void RegisterIndividualCriteriaProcessors(IServiceCollection services)
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