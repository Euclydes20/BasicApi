using System.Linq.Expressions;

namespace Api.Domain.Annotations
{
    public interface IAnnotationService
    {
        Task<Annotation> AddAsync(Annotation annotation);
        Task<Annotation> UpdateAsync(Annotation annotation);
        Task RemoveAsync(int annotationId);
        Task RemoveAsync(Annotation annotation);
        Task<IList<Annotation>> GetAsync();
        Task<Annotation?> GetAsync(int annotationId);
        Task<IList<Annotation>> GetByUserAsync(int userId);
        Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func);
    }
}
