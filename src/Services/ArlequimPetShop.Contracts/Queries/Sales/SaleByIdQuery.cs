using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    public class SaleByIdQuery : IRequest<SaleByIdQueryResult>
    {
        public SaleByIdQuery() { }

        public SaleByIdQuery(Guid? id)
        {
            Id = id;
        }

        [JsonIgnore]
        public Guid? Id { get; set; }
    }
}