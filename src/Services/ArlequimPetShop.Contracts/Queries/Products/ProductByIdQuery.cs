using SrShut.Cqrs.Requests;

namespace ArlequimPetShop.Contracts.Queries.Products
{
    /// <summary>
    /// Consulta um produto por identificador, nome ou descrição.
    /// </summary>
    public class ProductByIdQuery : IRequest<ProductByIdQueryResult>
    {
        /// <summary>
        /// Inicializa uma nova instância da consulta sem parâmetros.
        /// </summary>
        public ProductByIdQuery() { }

        /// <summary>
        /// Inicializa uma nova instância da consulta com parâmetros opcionais.
        /// </summary>
        /// <param name="id">Identificador único do produto.</param>
        /// <param name="name">Nome do produto.</param>
        /// <param name="description">Descrição do produto.</param>
        public ProductByIdQuery(Guid? id, string? name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Identificador único do produto.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Nome do produto.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descrição do produto.
        /// </summary>
        public string? Description { get; set; }
    }
}