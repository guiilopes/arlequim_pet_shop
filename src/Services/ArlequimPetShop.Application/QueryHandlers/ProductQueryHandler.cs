using ArlequimPetShop.Contracts.Queries.Products;
using ArlequimPetShop.SharedKernel;
using Dapper;
using Microsoft.Extensions.Configuration;
using SrShut.Cqrs.Requests;
using SrShut.Data;
using System.Text;

namespace ArlequimPetShop.Application.QueryHandlers
{
    public class ProductQueryHandler : BaseDataAccess,
                                       IRequestHandler<ProductQuery, ProductQueryResult>,
                                       IRequestHandler<ProductByIdQuery, ProductByIdQueryResult>

    {
        public ProductQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ProductQueryResult> HandleAsync(ProductQuery query)
        {
            var orderBy = " ORDER BY Name ASC ";

            var sql = $@"
            WITH RecordsRN AS (
            SELECT 
                P.Id,
                P.Name,
                P.Description,
                P.Price,
                PS.Quantity AS StockQuantity,
                P.ExpirationDate,
                P.CreatedOn,
                P.UpdatedOn,
                ROW_NUMBER() OVER({orderBy}) as NumberLine,
                COUNT(*) OVER() AS TotalRows 
            FROM [Product] AS P WITH(NOLOCK)
            INNER JOIN ProductStock AS PS WITH(NOLOCK) ON PS.ProductId = P.Id
            WHERE P.DeletedOn IS NULL
              {Condition(query)}
            ) SELECT * FROM RecordsRN WITH(NOLOCK) WHERE NumberLine between @FirstResult AND @LastResult ORDER BY NumberLine ASC; ";

            var items = new ProductQueryResult();

            using (var con = CreateConnection())
            {
                var results = await con.QueryAsync<dynamic>(sql, query);
                int? totalRows = null;

                foreach (var row in results)
                {
                    if (!totalRows.HasValue)
                        totalRows = row.TotalRows;

                    var item = new ProductQueryResult.ProductQueryItem()
                    {
                        Id = row.Id,
                        Name = row.Name,
                        Description = row.Description,
                        Price = row.Price,
                        ExpirationDate = row.ExpirationDate,
                        StockQuantity = row.StockQuantity,
                        CreatedOn = row.CreatedOn,
                        UpdatedOn = row.UpdatedOn,
                    };

                    items.Itens.Add(item);
                }

                items.TotalCount = totalRows ?? 0;

                return items;
            }
        }

        public async Task<ProductByIdQueryResult> HandleAsync(ProductByIdQuery query)
        {
            var sql = @$"
            SELECT
                P.Id,
                P.Name,
                P.Description,
                P.Price,
                P.ExpirationDate,
                PS.Quantity AS StockQuantity,
                P.CreatedOn,
                P.UpdatedOn
            FROM Product AS P WITH(NOLOCK)
            INNER JOIN ProductStock AS PS WITH(NOLOCK) ON PS.ProductId = P.Id
            WHERE P.DeletedOn IS NULL
              AND PS.DeletedOn IS NULL
              AND P.Id = @Id
            ORDER BY P.Name; ";

            using (var con = CreateConnection())
            {
                return (await con.QueryFirstOrDefaultAsync<ProductByIdQueryResult>(sql, query));
            }
        }

        private string Condition(ProductQuery query)
        {
            var condition = new StringBuilder();

            if (!string.IsNullOrEmpty(query.Text))
            {
                var like = " LIKE '%' + @Text + '%' ";

                condition.Append($@" AND ( P.Id {like} OR P.Name {like} OR P.Description {like} ) ");
            }

            if (query.EndExpirationDate.HasValue)
                query.EndExpirationDate = Date.LastHourOfTheDay(query.EndExpirationDate.Value);

            if (query.StartExpirationDate.HasValue && query.EndExpirationDate.HasValue)
                condition.Append($@" AND (P.ExpirationDate >= @StartExpirationDate AND P.ExpirationDate <= @EndExpirationDate) ");
            else if (query.StartExpirationDate.HasValue && !query.EndExpirationDate.HasValue)
                condition.Append($@" AND (P.ExpirationDate >= @StartExpirationDate) ");
            else if (!query.StartExpirationDate.HasValue && query.EndExpirationDate.HasValue)
                condition.Append($@" AND (P.ExpirationDate <= @EndExpirationDate) ");

            return condition.ToString();
        }
    }
}