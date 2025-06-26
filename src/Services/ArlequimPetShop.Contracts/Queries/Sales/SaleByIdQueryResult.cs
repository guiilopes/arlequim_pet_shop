using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Resultado da consulta de venda por ID.
    /// </summary>
    public class SaleByIdQueryResult : IRequestResult
    {
        /// <summary>
        /// Inicializa uma nova instância do resultado da consulta.
        /// </summary>
        public SaleByIdQueryResult()
        {
            Client = new SaleClientQueryItem();
            Products = new List<SaleProductQueryItem>();
        }

        /// <summary>
        /// Identificador da venda.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Identificador do cliente da venda (uso interno).
        /// </summary>
        [JsonIgnore]
        public Guid? ClientId { get; set; }

        /// <summary>
        /// Nome do cliente (uso interno).
        /// </summary>
        [JsonIgnore]
        public string? ClientName { get; set; }

        /// <summary>
        /// Documento do cliente (uso interno).
        /// </summary>
        [JsonIgnore]
        public string? ClientDocument { get; set; }

        /// <summary>
        /// Informações do cliente.
        /// </summary>
        public SaleClientQueryItem? Client { get; set; }

        /// <summary>
        /// Lista de produtos vendidos.
        /// </summary>
        public List<SaleProductQueryItem> Products { get; set; }

        /// <summary>
        /// Valor total da venda.
        /// </summary>
        public decimal? TotalPrice { get; set; }

        /// <summary>
        /// Data de criação da venda.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Data da última atualização da venda.
        /// </summary>
        public DateTime? UpdatedOn { get; set; }
    }
}