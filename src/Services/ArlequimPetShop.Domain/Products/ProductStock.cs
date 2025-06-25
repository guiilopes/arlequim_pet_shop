using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Products
{
    public class ProductStock
    {
        public ProductStock()
        {

        }

        public ProductStock(Product product, decimal? quantity) : this()
        {
            Product = product;
            Quantity = quantity;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public int Id { get; set; }

        [RequiredValidator(ErrorMessage = "Quantidade do produto obrigatória.")]
        public decimal? Quantity { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual Product Product { get; set; }

        public void Add(decimal? quantity)
        {
            Quantity += quantity;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Update(decimal? quantity)
        {
            Quantity = quantity;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Remove(decimal? quantity)
        {
            Quantity -= quantity;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Delete()
        {
            DeletedOn = DateTime.Now;
        }
    }
}