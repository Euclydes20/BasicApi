namespace Api.Domain.Users
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool Super { get; set; }
        public bool ProvisoryPassword { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Blocked { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(Name?.Trim()))
                throw new ArgumentNullException(nameof(Name), "Nome não informado.");

            if (string.IsNullOrEmpty(Login?.Trim()))
                throw new ArgumentNullException(nameof(Login), "Login não informado.");

            if (string.IsNullOrEmpty(Password?.Trim()))
                throw new ArgumentNullException(nameof(Password), "Senha não informado.");

            if (CreationDate <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(CreationDate), "Data de criação é inválida.");

            if (LastLogin <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(LastLogin), "Data de último login é inválida.");
        }
    }
}
