using Api.Domain.ComplexEntityAndAggregates;
using Api.Infra.Database;
using Api.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Repositories.ComplexEntityAndAggregates
{
    public sealed class ComplexPrincipalRepository(DataContextEF dataContextEF) : IComplexPrincipalRepository
    {
        private readonly DataContextEF _dataContextEF  = dataContextEF;

        public async Task<ComplexPrincipal> AddAsync(ComplexPrincipal complexPrincipal)
        {
            complexPrincipal.ClearReadOnlyFKs();

            await _dataContextEF.ComplexPrincipal.AddAsync(complexPrincipal);
            await _dataContextEF.SaveChangesAsync();

            return complexPrincipal;
        }
        
        public async Task<IList<ComplexPrincipal>> AddAsync(IList<ComplexPrincipal> complexPrincipal)
        {
            foreach (ComplexPrincipal cp in complexPrincipal)
                cp.ClearReadOnlyFKs();

            await _dataContextEF.ComplexPrincipal.AddRangeAsync(complexPrincipal);
            await _dataContextEF.SaveChangesAsync();

            return complexPrincipal;
        }

        public async Task<ComplexPrincipal> UpdateAsync(ComplexPrincipal complexPrincipal)
        {
            ComplexPrincipal? originalEntity = await GetAsync(complexPrincipal.Id)
                ?? throw new ResponseException("Entity not found.");

            // REMOVE AS AGREGADAS QUE FORAM RETIRADAS DO CADASTRO APÓS A ATUALIZAÇÃO DA MESMA
            ComplexAggregate? complexAggregateReference = null;
            ComplexSubAggregate? complexSubAggregateReference = null;
            foreach (ComplexAggregate oca in originalEntity.ComplexAggregates)
            {
                if ((complexAggregateReference = complexPrincipal.ComplexAggregates.FirstOrDefault(ca => ca.Id == oca.Id)) is null)
                {
                    _dataContextEF.ComplexAggregate.Remove(oca);
                    continue;
                }

                foreach (ComplexSubAggregate ocsa in oca.ComplexSubAggregates)
                {
                    if ((complexSubAggregateReference = complexAggregateReference.ComplexSubAggregates.FirstOrDefault(csa => csa.Id == ocsa.Id)) is null)
                    {
                        _dataContextEF.ComplexSubAggregate.Remove(ocsa);
                        continue;
                    }
                }
            }

            complexPrincipal.ClearReadOnlyFKs();

            _dataContextEF.ComplexPrincipal.Update(complexPrincipal);
            await _dataContextEF.SaveChangesAsync();

            return complexPrincipal;
        }

        public async Task DeleteAsync(ComplexPrincipal complexPrincipal)
        {
            complexPrincipal.ClearReadOnlyFKs();

            _dataContextEF.ComplexPrincipal.Remove(complexPrincipal);
            await _dataContextEF.SaveChangesAsync();

            // NÃO REMOVE EM CASCATA
            /*await _dataContextEF.ComplexPrincipal.AsNoTracking()
                .Where(cp => cp.Id == complexPrincipalId)
                .ExecuteDeleteAsync();*/
        }

        public async Task<IList<ComplexPrincipal>> GetAsync()
        {
            return await _dataContextEF.ComplexPrincipal.AsNoTracking().AsSplitQuery()
                .Include(cp => cp.ComplexSimpleFK)
                .Include(cp => cp.ComplexAggregates)
                    .ThenInclude(ca => ca.ComplexSubAggregates)
                .ToListAsync();
        }

        public async Task<ComplexPrincipal?> GetAsync(int complexPrincipalId)
        {
            return await _dataContextEF.ComplexPrincipal.AsNoTracking().AsSplitQuery()
                .Include(cp => cp.ComplexSimpleFK)
                .Include(cp => cp.ComplexAggregates)
                    .ThenInclude(ca => ca.ComplexSubAggregates)
                .FirstOrDefaultAsync(cp => cp.Id == complexPrincipalId);
        }

        public async Task<IList<ComplexPrincipal>> GetAsync(int page, int pageSize, string search = "")
        {
            var query = _dataContextEF.ComplexPrincipal.AsNoTracking().AsSplitQuery()
                .Include(cp => cp.ComplexSimpleFK)
                .Include(cp => cp.ComplexAggregates)
                .ThenInclude(ca => ca.ComplexSubAggregates).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(cp => EF.Functions.ILike(EF.Functions.Unaccent(cp.Description), EF.Functions.Unaccent($"%{search}%")));

            return await query.Skip(page - 1)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IList<ComplexSimpleFK>> GetSimpleFKsAsync()
        {
            return await _dataContextEF.ComplexSimpleFK.AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<ComplexPrincipal, bool>> func)
        {
            return await _dataContextEF.ComplexPrincipal.AsNoTracking()
                .AnyAsync(func);
        }
    }
}