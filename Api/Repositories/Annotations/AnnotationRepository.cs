using Api.Domain.Annotations;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Annotations
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly DataContextEF _dataContextEF;

        public AnnotationRepository(DataContextEF dataContext)
        {
            this._dataContextEF = dataContext;
        }

        public async Task<Annotation> AddAsync(Annotation annotation)
        {
            await _dataContextEF.Annotation.AddAsync(annotation);

            await _dataContextEF.SaveChangesAsync();

            return annotation;
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            _dataContextEF.Annotation.Update(annotation);

            await _dataContextEF.SaveChangesAsync();

            return annotation;
        }

        public async Task DeleteAsync(Annotation annotation)
        {
            _dataContextEF.Annotation.Remove(annotation);
            await _dataContextEF.SaveChangesAsync();
        }

        public async Task<IList<Annotation>> GetAsync()
        {
            return await _dataContextEF.Annotation.AsNoTracking()
                .ToListAsync();
            // ANALISE E ESTUDO DE PERFORMANCE DO JOIN
            /*return (await _dataContextEF.Annotation.AsNoTracking()
                .Join(_dataContextEF.User, a => a.UserId, u => u.Id, (a, u) => new
                {
                    Annotation = a,
                    User = u
                }).ToListAsync()).Select(au =>
                {
                    au.Annotation.User = au.User;
                    return au.Annotation;
                }).ToList();*/
        }

        public async Task<Annotation?> GetAsync(int annotationId)
        {
            return await _dataContextEF.Annotation.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == annotationId);
        }

        public async Task<IList<Annotation>> GetByUserAsync(int userId)
        {
            return await _dataContextEF.Annotation.AsNoTracking()
               .Where(a => a.UserId == userId)
               .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Annotation, bool>> func)
        {
            return await _dataContextEF.Annotation.AsNoTracking()
                .AnyAsync(func);
        }
    }
}