using System.ComponentModel.DataAnnotations;

namespace Api.Models.Security
{
    public class Authentication
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
