using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Users
{
    public class UserQueryResult : PartialCollection<UserQueryItem>, IRequestResult
    {
        public UserQueryResult() { }

        public UserQueryResult(IList<UserQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }
    }

    public class UserQueryItem
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}