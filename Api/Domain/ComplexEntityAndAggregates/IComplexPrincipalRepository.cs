using System.Linq.Expressions;

namespace Api.Domain.ComplexEntityAndAggregates
{
    public interface IComplexPrincipalRepository
    {
        Task<ComplexPrincipal> AddAsync(ComplexPrincipal complexPrincipal);
        Task<IList<ComplexPrincipal>> AddAsync(IList<ComplexPrincipal> complexPrincipal);
        Task<ComplexPrincipal> UpdateAsync(ComplexPrincipal complexPrincipal);
        Task DeleteAsync(ComplexPrincipal complexPrincipal);
        Task<IList<ComplexPrincipal>> GetAsync();
        Task<ComplexPrincipal?> GetAsync(int complexPrincipalId);
        Task<IList<ComplexPrincipal>> GetAsync(int page, int pageSize, string search = "");
        Task<IList<ComplexSimpleFK>> GetSimpleFKsAsync();
        Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func);
    }
}
