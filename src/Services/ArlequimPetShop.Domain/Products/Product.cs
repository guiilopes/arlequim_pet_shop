using SrShut.Cqrs.Domains;
using SrShut.Validation;
using System.Text;

namespace ArlequimPetShop.Domain.Products
{
    public class Product : AggregateRoot<Guid>
    {
        /*Essa classe eu não quis fazer sumario, 
          para mostrar como geralmente na camada de Domain 
          eu gosto de deixar a classe, para não ficar muito poluida
        */
        public Product()
        {
            Stocks = new List<ProductStock>();
            Histories = new List<ProductHistory>();
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

        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual IList<ProductStock> Stocks { get; set; }

        public virtual IList<ProductHistory> Histories { get; set; }

        public bool HasSufficientStock(decimal? quantity)
        {
            return Stocks.FirstOrDefault().Quantity < quantity;
        }

        public void AddStock(decimal? quantity = 0M)
        {
            var stock = Stocks.Where(s => s.Product == this)
                              .FirstOrDefault();

            if (stock == null)
                Stocks.Add(new ProductStock(this, quantity));
            else
                stock.Add(quantity);

            UpdatedOn = DateTime.Now;
        }

        public void AddHistory(string? name, string? description, decimal? quantity, string? documentNumber)
        {
            var message = new StringBuilder();

            if (Name != name)
                message.Append($"Produto alterado o nome de {Name} para {name}; ");

            if (Description != description)
                message.Append($"Produto alterado a descrição de {Description} para {description}; ");

            var entity = new ProductHistory(this, message.ToString(), quantity, documentNumber);

            Histories.Add(entity);

            UpdatedOn = DateTime.Now;
        }

        public void UpdateStock(decimal? quantity = 0M)
        {
            var stock = Stocks.Where(s => s.Product == this)
                              .FirstOrDefault();

            if (stock == null)
                Stocks.Add(new ProductStock(this, quantity));
            else
                stock.Update(quantity);

            UpdatedOn = DateTime.Now;
        }

        public void RemoveStock(decimal? quantity)
        {
            var stock = Stocks.FirstOrDefault();

            stock?.Remove(quantity);

            UpdatedOn = DateTime.Now;
        }

        public void Update(string? name, string? description, decimal? price, DateTime? expirationDate)
        {
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