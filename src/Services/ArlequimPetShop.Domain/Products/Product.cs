using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Products
{
    public class Product : AggregateRoot<Guid>
    {
        public Product()
        {
            Stocks = new List<ProductStock>();
        }

        public Product(Guid id, string barcode, string? name, string? description, decimal? price, DateTime? expirationDate, decimal? quantity = 0M) : this()
        {
            Id = id;
            Barcode = barcode;
            Name = name;
            Description = description;
            Price = price;
            ExpirationDate = expirationDate;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        [RequiredValidator(ErrorMessage = "Id obrigatório.")]
        public override Guid Id { get; set; }

        [RequiredValidator(ErrorMessage = "Código de barras do produto obrigatório.")]
        public string? Barcode { get; set; }

        [RequiredValidator(ErrorMessage = "Nome do produto obrigatório.")]
        public string? Name { get; set; }

        [RequiredValidator(ErrorMessage = "Descrição do produto obrigatório.")]
        public string? Description { get; set; }

        [RequiredValidator(ErrorMessage = "Preço do produto obrigatório.")]
        public decimal? Price { get; set; }

        public DateTime? ExpirationDate { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatório.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatório.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual IList<ProductStock> Stocks { get; set; }

        public void AddStock(decimal? quantity = 0M)
        {
            var stock = Stocks.Where(s => s.Product == this)
                              .FirstOrDefault();

            if (stock == null)
                Stocks.Add(new ProductStock(this, quantity));
            else
                stock.Update(quantity);

            UpdatedOn = DateTime.Now;
        }

        public void Update(string? barcode, string? name, string? description, decimal? price, DateTime? expirationDate)
        {
            Barcode = barcode;
            Name = name;
            Description = description;
            Price = price;
            ExpirationDate = expirationDate;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Delete()
        {
            DeletedOn = DateTime.Now;
        }
    }
}