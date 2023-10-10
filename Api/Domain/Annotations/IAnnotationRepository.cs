using System.Linq.Expressions;

namespace Api.Domain.Annotations
{
    public interface IAnnotationRepository
    {
        Task<Annotation> AddAsync(Annotation annotation);
        Task<Annotation> UpdateAsync(Annotation annotation);
        Task RemoveAsync(Annotation annotation);
        Task<IEnumerable<Annotation>> GetAsync();
        Task<Annotation?> GetAsync(int annotationId);
        Task<IEnumerable<Annotation>> GetByUserAsync(int userId);
        Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func);
    }
}
