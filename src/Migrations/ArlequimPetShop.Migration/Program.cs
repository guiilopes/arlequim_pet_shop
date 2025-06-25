namespace ArlequimPetShop.Migrations
{
    using FluentMigrator.Runner;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System;

    namespace ArlequimPetShop.Migrations
    {
        class Program
        {
            static void Main(string[] args)
            {
                var host = Host.CreateDefaultBuilder(args)
                    .ConfigureAppConfiguration(config =>
                    {
                        config.AddJsonFile("appsettings.json", optional: false);
                    })
                    .ConfigureServices((context, services) =>
                    {
                        var connectionString = context.Configuration.GetConnectionString("DefaultConnectionString");
                        Console.WriteLine($"Conectando com: {connectionString}");

                        services
                            .AddFluentMigratorCore()
                            .ConfigureRunner(rb => rb
                                .AddSqlServer()
                                .WithGlobalConnectionString(connectionString)
                                .ScanIn(typeof(CreateTableUser).Assembly).For.Migrations());
                    })
                    .Build();

                using var scope = host.Services.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
        }
    }
}