using Api.Domain.Annotations;
using Api.Domain.Users;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Annotations
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly DataContext dataContext;

        public AnnotationRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Annotation> AddAsync(Annotation annotation)
        {
            await dataContext.Annotation.AddAsync(annotation);

            await dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            dataContext.Annotation.Update(annotation);

            await dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task DeleteAsync(Annotation annotation)
        {
            dataContext.Annotation.Remove(annotation);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IList<Annotation>> GetAsync()
        {
            return await dataContext.Annotation.AsNoTracking()
                .ToListAsync();
            // ANALISE E ESTUDO DE PERFORMANCE DO JOIN
            /*return (await dataContext.Annotation.AsNoTracking()
                .Join(dataContext.User, a => a.UserId, u => u.Id, (a, u) => new
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
            return await dataContext.Annotation.AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == annotationId);
        }

        public async Task<IList<Annotation>> GetByUserAsync(int userId)
        {
            return await dataContext.Annotation.AsNoTracking()
               .Where(a => a.UserId == userId)
               .ToListAsync();
        }

        public async Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func)
        {
            return await dataContext.Annotation.AsNoTracking()
                .AnyAsync(func);
        }
    }
}