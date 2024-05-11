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

            ComplexPrincipal? entityToRemove = await GetAsync(complexPrincipalId)
                ?? throw new ResponseException("Entity not found.");

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

        public async Task<IList<ComplexPrincipal>> GetAsync(int page, int pageSize, string search = "")
        {
            if (page <= 0 || pageSize <= 0)
                return [];

            return await _complexPrincipalRepository.GetAsync(page, pageSize, search);
        }

        public async Task<IList<ComplexPrincipal>> GenerateRandom(int quantity)
        {
            if (quantity <= 0)
                return [];

            IList<ComplexPrincipal> listComplexPrincipal = [];

            IList<ComplexSimpleFK> listSimpleFKs = await _complexPrincipalRepository.GetSimpleFKsAsync();

            Random random = new();
            ComplexPrincipal complexPrincipal;
            ComplexAggregate complexAggregate;
            ComplexSimpleFK complexSimpleFK;
            int i = 0;
            int j = 0;
            int k = 0;
            int complexAggregateQuantity;
            int complexSubAggregateQuantity;
            for (i = 0; i < quantity; i++)
            {
                complexSimpleFK = listSimpleFKs[random.Next(listSimpleFKs.Count)];

                complexPrincipal = new()
                {
                    ComplexSimpleFKId = complexSimpleFK.Id,
                    ComplexSimpleFK = complexSimpleFK,
                    Description = Guid.NewGuid().ToString("N"),
                };

                complexAggregateQuantity = random.Next(1, 20);
                for (j = 0; j < complexAggregateQuantity; j++)
                {
                    complexAggregate = new()
                    {
                        Description = Guid.NewGuid().ToString("N"),
                    };

                    complexSubAggregateQuantity = random.Next(1, 20);
                    for (k = 0; k < complexSubAggregateQuantity; k++)
                    {
                        complexAggregate.ComplexSubAggregates.Add(new()
                        {
                            Description = Guid.NewGuid().ToString("N"),
                        });
                    }

                    complexPrincipal.ComplexAggregates.Add(complexAggregate);
                }

                listComplexPrincipal.Add(complexPrincipal);
            }

            return await _complexPrincipalRepository.AddAsync(listComplexPrincipal);
        }

        public async Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func)
        {
            return await _complexPrincipalRepository.ExistsAsync(func);
        }
    }
}