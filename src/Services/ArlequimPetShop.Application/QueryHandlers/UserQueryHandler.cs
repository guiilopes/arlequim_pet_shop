using ArlequimPetShop.Contracts.Queries.Users;
using Dapper;
using Microsoft.Extensions.Configuration;
using SrShut.Cqrs.Requests;
using SrShut.Data;
using System.Text;

namespace ArlequimPetShop.Application.QueryHandlers
{
    /// <summary>
    /// Manipulador de consultas de usuários, responsável por buscar dados paginados conforme filtros aplicados.
    /// </summary>
    public class UserQueryHandler : BaseDataAccess,
        IRequestHandler<UserQuery, UserQueryResult>
    {
        /// <summary>
        /// Construtor da classe, que injeta a configuração de conexão.
        /// </summary>
        public UserQueryHandler(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Manipula a consulta de usuários com suporte a paginação e filtro textual por ID, nome ou e-mail.
        /// </summary>
        /// <param name="query">Consulta com parâmetros de busca e paginação.</param>
        /// <returns>Resultado contendo os usuários encontrados e contagem total.</returns>
        public async Task<UserQueryResult> HandleAsync(UserQuery query)
        {
            var orderBy = " ORDER BY Name ASC ";

            var sql = $@"
            WITH RecordsRN AS (
                SELECT 
                    U.Id,
                    U.Type,
                    U.Name,
                    U.Email,
                    U.CreatedOn,
                    U.UpdatedOn,
                    ROW_NUMBER() OVER({orderBy}) AS NumberLine,
                    COUNT(*) OVER() AS TotalRows 
                FROM [User] AS U WITH(NOLOCK)
                WHERE U.DeletedOn IS NULL
                  {Condition(query)}
            )
            SELECT * 
            FROM RecordsRN WITH(NOLOCK)
            WHERE NumberLine BETWEEN @FirstResult AND @LastResult
            ORDER BY NumberLine ASC;";

            var items = new UserQueryResult();

            using var con = CreateConnection();
            var results = await con.QueryAsync<dynamic>(sql, query);

            int? totalRows = null;

            foreach (var row in results)
            {
                if (!totalRows.HasValue)
                    totalRows = row.TotalRows;

                var item = new UserQueryResult.UserQueryItem
                {
                    Id = row.Id,
                    Type = row.Type,
                    Name = row.Name,
                    Email = row.Email,
                    CreatedOn = row.CreatedOn,
                    UpdatedOn = row.UpdatedOn,
                };

                items.Itens.Add(item);
            }

            items.TotalCount = totalRows ?? 0;

            return items;
        }

        /// <summary>
        /// Constrói a cláusula WHERE dinamicamente com base no texto da consulta.
        /// </summary>
        /// <param name="query">Query com filtro textual opcional.</param>
        /// <returns>Trecho SQL da cláusula WHERE.</returns>
        private string Condition(UserQuery query)
        {
            var condition = new StringBuilder();

            if (!string.IsNullOrEmpty(query.Text))
            {
                var like = " LIKE '%' + @Text + '%' ";
                condition.Append($@" AND ( U.Id {like} OR U.Name {like} OR U.Email {like} ) ");
            }

            return condition.ToString();
        }
    }
}