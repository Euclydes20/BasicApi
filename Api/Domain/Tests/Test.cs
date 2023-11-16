using LinqToDB.Mapping;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Tests
{
    public class Test
    {
        [Key, PrimaryKey, Identity]
        public int Id { get; set; } = 0;

        public string Text { get; set; } = string.Empty;
    }
}
