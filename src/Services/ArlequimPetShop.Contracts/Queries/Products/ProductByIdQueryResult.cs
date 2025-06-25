using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    public class ProductByIdQueryResult : IRequestResult
    {
        public ProductByIdQueryResult() { }

        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public decimal? Price { get; set; }

        public decimal? StockQuantity { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}