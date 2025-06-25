using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Clients
{
    public class Client : AggregateRoot<Guid>
    {
        public Client() { }

        public Client(Guid id, string? name, string? document) : this()
        {
            Id = id;
            Name = name ?? document;
            Document = document;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public override Guid Id { get; set; }

        public string Name { get; set; }

        [RequiredValidator(ErrorMessage = "Documento do cliente obrigatório.")]
        public string Document { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }
    }
}