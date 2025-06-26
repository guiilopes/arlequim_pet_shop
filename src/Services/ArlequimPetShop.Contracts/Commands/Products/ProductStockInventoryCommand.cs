using Microsoft.AspNetCore.Http;
using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando para importação de planilha de estoque de produtos.
    /// Permite atualizar as quantidades de produtos com base em dados de uma planilha CSV.
    /// </summary>
    public class ProductStockInventoryCommand : ICommand
    {
        /// <summary>
        /// Inicializa uma nova instância do comando <see cref="ProductStockInventoryCommand"/>.
        /// </summary>
        public ProductStockInventoryCommand() { }

        /// <summary>
        /// Nome do usuário que está realizando a importação (preenchido automaticamente pelo controller).
        /// </summary>
        [JsonIgnore]
        public string? UserName { get; set; }

        /// <summary>
        /// Arquivo da planilha CSV com os dados de estoque a serem importados.  
        /// Formato suportado: `.csv`.  
        /// [Clique aqui para baixar o modelo](https://localhost:5001/assets/modeloestoque.csv)
        /// </summary>
        [Required(ErrorMessage = "O arquivo da planilha é obrigatório.")]
        public IFormFile? File { get; set; }
    }
}