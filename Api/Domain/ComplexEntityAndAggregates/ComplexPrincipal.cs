namespace Api.Domain.ComplexEntityAndAggregates
{
    public sealed class ComplexPrincipal
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;

        private int _complexSimpleFKId;
        public int? ComplexSimpleFKId
        {
            get => _complexSimpleFKId;
            set => _complexSimpleFKId = value ?? 0;
        }

        public List<ComplexAggregate> ComplexAggregates { get; set; } = [];
        public ComplexSimpleFK? ComplexSimpleFK { get; set; }

        public void ClearReadOnlyFKs()
        {
            ComplexSimpleFK = null;
        }
    }
}
