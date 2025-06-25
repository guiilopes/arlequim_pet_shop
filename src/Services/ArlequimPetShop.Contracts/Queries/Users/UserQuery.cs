using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Users
{
    public class UserQuery : IRequest<UserQueryResult>
    {
        public UserQuery()
        {
        }

        public UserQuery(string text) : base()
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}