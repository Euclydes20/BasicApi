namespace Api.Domain.ComplexEntityAndAggregates
{
    public sealed class ComplexPrincipal
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        public List<ComplexAggregate> ComplexAggregates { get; set; } = [];
    }
}
