using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.Domain.Products;
using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Sales
{
    public class Sale : AggregateRoot<Guid>
    {
        public Sale()
        {
            Products = new List<SaleProduct>();
        }

        public Sale(Guid id, Client client) : this()
        {
            Id = id;
            Client = client;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public override Guid Id { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação da venda obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização da venda obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual Client Client { get; set; }

        public virtual IList<SaleProduct> Products { get; set; }

        public void AddProduct(Product product, decimal? quantity, decimal? discount, decimal? netPrice)
        {
            Products.Add(new SaleProduct(this, product, quantity, discount, netPrice));

            UpdatedOn = DateTime.Now;
        }
    }
}