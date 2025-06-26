using ArlequimPetShop.SharedKernel.Enums;
using Microsoft.IdentityModel.Tokens;
using SrShut.Common;
using SrShut.Common.Exceptions;
using SrShut.Cqrs.Domains;
using SrShut.Validation;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ArlequimPetShop.Domain.Users
{
    /// <summary>
    /// Entidade que representa um usuário autenticado no sistema, incluindo autenticação,
    /// logins históricos, validação de senha forte e geração de tokens JWT.
    /// </summary>
    public class User : AggregateRoot<Guid>
    {
        private string _password;

        /// <summary>
        /// Construtor padrão. Inicializa a lista de logins.
        /// </summary>
        public User()
        {
            Logins = new List<UserLogin>();
        }

        /// <summary>
        /// Construtor completo da entidade User.
        /// </summary>
        public User(Guid id, UserTypes type, string name, string email, string password) : this()
        {
            Id = id;
            Type = type;
            Name = name;
            Email = email;
            Password = password;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        [RequiredValidator(ErrorMessage = "Id obrigatório.")]
        public override Guid Id { get; set; }

        [RequiredValidator(ErrorMessage = "Tipo do usuário obrigatório.")]
        public virtual UserTypes Type { get; set; }

        [RequiredValidator(ErrorMessage = "Nome do usuário obrigatório.")]
        public virtual string Name { get; set; }

        [RequiredValidator(ErrorMessage = "Email do usuário obrigatório.")]
        public virtual string Email { get; set; }

        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha precisa ter entre {2} e {1} characteres.")]
        [RequiredValidator(ErrorMessage = "Senha do usuário obrigatório.")]
        public string Password
        {
            get => _password;
            private set
            {
                if (!IsStrongPassword(value, out var error))
                    throw new BusinessException(error);

                _password = value;
            }
        }

        [RequiredValidator(ErrorMessage = "Data de criação do usuário obrigatória.")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do usuário obrigatória.")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Lista de logins realizados por este usuário.
        /// </summary>
        public virtual IList<UserLogin> Logins { get; set; }

        /// <summary>
        /// Atualiza a senha do usuário com nova validação de força.
        /// </summary>
        public void SetPassword(string password) => Password = password;

        /// <summary>
        /// Realiza o login e retorna um token JWT assinado.
        /// </summary>
        /// <param name="email">E-mail do usuário.</param>
        /// <param name="secret">Chave secreta usada para assinar o token.</param>
        /// <returns>Token JWT.</returns>
        public string Login(string email, string secret)
        {
            var token = $"Bearer {GenerateToken(email, secret)}";
            var entity = new UserLogin(this, email);

            Logins.Add(entity);

            return token;
        }

        /// <summary>
        /// Gera um token JWT contendo informações básicas do usuário.
        /// </summary>
        private string GenerateToken(string email, string secret)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("name", email),
                new Claim("role", Text.EnumDescription(Type)),
                new Claim("userId", Id.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: "arlequim",
                audience: "arlequim-petshop",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// Verifica se a senha atende aos critérios de segurança:
        /// - Mínimo de 6 caracteres
        /// - Letra maiúscula
        /// - Letra minúscula
        /// - Número
        /// - Caractere especial
        /// </summary>
        private bool IsStrongPassword(string password, out List<ErrorMessage> error)
        {
            error = new List<ErrorMessage>();

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                error.Add(new ErrorMessage("Password.Error", "A senha deve ter pelo menos 6 caracteres."));
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                error.Add(new ErrorMessage("Password.Error", "A senha deve conter pelo menos uma letra maiúscula."));
                return false;
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                error.Add(new ErrorMessage("Password.Error", "A senha deve conter pelo menos uma letra minúscula."));
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                error.Add(new ErrorMessage("Password.Error", "A senha deve conter pelo menos um número."));
                return false;
            }

            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                error.Add(new ErrorMessage("Password.Error", "A senha deve conter pelo menos um caractere especial."));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Atualiza nome, e-mail e senha do usuário.
        /// </summary>
        public void Update(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        /// <summary>
        /// Marca o usuário como deletado logicamente.
        /// </summary>
        public void Delete()
        {
            DeletedOn = DateTime.Now;
        }
    }
}