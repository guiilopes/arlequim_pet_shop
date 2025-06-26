using SrShut.Cqrs.Requests;
using SrShut.Data;

namespace ArlequimPetShop.Contracts.Queries.Users
{
    /// <summary>
    /// Query utilizada para listar usuários com paginação e filtro de texto.
    /// </summary>
    public class UserQuery : PaginationCriteria, IRequest<UserQueryResult>
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public UserQuery() { }

        /// <summary>
        /// Construtor que recebe o texto para filtro.
        /// </summary>
        /// <param name="text">Texto utilizado para busca por nome, e-mail ou ID.</param>
        public UserQuery(string? text) : base()
        {
            Text = text;
        }

        /// <summary>
        /// Texto utilizado para realizar a busca.
        /// Pode ser parte do nome, e-mail ou identificador do usuário.
        /// </summary>
        public string? Text { get; set; }
    }
}