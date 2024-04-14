namespace Api.Domain.ComplexEntityAndAggregates
{
    public sealed class ComplexAggregate
    {
        public int Id { get; set; }
        public int ComplexPrincipalId { get; set; }
        public string Description { get; set; } = string.Empty;

        public List<ComplexSubAggregate> ComplexSubAggregates { get; set; } = [];
    }
}
