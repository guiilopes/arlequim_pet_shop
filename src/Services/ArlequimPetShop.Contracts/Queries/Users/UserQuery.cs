using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Users
{
    public class UserQuery : PaginationCriteria, IRequest<UserQueryResult>
    {
        public UserQuery()
        {
        }

        public UserQuery(string? text) : base()
        {
            Text = text;
        }

        public string? Text { get; set; }
    }
}