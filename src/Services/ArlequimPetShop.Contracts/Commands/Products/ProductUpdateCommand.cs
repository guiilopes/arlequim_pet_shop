using SrShut.Cqrs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    public class ProductUpdateCommand : ICommand
    {
        [JsonIgnore]
        public Guid? Id { get; set; }

        [Required]
        public string? Barcode { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public decimal? Price { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}