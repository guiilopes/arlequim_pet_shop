using SrShut.Validation;

namespace ArlequimPetShop.Domain.Users
{
    public class UserLogin
    {
        public UserLogin() { }

        public UserLogin(User user, string email) : this()
        {
            User = user;
            Email = email;

            CreatedOn = UpdatedOn = DateTime.Now;

            this.Validate();
        }

        public virtual int Id { get; set; }

        [RequiredValidator(ErrorMessage = "Email do loging obrigatório.")]
        public virtual string Email { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do loging obrigatória.")]
        public DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do loging obrigatória.")]
        public DateTime UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual User User { get; set; }
    }
}