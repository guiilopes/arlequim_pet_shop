using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Query para obter uma venda pelo seu identificador.
    /// </summary>
    public class SaleByIdQuery : IRequest<SaleByIdQueryResult>
    {
        /// <summary>
        /// Construtor vazio (necessário para serialização).
        /// </summary>
        public SaleByIdQuery() { }

        /// <summary>
        /// Construtor que recebe o ID da venda.
        /// </summary>
        /// <param name="id">Identificador da venda.</param>
        public SaleByIdQuery(Guid? id)
        {
            Id = id;
        }

        /// <summary>
        /// Identificador da venda a ser consultada.
        /// </summary>
        [JsonIgnore]
        public Guid? Id { get; set; }
    }
}
