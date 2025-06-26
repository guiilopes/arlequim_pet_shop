using ArlequimPetShop.Api.Helpers;
using ArlequimPetShop.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registra a factory do provedor de dados SQL Server.
/// Necessário para ambientes que usam ADO.NET ou NHibernate diretamente.
/// </summary>
DbProviderFactories.RegisterFactory("System.Data.SqlClient", SqlClientFactory.Instance);

/// <summary>
/// Configura o servidor Kestrel (desabilita o header padrão e aumenta o tamanho permitido no corpo da requisição).
/// </summary>
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
    serverOptions.Limits.MaxRequestBodySize = long.MaxValue;
});

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;

/// <summary>
/// Define cultura padrão do sistema como pt-BR (para datas, números, etc).
/// </summary>
var culture = CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
CultureInfo.CurrentUICulture = culture;
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

/// <summary>
/// Injeta dependências customizadas da aplicação.
/// </summary>
ManagementContainer.Install(configuration, services);

builder.Services.AddHttpContextAccessor();

/// <summary>
/// Configura controllers com filtro global de UnitOfWork e enum como string no JSON.
/// </summary>
services.AddControllers(a =>
{
    a.Filters.Add(typeof(UnitOfWorkAttribute));
})
.AddJsonOptions(a =>
{
    a.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

/// <summary>
/// Configuração do NLog.
/// </summary>
LogManager.Configuration = new NLogLoggingConfiguration(builder.Configuration.GetSection("NLog"));
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Logging.AddNLog(configuration);

/// <summary>
/// Configura CORS para permitir qualquer origem, método e cabeçalho.
/// </summary>
services.AddCors(option => option.AddPolicy("ArlequimPolicy", builder =>
{
    builder.WithExposedHeaders("Content-Disposition")
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

/// <summary>
/// Habilita Swagger + filtros customizados.
/// </summary>
services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Arlequim API", Version = "v1" });

    // JWT auth config
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

    // Comentários XML para documentação de métodos
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Filtros para upload de arquivos e ignorar campos via atributo [Ignore]
    c.SchemaFilter<FileUploadSchemaFilter>();
    c.SchemaFilter<SwaggerIgnoreFilter>();

    c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);
});

/// <summary>
/// Configuração da autenticação JWT com validação de emissor, audiência e chave.
/// </summary>
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

    // Mensagem customizada para 403 Forbidden
    options.Events = new JwtBearerEvents
    {
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                success = false,
                message = "Você não tem permissão para acessar este recurso."
            });

            return context.Response.WriteAsync(result);
        }
    };
});

var app = builder.Build();

/// <summary>
/// Configura tratamento de erros padrão e modo desenvolvedor.
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

/// <summary>
/// Middlewares e configuração do pipeline da aplicação.
/// </summary>
app.UseStaticFiles();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseTrace();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("ArlequimPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

/// <summary>
/// Swagger: registra a URL da API via appsettings, se existir.
/// </summary>
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

/// <summary>
/// Swagger UI configurado para versão v1.
/// </summary>
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("./v1/swagger.json", "Arlequim - PetShop - API");
});

/// <summary>
/// Inicia o pipeline da aplicação.
/// </summary>
app.Run();