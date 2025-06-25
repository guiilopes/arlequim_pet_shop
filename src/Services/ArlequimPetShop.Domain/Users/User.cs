using ArlequimPetShop.SharedKernel.Enums;
using SrShut.Cqrs.Domains;
using SrShut.Validation;
using System.ComponentModel.DataAnnotations;

namespace ArlequimPetShop.Domain.Users
{
    public class User : AggregateRoot<Guid>
    {
        public User()
        {
            Logins = new List<UserLogin>();
        }

        public User(Guid id, string name, string email, string password) : this()
        {
            Id = id;
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

        [StringLength(maximumLength: 50, ErrorMessage = "Tamanho máximo para o Logradouro é de {1} characteres.", MinimumLength = 6)]
        [RequiredValidator(ErrorMessage = "Senha do usuário obrigatório")]
        public virtual string Password { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do usuário obrigatório")]
        public virtual DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do usuário obrigatório")]
        public virtual DateTime UpdatedOn { get; set; }

        public virtual DateTime? DeletedOn { get; set; }

        public virtual IList<UserLogin> Logins { get; set; }

        public string Login(string email, string passsword)
        {
            var token = GenerateToken(email, passsword);
            var entity = new UserLogin(this, email);

            Logins.Add(entity);

            return token;
        }

        public string GenerateToken(string email, string password)
        {
            return string.Empty;
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