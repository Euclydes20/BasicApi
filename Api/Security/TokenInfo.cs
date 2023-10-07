namespace Api.Security
{
    public class TokenInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public bool UserSuper { get; set; }
        public bool ProvisoryPassword { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
