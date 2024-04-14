using Api.Domain.ComplexEntityAndAggregates;
using Api.Models.Exceptions;
using System.Linq.Expressions;

namespace Api.Services.ComplexEntityAndAggregates
{
    public sealed class ComplexPrincipalService(IComplexPrincipalRepository complexPrincipalRepository) : IComplexPrincipalService
    {
        private readonly IComplexPrincipalRepository _complexPrincipalRepository  = complexPrincipalRepository;

        public async Task<ComplexPrincipal> AddAsync(ComplexPrincipal complexPrincipal)
        {
            if (complexPrincipal is null)
                throw new ResponseException("Invalid data.");

            return await _complexPrincipalRepository.AddAsync(complexPrincipal);
        }

        public async Task<ComplexPrincipal> UpdateAsync(ComplexPrincipal complexPrincipal)
        {
            if (complexPrincipal is null)
                throw new ResponseException("Invalid data.");

            return await _complexPrincipalRepository.UpdateAsync(complexPrincipal);
        }

        public async Task DeleteAsync(int complexPrincipalId)
        {
            if (complexPrincipalId <= 0)
                throw new ResponseException("Invalid ID.");

            ComplexPrincipal? entityToRemove = await GetAsync(complexPrincipalId);
            if (entityToRemove is null)
                throw new ResponseException("Entity not found.");

            await _complexPrincipalRepository.DeleteAsync(entityToRemove);
        }

        public async Task DeleteAsync(ComplexPrincipal complexPrincipal)
            => await DeleteAsync(complexPrincipal?.Id ?? 0);

        public async Task<IList<ComplexPrincipal>> GetAsync()
        {
            return await _complexPrincipalRepository.GetAsync();
        }

        public async Task<ComplexPrincipal?> GetAsync(int complexPrincipalId)
        {
            if (complexPrincipalId <= 0)
                throw new ResponseException("Invalid ID.");

            return await _complexPrincipalRepository.GetAsync(complexPrincipalId);
        }

        public async Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func)
        {
            return await _complexPrincipalRepository.ExistsAsync(func);
        }
    }
}