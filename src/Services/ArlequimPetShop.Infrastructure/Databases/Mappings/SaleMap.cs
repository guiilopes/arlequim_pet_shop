using ArlequimPetShop.Domain.Sales;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class SaleMap : ClassMap<Sale>
    {
        public SaleMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.TotalPrice).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();

            References(m => m.Client).Column("ClientId").Not.Nullable().Cascade.None();

            HasMany(m => m.Products).KeyColumn("SaleId").ExtraLazyLoad().AsBag().BatchSize(16).Inverse().Cascade.AllDeleteOrphan().Where(" DeletedOn IS NULL ");
        }
    }
}