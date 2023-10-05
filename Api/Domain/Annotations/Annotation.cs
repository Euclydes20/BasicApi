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
        public int Author { get; set; }
    }
}
