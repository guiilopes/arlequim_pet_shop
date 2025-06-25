using CsvHelper.Configuration.Attributes;

namespace ArlequimPetShop.Domain.Products.Dtos
{
    public class ProductStockInventoryDto
    {
        [Name("barcode")]
        public string? Barcode { get; set; }

        [Name("name")]
        public string? Name { get; set; }

        [Name("description")]
        public string? Description { get; set; }

        [Name("price")]
        public decimal? Price { get; set; }

        [Name("quantity")]
        public decimal? Quantity { get; set; }

        [Name("expirationdate")]
        public DateTime? ExpirationDate { get; set; }
    }
}