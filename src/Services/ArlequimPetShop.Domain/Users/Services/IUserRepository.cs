using SrShut.Data;

namespace ArlequimPetShop.Domain.Users.Services
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsCredentialsValid(string email, string? password);

        Task<bool> HasUserByEmail(string email);

        Task<User> GetAsyncByEmail(string email);
    }
}
