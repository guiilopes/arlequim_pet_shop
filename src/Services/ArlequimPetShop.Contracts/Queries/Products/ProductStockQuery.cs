using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    public class ProductStockQuery : PaginationCriteria, IRequest<ProductStockQueryResult>
    {
        public ProductStockQuery()
        {
        }

        public ProductStockQuery(string? text) : base()
        {
            Text = text;
        }

        public string? Text { get; set; }

        public DateTime? StartExpirationDate { get; set; }

        public DateTime? EndExpirationDate { get; set; }
    }
}