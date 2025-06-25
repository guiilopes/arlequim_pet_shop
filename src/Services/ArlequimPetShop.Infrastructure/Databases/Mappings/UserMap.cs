using ArlequimPetShop.Domain.Users;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.Type).Not.Nullable();
            Map(m => m.Name).Not.Nullable();
            Map(m => m.Email).Not.Nullable();
            Map(m => m.Password).Not.Nullable();

            Map(m => m.CreatedOn).Nullable();
            Map(m => m.UpdatedOn).Nullable();
            Map(m => m.DeletedOn).Nullable();

            HasMany(m => m.Logins).KeyColumn("UserId").ExtraLazyLoad().AsBag().BatchSize(16).Inverse().Cascade.AllDeleteOrphan().Where(" DeletedOn IS NULL ");
        }
    }
}