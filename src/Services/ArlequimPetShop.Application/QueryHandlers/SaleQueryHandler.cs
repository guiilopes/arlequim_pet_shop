using ArlequimPetShop.Contracts.Queries.Sales;
using Dapper;
using Microsoft.Extensions.Configuration;
using SrShut.Cqrs.Requests;
using SrShut.Data;
using System.Text;

namespace ArlequimPetShop.Application.QueryHandlers
{
    /// <summary>
    /// Manipulador de consultas de vendas, responsável por retornar resultados paginados ou detalhados de vendas e seus produtos.
    /// </summary>
    public class SaleQueryHandler : BaseDataAccess,
                                    IRequestHandler<SaleQuery, SaleQueryResult>,
                                    IRequestHandler<SaleByIdQuery, SaleByIdQueryResult>
    {
        /// <summary>
        /// Construtor do manipulador de vendas, com injeção da configuração para conexão.
        /// </summary>
        public SaleQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Retorna a lista paginada de vendas, com dados do cliente e produtos vendidos.
        /// </summary>
        /// <param name="query">Parâmetros de paginação e filtro.</param>
        /// <returns>Resultado com lista de vendas e total.</returns>
        public async Task<SaleQueryResult> HandleAsync(SaleQuery query)
        {
            var orderBy = " ORDER BY S.CreatedOn DESC ";

            var sql = $@"
            WITH RecordsRN AS (
                SELECT 
                    S.Id,
                    S.CreatedOn,
                    S.UpdatedOn,
                    C.Id AS ClientId,
                    C.Name AS ClientName,
                    C.Document AS ClientDocument,
                    S.TotalPrice,
                    ROW_NUMBER() OVER({orderBy}) AS NumberLine,
                    COUNT(*) OVER() AS TotalRows 
                FROM Sale AS S WITH(NOLOCK)
                INNER JOIN Client AS C WITH(NOLOCK) ON C.Id = S.ClientId
                WHERE S.DeletedOn IS NULL
                  AND C.DeletedOn IS NULL
                  {Condition(query)}
            )
            SELECT * FROM RecordsRN WITH(NOLOCK)
            WHERE NumberLine BETWEEN @FirstResult AND @LastResult
            ORDER BY NumberLine ASC;

            SELECT
                SP.SaleId,
                P.Id,
                P.Name,
                P.Description,
                P.ExpirationDate,
                SP.Quantity,
                P.Price,
                SP.Discount,
                SP.NetPrice
            FROM SaleProduct AS SP WITH(NOLOCK)
            INNER JOIN Product AS P WITH(NOLOCK) ON P.Id = SP.ProductId
            WHERE SP.DeletedOn IS NULL;";

            using var con = CreateConnection();
            using var multi = await con.QueryMultipleAsync(sql, query);

            var results = new SaleQueryResult();
            var items = (await multi.ReadAsync<SaleQueryResult.SaleQueryItem>()).ToList();

            if (items != null && items.Count > 0)
            {
                var products = (await multi.ReadAsync<SaleProductQueryItem>()).ToList();

                foreach (var item in items)
                {
                    item.Client = new SaleClientQueryItem
                    {
                        Id = item.ClientId,
                        Name = item.ClientName,
                        Document = item.ClientDocument
                    };

                    item.Products.AddRange(products.Where(p => p.SaleId == item.Id));
                    results.Itens.Add(item);
                }
            }

            results.TotalCount = items?.Count ?? 0;
            return results;
        }

        /// <summary>
        /// Retorna os detalhes de uma venda específica, incluindo cliente e produtos vendidos.
        /// </summary>
        /// <param name="query">Consulta com o ID da venda.</param>
        /// <returns>Venda detalhada ou vazia, se não encontrada.</returns>
        public async Task<SaleByIdQueryResult> HandleAsync(SaleByIdQuery query)
        {
            var sql = @$"
            SELECT 
                S.Id,
                S.CreatedOn,
                S.UpdatedOn,
                C.Id AS ClientId,
                C.Name AS ClientName,
                C.Document AS ClientDocument,
                S.TotalPrice
            FROM Sale AS S WITH(NOLOCK)
            INNER JOIN Client AS C WITH(NOLOCK) ON C.Id = S.ClientId
            WHERE S.DeletedOn IS NULL
              AND C.DeletedOn IS NULL
              AND S.Id = @Id;

            SELECT
                SP.SaleId,
                P.Id,
                P.Name,
                P.Description,
                P.ExpirationDate,
                SP.Quantity,
                P.Price,
                SP.Discount,
                SP.NetPrice
            FROM SaleProduct AS SP WITH(NOLOCK)
            INNER JOIN Product AS P WITH(NOLOCK) ON P.Id = SP.ProductId
            WHERE SP.DeletedOn IS NULL
              AND SP.SaleId = @Id;";

            using var con = CreateConnection();
            using var multi = await con.QueryMultipleAsync(sql, query);

            var item = (await multi.ReadAsync<SaleByIdQueryResult>()).FirstOrDefault();

            if (item == null)
                return new SaleByIdQueryResult();

            item.Client = new SaleClientQueryItem
            {
                Id = item.ClientId,
                Name = item.ClientName,
                Document = item.ClientDocument
            };

            item.Products = (await multi.ReadAsync<SaleProductQueryItem>()).ToList();

            return item;
        }

        /// <summary>
        /// Constrói cláusula WHERE dinâmica para busca por texto em ID, nome ou documento do cliente.
        /// </summary>
        private string Condition(SaleQuery query)
        {
            var condition = new StringBuilder();

            if (!string.IsNullOrEmpty(query.Text))
            {
                var like = " LIKE '%' + @Text + '%' ";
                condition.Append($@" AND ( S.Id {like} OR C.Id {like} OR C.Name {like} OR C.Document {like} ) ");
            }

            return condition.ToString();
        }
    }
}