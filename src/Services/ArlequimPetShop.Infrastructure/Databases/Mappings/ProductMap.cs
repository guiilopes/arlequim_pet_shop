using ArlequimPetShop.Domain.Products;
using ArlequimPetShop.Domain.Users;
using FluentNHibernate.Mapping;

namespace ArlequimPetShop.Infrastructure.Databases.Mappings
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(m => m.Id).Not.Nullable();

            Map(m => m.Barcode).Not.Nullable();
            Map(m => m.Name).Not.Nullable();
            Map(m => m.Description).Not.Nullable();
            Map(m => m.Price).Not.Nullable();
            Map(m => m.ExpirationDate).Nullable();

            Map(m => m.CreatedOn).Nullable();
            Map(m => m.UpdatedOn).Nullable();
            Map(m => m.DeletedOn).Nullable();

            HasMany(m => m.Stocks).KeyColumn("ProductId").ExtraLazyLoad().AsBag().BatchSize(16).Inverse().Cascade.AllDeleteOrphan().Where(" DeletedOn IS NULL ");
        }
    }
}