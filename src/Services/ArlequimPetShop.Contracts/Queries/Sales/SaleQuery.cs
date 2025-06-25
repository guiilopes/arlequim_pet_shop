using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    public class SaleQuery : PaginationCriteria, IRequest<SaleQueryResult>
    {
        public SaleQuery()
        {
        }

        public SaleQuery(string? text) : base()
        {
            Text = text;
        }

        public string? Text { get; set; }
    }
}