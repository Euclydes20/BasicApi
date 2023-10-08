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
            await dataContext.AddAsync(annotation);

            await dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            dataContext.Update(annotation);

            await dataContext.SaveChangesAsync();

            return annotation;
        }

        public async Task RemoveAsync(Annotation annotation)
        {
            dataContext.Remove(annotation);
            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Annotation>> GetAsync()
        {
            return await dataContext.Annotation
                .ToListAsync();
        }

        public async Task<Annotation?> GetAsync(int annotationId)
        {
            return await dataContext.Annotation
                .FirstOrDefaultAsync(a => a.Id == annotationId);
        }

        public async Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func)
        {
            return await dataContext.Annotation
                .AnyAsync(func);
        }
    }
}