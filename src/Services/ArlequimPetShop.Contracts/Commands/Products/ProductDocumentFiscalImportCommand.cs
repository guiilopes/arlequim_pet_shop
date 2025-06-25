using Microsoft.AspNetCore.Http;
using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    public class ProductDocumentFiscalImportCommand : ICommand
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}