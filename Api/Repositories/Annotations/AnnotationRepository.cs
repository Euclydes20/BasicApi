using Api.Domain.Annotations;
using Api.Domain.Users;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Annotations
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly DataContext _dataContext;

        public AnnotationRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<Annotation> AddAsync(Annotation annotation)
        {
            await _dataContext.Annotation.AddAsync(annotation);

            await _dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            _dataContext.Annotation.Update(annotation);

            await _dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task DeleteAsync(Annotation annotation)
        {
            _dataContext.Annotation.Remove(annotation);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<IList<Annotation>> GetAsync()
        {
            return await _dataContext.Annotation.AsNoTracking()
                .ToListAsync();
            // ANALISE E ESTUDO DE PERFORMANCE DO JOIN
            /*return (await _dataContext.Annotation.AsNoTracking()
                .Join(_dataContext.User, a => a.UserId, u => u.Id, (a, u) => new
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
            return await _dataContext.Annotation.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == annotationId);
        }

        public async Task<IList<Annotation>> GetByUserAsync(int userId)
        {
            return await _dataContext.Annotation.AsNoTracking()
               .Where(a => a.UserId == userId)
               .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Annotation, bool>> func)
        {
            return await _dataContext.Annotation.AsNoTracking()
                .AnyAsync(func);
        }
    }
}