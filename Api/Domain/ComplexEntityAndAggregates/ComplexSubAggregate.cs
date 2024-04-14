namespace Api.Domain.ComplexEntityAndAggregates
{
    public sealed class ComplexSubAggregate
    {
        public int Id { get; set; }
        public int ComplexAggregateId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
