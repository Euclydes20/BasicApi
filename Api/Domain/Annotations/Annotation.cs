using Api.Domain.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Annotations
{
    public class Annotation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastChange { get; set; }
        public int UserId { get; set; }

        [NotMapped]
        public User User { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(Title?.Trim()))
                throw new ArgumentNullException(nameof(Title), "Título não informado.");

            if (string.IsNullOrEmpty(Text?.Trim()))
                throw new ArgumentNullException(nameof(Text), "Texto não informado.");

            if (LastChange <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(Title), "Data de alteração é inválida.");

            if (UserId <= 0)
                throw new ArgumentNullException(nameof(Title), "Usuário (Autor) não localizado.");
        }
    }
}
