using ArlequimPetShop.Contracts.Commands.Products;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArlequimPetShop.Api.Helpers
{
    public class FileUploadSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.Name == nameof(ProductStockInventoryCommand))
            {
                if (schema.Properties.ContainsKey("File"))
                {
                    schema.Properties["File"].Description = "Faça upload da planilha. [Clique aqui para baixar o modelo](/assets/modeloestoque.csv)";
                }
            }
        }
    }
}