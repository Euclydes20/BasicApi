namespace Api.Domain.Users
{
    public sealed class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Bibliography { get; set; }
        public bool Super { get; set; }
        public bool ProvisoryPassword { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool Blocked { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(Name?.Trim()))
                throw new ArgumentNullException(nameof(Name), "Nome não informado.");

            if (Name.Length > 100)
                throw new ArgumentNullException(nameof(Name), "Nome muito grande, limite de caracteres é 100.");

            if (string.IsNullOrEmpty(Login?.Trim()))
                throw new ArgumentNullException(nameof(Login), "Login não informado.");

            if (Login.Length > 30)
                throw new ArgumentNullException(nameof(Login), "Login muito grande, limite de caracteres é 30.");

            if (string.IsNullOrEmpty(Password?.Trim()))
                throw new ArgumentNullException(nameof(Password), "Senha não informada.");

            if (string.IsNullOrEmpty(Bibliography?.Trim()))
                throw new ArgumentNullException(nameof(Password), "Bibliografia não informada.");

            if (Bibliography.Length > 500)
                throw new ArgumentNullException(nameof(Bibliography), "Bibliografia muito grande, limite de caracteres é 500.");

            if (CreationDate <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(CreationDate), "Data de criação é inválida.");

            if (LastLogin <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(LastLogin), "Data de último login é inválida.");
        }

        internal void ClearPassword(bool maskPassword = false)
        {
            Password = maskPassword ? "********" : null;
        }
    }
}
