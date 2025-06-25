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

        public int Id { get; set; }

        public virtual string Email { get; set; }

        [RequiredValidator(ErrorMessage = "Data de criação do loging obrigatório.")]
        public DateTime CreatedOn { get; set; }

        [RequiredValidator(ErrorMessage = "Data de atualização do loging obrigatório.")]
        public DateTime UpdatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual User User { get; set; }
    }
}