using SrShut.Data;

namespace ArlequimPetShop.Domain.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> HasUserByEmail(string email);
    }
}
