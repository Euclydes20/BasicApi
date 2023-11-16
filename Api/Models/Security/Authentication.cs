namespace Api.Models.Security
{
    public sealed class Authentication
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public bool EncryptedPassword { get; set; } = true;
    }
}
