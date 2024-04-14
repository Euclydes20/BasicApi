using Api.Domain.Users;
using System.Linq.Expressions;

namespace Api.Domain.ComplexEntityAndAggregates
{
    public interface IComplexPrincipalRepository
    {
        Task<ComplexPrincipal> AddAsync(ComplexPrincipal complexPrincipal);
        Task<ComplexPrincipal> UpdateAsync(ComplexPrincipal complexPrincipal);
        Task DeleteAsync(ComplexPrincipal complexPrincipal);
        Task<IList<ComplexPrincipal>> GetAsync();
        Task<ComplexPrincipal?> GetAsync(int complexPrincipalId);
        Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func);
    }
}
