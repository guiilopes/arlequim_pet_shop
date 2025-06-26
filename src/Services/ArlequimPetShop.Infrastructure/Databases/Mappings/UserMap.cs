using ArlequimPetShop.Domain.Users;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    /// <summary>
    /// Mapeamento da entidade <see cref="User"/> para a tabela no banco de dados usando Fluent NHibernate.
    /// </summary>
    public class UserMap : ClassMap<User>
    {
        /// <summary>
        /// Construtor responsável por configurar o mapeamento dos campos da entidade <see cref="User"/>.
        /// </summary>
        public UserMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.Type).Not.Nullable();
            Map(m => m.Name).Not.Nullable();
            Map(m => m.Email).Not.Nullable();
            Map(m => m.Password).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();

            HasMany(m => m.Logins).KeyColumn("UserId").ExtraLazyLoad().AsBag().BatchSize(16).Inverse().Cascade.AllDeleteOrphan().Where(" DeletedOn IS NULL ");
        }
    }
}