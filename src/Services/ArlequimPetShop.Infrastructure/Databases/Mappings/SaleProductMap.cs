using ArlequimPetShop.Domain.Sales;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class SaleProductMap : ClassMap<SaleProduct>
    {
        public SaleProductMap()
        {
            Id(m => m.Id).Not.Nullable().GeneratedBy.Identity();

            Map(m => m.Quantity).Not.Nullable();
            Map(m => m.Price).Not.Nullable();
            Map(m => m.Discount).Not.Nullable();
            Map(m => m.NetPrice).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();

            References(m => m.Sale).Column("SaleId").Not.Nullable().Cascade.None();
            References(m => m.Product).Column("ProductId").Not.Nullable().Cascade.None();
        }
    }
}