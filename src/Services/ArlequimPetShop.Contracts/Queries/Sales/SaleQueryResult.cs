using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using System.Text.Json.Serialization;
using static ArlequimPetShop.Contracts.Queries.Sales.SaleQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Sales
{
    /// <summary>
    /// Resultado paginado da consulta de vendas.
    /// </summary>
    public class SaleQueryResult : PartialCollection<SaleQueryItem>, IRequestResult
    {
        public SaleQueryResult() { }

        public SaleQueryResult(IList<SaleQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        /// <summary>
        /// Item da lista de vendas.
        /// </summary>
        public class SaleQueryItem
        {
            public SaleQueryItem()
            {
                Client = new SaleClientQueryItem();
                Products = new List<SaleProductQueryItem>();
            }

            /// <summary>
            /// Identificador da venda.
            /// </summary>
            public Guid? Id { get; set; }

            /// <summary>
            /// Identificador do cliente (uso interno).
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
            /// Lista de produtos da venda.
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
            /// Data de atualização da venda.
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}