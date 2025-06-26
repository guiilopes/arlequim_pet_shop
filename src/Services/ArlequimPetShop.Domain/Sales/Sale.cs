using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.Domain.Products;
using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Sales
{
    /*Essa classe eu não quis fazer sumario, 
      para mostrar como geralmente na camada de Domain 
      eu gosto de deixar a classe, para não ficar muito poluida
    */
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
            TotalPrice = 0M;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public override Guid Id { get; set; }

        [RequiredValidator(ErrorMessage = "Valor total da venda obrigatório.")]
        public decimal? TotalPrice { get; set; }

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

            TotalPrice += netPrice;

            UpdatedOn = DateTime.Now;
        }
    }
}