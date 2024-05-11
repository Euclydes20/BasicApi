using System.Linq.Expressions;

namespace Api.Domain.ComplexEntityAndAggregates
{
    public interface IComplexPrincipalService
    {
        Task<ComplexPrincipal> AddAsync(ComplexPrincipal complexPrincipal);
        Task<ComplexPrincipal> UpdateAsync(ComplexPrincipal complexPrincipal);
        Task DeleteAsync(int complexPrincipalId);
        Task<IList<ComplexPrincipal>> GetAsync();
        Task<ComplexPrincipal?> GetAsync(int complexPrincipalId);
        Task<IList<ComplexPrincipal>> GetAsync(int page, int pageSize, string search = "");
        Task<IList<ComplexPrincipal>> GenerateRandom(int quantity);
        Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func);
    }
}
