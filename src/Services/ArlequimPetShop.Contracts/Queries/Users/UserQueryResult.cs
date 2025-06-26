using SrShut.Common.Collections;
using SrShut.Cqrs.Requests;
using static ArlequimPetShop.Contracts.Queries.Users.UserQueryResult;

namespace ArlequimPetShop.Contracts.Queries.Users
{
    /// <summary>
    /// Resultado da consulta paginada de usuários.
    /// Contém a coleção de usuários encontrados e a contagem total.
    /// </summary>
    public class UserQueryResult : PartialCollection<UserQueryItem>, IRequestResult
    {
        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public UserQueryResult() { }

        /// <summary>
        /// Construtor que recebe uma lista de usuários.
        /// </summary>
        /// <param name="items">Lista de itens do tipo UserQueryItem.</param>
        public UserQueryResult(IList<UserQueryItem> items) : base(items)
        {
            TotalCount = items.Count;
        }

        /// <summary>
        /// Representa um item (usuário) na listagem.
        /// </summary>
        public class UserQueryItem
        {
            /// <summary>
            /// Identificador único do usuário.
            /// </summary>
            public Guid? Id { get; set; }

            /// <summary>
            /// Tipo de usuário (por exemplo: Admin, Cliente).
            /// </summary>
            public string? Type { get; set; }

            /// <summary>
            /// Nome do usuário.
            /// </summary>
            public string? Name { get; set; }

            /// <summary>
            /// Endereço de e-mail do usuário.
            /// </summary>
            public string? Email { get; set; }

            /// <summary>
            /// Data de criação do usuário.
            /// </summary>
            public DateTime? CreatedOn { get; set; }

            /// <summary>
            /// Data da última atualização do usuário.
            /// </summary>
            public DateTime? UpdatedOn { get; set; }
        }
    }
}