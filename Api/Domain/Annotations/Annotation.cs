using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Annotations
{
    public class Annotation
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime ChangeDate { get; set; }
        public int UserId { get; set; }

        internal void Validate()
        {
            if (string.IsNullOrEmpty(Title?.Trim()))
                throw new ArgumentNullException(nameof(Title), "Título não informado.");

            if (string.IsNullOrEmpty(Text?.Trim()))
                throw new ArgumentNullException(nameof(Text), "Texto não informado.");

            if (ChangeDate <= DateTime.MinValue)
                throw new ArgumentNullException(nameof(Title), "Data de alteração é inválida.");

            if (UserId <= 0)
                throw new ArgumentNullException(nameof(Title), "Usuário (Autor) não localizado.");
        }
    }
}
