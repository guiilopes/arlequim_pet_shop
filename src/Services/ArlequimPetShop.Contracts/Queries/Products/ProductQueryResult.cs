using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Products.ProductQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    public class ProductQueryResult : PartialCollection<ProductQueryItem>, IRequestResult
    {
        public ProductQueryResult() { }

        public ProductQueryResult(IList<ProductQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        public class ProductQueryItem
        {
            public Guid? Id { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public decimal? Price { get; set; }

            public DateTime? ExpirationDate { get; set; }

            public decimal? StockQuantity { get; set; }

            public DateTime? CreatedOn { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }
    }
}