using ArlequimPetShop.Api.Helpers;
using ArlequimPetShop.Infrastructure;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using SrShut.Common.AppSettings;
using SrShut.Cqrs.Traces;
using SrShut.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
var host = builder.Host;
var culture = CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

ManagementContainer.Install(configuration, services);

builder.Services.AddHttpContextAccessor();

services.AddControllers(a =>
{
    a.Filters.Add(typeof(UnitOfWorkAttribute));
})
.AddJsonOptions(a =>
{
    a.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

LogManager.Configuration = new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog"));
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog(configuration);

services.AddCors(option => option.AddPolicy("ArlequimPolicy", builder =>
{
    builder.WithExposedHeaders("Content-Disposition")
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           ;
}));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Arlequim - PetShop - API",
        Version = "v1",
        Description = "Arlequim - PetShop - API"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Por favor, insira JWT com portador no campo",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            }
                        },
                        new string[] { }
                    }
                });

    options.SchemaFilter<SwaggerIgnoreFilter>();

    options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date", });
    options.OrderActionsBy(apiDesc => apiDesc.RelativePath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseTrace();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("ArlequimPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger(c =>
{
    string apiUrl = configuration.AppSettings("ApiUrl");
    if (!string.IsNullOrEmpty(apiUrl))
    {
        c.PreSerializeFilters.Add((swagger, httpReq) =>
        {
            swagger.Servers = new List<OpenApiServer>
            {
                new OpenApiServer { Url = apiUrl }
            };
        });
    }
});

app.UseSwaggerUI(options => { options.SwaggerEndpoint("./v1/swagger.json", "Arlequim - PetShop - API"); });
app.Run();