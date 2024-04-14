using Api.Domain.Users;
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
        Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func);
    }
}
