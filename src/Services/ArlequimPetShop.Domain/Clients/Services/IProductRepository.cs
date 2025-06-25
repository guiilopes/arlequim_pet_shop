using SrShut.Data;

namespace ArlequimPetShop.Domain.Clients.Services
{
    public interface IClientRepository : IRepository<Client>
    {
        Task<Client> GetAsyncByDocument(string? document);
    }
}