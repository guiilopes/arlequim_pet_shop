using SrShut.Cqrs.Domains;
using SrShut.Validation;

namespace ArlequimPetShop.Domain.Clients
{
    /// <summary>
    /// Entidade que representa um cliente no domínio da aplicação.
    /// </summary>
    public class Client : AggregateRoot<Guid>
    {
        /// <summary>
        /// Construtor padrão para ORMs e serialização.
        /// </summary>
        public Client() { }

        /// <summary>
        /// Construtor principal da entidade.
        /// </summary>
        /// <param name="id">Identificador único do cliente.</param>
        /// <param name="name">Nome do cliente.</param>
        /// <param name="document">Documento (CPF/CNPJ) do cliente.</param>
        public Client(Guid id, string? name, string? document) : this()
        {
            Id = id;
            Name = name ?? document;
            Document = document;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate(); // Validação baseada nos atributos decorados com RequiredValidator
        }

        /// <summary>
        /// Identificador único do cliente.
        /// </summary>
        public override Guid Id { get; set; }

        /// <summary>
        /// Nome do cliente.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Documento do cliente (CPF ou CNPJ).
        /// Obrigatório.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Documento do cliente obrigatório.")]
        public string? Document { get; set; }

        /// <summary>
        /// Data de criação do cliente.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Data de criação do produto obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        /// <summary>
        /// Data da última atualização do cliente.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Data de atualização do produto obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Data de exclusão lógica do cliente.
        /// </summary>
        public virtual DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Atualiza o nome do cliente e define a data de modificação.
        /// </summary>
        /// <param name="name">Novo nome do cliente.</param>
        public void Update(string? name)
        {
            Name = name;
            UpdatedOn = DateTime.Now;
        }
    }
}