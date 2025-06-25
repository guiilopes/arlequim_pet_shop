namespace ArlequimPetShop.Contracts.Queries.Sales
{
    public class SaleProductQueryItem
    {
        public Guid? SaleId { get; set; }

        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public decimal? Quantity { get; set; }

        public decimal? Price { get; set; }

        public decimal? Discount { get; set; }

        public decimal? NetPrice { get; set; }
    }
}