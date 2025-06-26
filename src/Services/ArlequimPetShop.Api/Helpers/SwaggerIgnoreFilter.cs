using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace ArlequimPetShop.Api.Helpers
{
    /// <summary>
    /// Filtro para remover propriedades do schema Swagger que estejam marcadas com o atributo <see cref="IgnoreAttribute"/>.
    /// </summary>
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        /// <summary>
        /// Aplica o filtro ao schema, removendo campos marcados com o atributo <see cref="IgnoreAttribute"/>.
        /// </summary>
        /// <param name="schema">Esquema OpenAPI a ser modificado.</param>
        /// <param name="schemaFilterContext">Contexto do schema atual sendo gerado.</param>
        public void Apply(OpenApiSchema schema, SchemaFilterContext schemaFilterContext)
        {
            if (schema.Properties.Count == 0)
                return;

            const BindingFlags bindingFlags = BindingFlags.Public |
                                              BindingFlags.NonPublic |
                                              BindingFlags.Instance;

            // Coleta membros (fields e properties) com os atributos definidos
            var memberList = schemaFilterContext.Type
                .GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(schemaFilterContext.Type.GetProperties(bindingFlags));

            // Seleciona nomes das propriedades com [Ignore]
            var excludedList = memberList
                .Where(m => m.GetCustomAttribute<IgnoreAttribute>() != null)
                .Select(m =>
                    m.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
                    ?? m.Name.ToCamelCase());

            // Remove do schema
            foreach (var excludedName in excludedList)
            {
                if (schema.Properties.ContainsKey(excludedName))
                    schema.Properties.Remove(excludedName);
            }
        }
    }

    /// <summary>
    /// Extensão auxiliar para converter nomes em camelCase, compatível com convenções Swagger e JSON.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Converte a primeira letra da string para minúscula (camelCase).
        /// </summary>
        /// <param name="value">Texto a ser convertido.</param>
        /// <returns>Texto em camelCase.</returns>
        internal static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return char.ToLowerInvariant(value[0]) + value[1..];
        }
    }

    /// <summary>
    /// Atributo para indicar que um campo ou propriedade deve ser ignorado na geração do Swagger.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    internal class IgnoreAttribute : Attribute
    {
    }
}