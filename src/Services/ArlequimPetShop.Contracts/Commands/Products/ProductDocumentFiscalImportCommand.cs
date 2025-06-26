using Microsoft.AspNetCore.Http;
using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    /// <summary>
    /// Comando responsável por importar documentos fiscais XML para inclusão de produtos no estoque.
    /// </summary>
    public class ProductDocumentFiscalImportCommand : ICommand
    {
        /// <summary>
        /// Arquivo XML contendo os dados da nota fiscal eletrônica (NFe) para importação.
        /// A validação de extensão e estrutura ocorre na camada de serviço.
        /// </summary>
        [Required(ErrorMessage = "O arquivo da nota fiscal é obrigatório.")]
        public IFormFile? File { get; set; }
    }
}