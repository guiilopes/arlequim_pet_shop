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

        [RequiredValidator(ErrorMessage = "Nome do produto obrigatório.")]
        public decimal? Quantity { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatório.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatório.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual Product Product { get; set; }

        public void Update(decimal? quantity, bool? incress = false)
        {
            if (!incress.HasValue)
                Quantity = quantity;
            else
                Quantity += quantity ?? 0M;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Delete()
        {
            DeletedOn = DateTime.Now;
        }
    }
}