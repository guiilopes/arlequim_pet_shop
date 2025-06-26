using ArlequimPetShop.Contracts.Commands.Products;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArlequimPetShop.Api.Helpers
{
    /// <summary>
    /// Filtro de schema utilizado para customizar a descrição do campo "File" no Swagger,
    /// especificamente para o comando <see cref="ProductStockInventoryCommand"/>.
    /// </summary>
    public class FileUploadSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Aplica a customização do schema para exibir uma descrição personalizada no campo "File"
        /// do comando <see cref="ProductStockInventoryCommand"/> no Swagger.
        /// </summary>
        /// <param name="schema">Esquema OpenAPI a ser modificado.</param>
        /// <param name="context">Contexto do schema atual sendo gerado.</param>
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