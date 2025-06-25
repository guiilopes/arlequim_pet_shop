using ArlequimPetShop.Domain.Clients;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class ClientMap : ClassMap<Client>
    {
        public ClientMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.Name).Not.Nullable();
            Map(m => m.Document).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();
        }
    }
}