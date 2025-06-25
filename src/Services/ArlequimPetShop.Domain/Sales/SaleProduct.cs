using ArlequimPetShop.Domain.Products;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Sales
{
    public class SaleProduct
    {
        public SaleProduct()
        {
        }

        public SaleProduct(Sale sale, Product product, decimal? quantity, decimal? discount, decimal? netPrice) : this()
        {
            Sale = sale;
            Product = product;
            Quantity = quantity;
            Price = product.Price;
            Discount = discount;
            NetPrice = netPrice;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public virtual int Id { get; set; }

        [RequiredValidator(ErrorMessage = "Quantidade do produto da venda obrigatória.")]
        public virtual decimal? Quantity { get; set; }

        [RequiredValidator(ErrorMessage = "Preço do produto da venda obrigatório.")]
        public virtual decimal? Price { get; set; }

        [RequiredValidator(ErrorMessage = "Desconto do produto da venda obrigatório.")]
        public virtual decimal? Discount { get; set; }

        [RequiredValidator(ErrorMessage = "Preço líquido do produto da venda obrigatório.")]
        public virtual decimal? NetPrice { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do produto da venda obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto da venda obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual Product Product { get; set; }
    }
}