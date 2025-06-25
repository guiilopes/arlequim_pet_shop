using SrShut.Cqrs.Commands;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Commands.Sales
{
    public class SaleCreateCommand : ICommand
    {
        public SaleCreateCommand()
        {
            Products = new List<SaleProductCommandItem>();
        }

        [JsonIgnore]
        public Guid Id { get; set; }

        public string? Document { get; set; }

        public string? Name { get; set; }

        public List<SaleProductCommandItem> Products { get; set; }
    }

    public class SaleProductCommandItem
    {
        public string? Barcode { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Discount { get; set; }
    }
}