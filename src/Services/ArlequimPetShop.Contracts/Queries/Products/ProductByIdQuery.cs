using SrShut.Cqrs.Requests;
using System.ComponentModel.DataAnnotations;

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
        /// <param name="text">Identificador do produto.</param>
        public ProductByIdQuery(string? text)
        {
            Text = text;
        }

        /// <summary>
        /// Identificador do produto.
        /// </summary>
        public string? Text { get; set; }
    }
}