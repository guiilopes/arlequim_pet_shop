using SrShut.Cqrs.Commands;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Products
{
    public class ProductDeleteCommand : ICommand
    {
        [JsonIgnore]
        public Guid? Id { get; set; }
    }
}