using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    public class SaleByIdQueryResult : IRequestResult
    {
        public SaleByIdQueryResult()
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