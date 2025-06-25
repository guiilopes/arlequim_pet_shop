using ArlequimPetShop.Contracts.Queries.Products;
using ArlequimPetShop.Contracts.Queries.Sales;
using ArlequimPetShop.Domain.Clients;
using ArlequimPetShop.SharedKernel;
using Dapper;
using Microsoft.Extensions.Configuration;
using NHibernate.Criterion;
using SrShut.Cqrs.Requests;
using SrShut.Data;
using System.Text;

namespace ArlequimPetShop.Application.QueryHandlers
{
    public class SaleQueryHandler : BaseDataAccess,
                                    IRequestHandler<SaleQuery, SaleQueryResult>,
                                    IRequestHandler<SaleByIdQuery, SaleByIdQueryResult>

    {
        public SaleQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

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
                ROW_NUMBER() OVER({orderBy}) as NumberLine,
                COUNT(*) OVER() AS TotalRows 
            FROM Sale AS S WITH(NOLOCK)
            INNER JOIN Client AS C WITH(NOLOCK) ON C.Id = S.ClientId
            WHERE S.DeletedOn IS NULL
              AND C.DeletedOn IS NULL
              {Condition(query)}
            ) SELECT * FROM RecordsRN WITH(NOLOCK) WHERE NumberLine between @FirstResult AND @LastResult ORDER BY NumberLine ASC; 

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

            using (var con = CreateConnection())
            {
                using (var multi = await con.QueryMultipleAsync(sql, query))
                {
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

                    results.TotalCount = items?.Count();

                    return results;
                }
            }
        }

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

            using (var con = CreateConnection())
            {
                using (var multi = await con.QueryMultipleAsync(sql, query))
                {
                    var item = (await multi.ReadAsync<SaleByIdQueryResult>()).FirstOrDefault();

                    item.Client = new SaleClientQueryItem
                    {
                        Id = item.ClientId,
                        Name = item.ClientName,
                        Document = item.ClientDocument
                    };

                    if (item == null)
                        return new SaleByIdQueryResult();

                    item.Products = (await multi.ReadAsync<SaleProductQueryItem>()).ToList();

                    return item;
                }
            }
        }

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