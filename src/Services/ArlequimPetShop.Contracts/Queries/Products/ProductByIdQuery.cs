using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    public class ProductByIdQuery : IRequest<ProductByIdQueryResult>
    {
        public ProductByIdQuery() { }

        public ProductByIdQuery(Guid? id, string? name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}