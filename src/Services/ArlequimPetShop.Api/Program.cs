using ArlequimPetShop.Api.Helpers;
using ArlequimPetShop.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using SrShut.Common.AppSettings;
using SrShut.Cqrs.Traces;
using SrShut.Mvc;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arlequim API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header.  
                        Informe assim: Bearer **seu_token_aqui**",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "Bearer",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    c.SchemaFilter<SwaggerIgnoreFilter>();

    c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date", });
    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);
});


builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = "arlequim",
        ValidAudience = "arlequim-petshop",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Security:Secret"]))
    };
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