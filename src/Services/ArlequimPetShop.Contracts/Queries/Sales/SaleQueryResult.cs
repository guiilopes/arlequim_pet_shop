using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;
using static ArlequimPetShop.Contracts.Queries.Sales.SaleQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    public class SaleQueryResult : PartialCollection<SaleQueryItem>, IRequestResult
    {
        public SaleQueryResult() { }

        public SaleQueryResult(IList<SaleQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        public class SaleQueryItem
        {
            public SaleQueryItem()
            {
                Client = new SaleClientQueryItem();
                Products = new List<SaleProductQueryItem>();
            }

            public Guid? Id { get; set; }

            [JsonIgnore]
            public Guid? ClientId { get; set; }

            [JsonIgnore]
            public string? ClientName { get; set; }

            [JsonIgnore]
            public string? ClientDocument { get; set; }

            public SaleClientQueryItem? Client { get; set; }

            public List<SaleProductQueryItem> Products { get; set; }

            public decimal? TotalPrice { get; set; }

            public DateTime? CreatedOn { get; set; }

            public DateTime? UpdatedOn { get; set; }
        }
    }
}