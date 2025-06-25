using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Products.ProductStockQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    public class ProductStockQueryResult : PartialCollection<ProductStockQueryItem>, IRequestResult
    {
        public ProductStockQueryResult() { }

        public ProductStockQueryResult(IList<ProductStockQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        public class ProductStockQueryItem
        {
            public Guid? ProductId { get; set; }

            public int? Id { get; set; }

            public string? Name { get; set; }

            public string? Description { get; set; }

            public decimal? Price { get; set; }

            public DateTime? ExpirationDate { get; set; }

            public decimal? Quantity { get; set; }

            public string? LastDocumentFiscalNumber { get; set; }

            public DateTime? CreatedOn { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }
    }
}