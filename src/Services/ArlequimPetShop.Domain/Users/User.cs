using ArlequimPetShop.SharedKernel;
using ArlequimPetShop.SharedKernel.Enums;
using Microsoft.AspNetCore.Mvc.Formatters;
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
    public class User : AggregateRoot<Guid>
    {
        private string _password;

        public User()
        {
            Logins = new List<UserLogin>();
        }

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

        [RequiredValidator(ErrorMessage = "Id obrigatório")]
        public override Guid Id { get; set; }

        [RequiredValidator(ErrorMessage = "Tipo do usuário obrigatório")]
        public virtual UserTypes Type { get; set; }

        [RequiredValidator(ErrorMessage = "Nome do usuário obrigatório")]
        public virtual string Name { get; set; }

        [RequiredValidator(ErrorMessage = "Email do usuário obrigatório")]
        public virtual string Email { get; set; }

        [StringLength(maximumLength: 50, ErrorMessage = "A senha precisa ter entre {2} e {1} characteres.", MinimumLength = 6)]
        [RequiredValidator(ErrorMessage = "Senha do usuário obrigatório")]
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

        [RequiredValidator(ErrorMessage = "Data de criação do usuário obrigatório")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do usuário obrigatório")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual IList<UserLogin> Logins { get; set; }

        public void SetPassword(string password) => Password = password;

        public string Login(string email, string secret)
        {
            var token = $"Bearer {GenerateToken(email, secret)}";
            var entity = new UserLogin(this, email);

            Logins.Add(entity);

            return token;
        }

        public string GenerateToken(string email, string secret)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("name", email),
                new Claim("role", Util.EnumDescription(Type)),
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

        public void Update(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;

            UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public void Delete()
        {
            DeletedOn = DateTime.Now;
        }
    }
}