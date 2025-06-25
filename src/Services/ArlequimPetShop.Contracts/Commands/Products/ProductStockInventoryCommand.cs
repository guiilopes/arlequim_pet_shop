using Microsoft.AspNetCore.Http;
using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando para importar planilha de estoque
    /// </summary>
    public class ProductStockInventoryCommand : ICommand
    {
        public ProductStockInventoryCommand() { }

        public string? DocumentFiscalNumber { get; set; }

        [JsonIgnore]
        public string? UserName { get; set; }

        /// <summary>
        /// Arquivo da planilha.  
        /// [Clique aqui para baixar o modelo](https://localhost:5001/assets/modeloestoque.csv)
        /// </summary>
        [Required]
        public IFormFile? File { get; set; }
    }
}