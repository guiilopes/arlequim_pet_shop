using SrShut.Validation;

namespace ArlequimPetShop.Domain.Products
{
    public class ProductHistory
    {      
        /*Essa classe eu não quis fazer sumario, 
          para mostrar como geralmente na camada de Domain 
          eu gosto de deixar a classe, para não ficar muito poluida
        */
        public ProductHistory()
        {

        }

        public ProductHistory(Product product, string description, decimal? quantity, string? documentNumber) : this()
        {
            Product = product;
            Description = description;
            Quantity = quantity;
            DocumentFiscalNumber = documentNumber;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        [RequiredValidator(ErrorMessage = "Nome do produto obrigatório.")]
        public decimal? Quantity { get; set; }

        [RequiredValidator(ErrorMessage = "Número da nota fiscal do produto obrigatório.")]
        public string? DocumentFiscalNumber { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do histórico do produto obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do histórico do produto obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual Product Product { get; set; }
    }
}