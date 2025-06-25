using ArlequimPetShop.Domain.Products;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class ProductStockMap : ClassMap<ProductStock>
    {
        public ProductStockMap()
        {
            Id(m => m.Id).Not.Nullable().GeneratedBy.Identity();

            Map(m => m.Quantity).Not.Nullable();

            Map(m => m.CreatedOn).Not.Nullable();
            Map(m => m.UpdatedOn).Not.Nullable();
            Map(m => m.DeletedOn).Nullable();

            References(m => m.Product).Column("ProductId").Not.Nullable().Cascade.None();
        }
    }
}