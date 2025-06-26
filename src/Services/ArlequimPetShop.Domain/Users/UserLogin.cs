using SrShut.Validation;

namespace ArlequimPetShop.Domain.Users
{
    /// <summary>
    /// Representa um login de usuário contendo informações como e-mail, datas de criação e atualização, e referência ao usuário associado.
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Construtor padrão necessário para frameworks de persistência.
        /// </summary>
        public UserLogin() { }

        /// <summary>
        /// Cria um novo login de usuário com base no usuário e e-mail informados.
        /// </summary>
        /// <param name="user">Usuário associado.</param>
        /// <param name="email">E-mail do login.</param>
        public UserLogin(User user, string email) : this()
        {
            User = user;
            Email = email;
            CreatedOn = UpdatedOn = DateTime.Now;
            this.Validate();
        }

        /// <summary>
        /// Identificador único do login.
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// E-mail usado no login.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Email do loging obrigatório.")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Data de criação do login.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Data de criação do loging obrigatória.")]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Data da última atualização do login.
        /// </summary>
        [RequiredValidator(ErrorMessage = "Data de atualização do loging obrigatória.")]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Data de exclusão lógica do login, se aplicável.
        /// </summary>
        public DateTime? DeletedOn { get; set; }

        /// <summary>
        /// Referência ao usuário proprietário do login.
        /// </summary>
        public virtual User User { get; set; }
    }
}