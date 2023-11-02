using System.Linq.Expressions;

namespace Api.Domain.Annotations
{
    public interface IAnnotationRepository
    {
        Task<Annotation> AddAsync(Annotation annotation);
        Task<Annotation> UpdateAsync(Annotation annotation);
        Task DeleteAsync(Annotation annotation);
        Task<IList<Annotation>> GetAsync();
        Task<Annotation?> GetAsync(int annotationId);
        Task<IList<Annotation>> GetByUserAsync(int userId);
        Task<bool> ExistsAsync(Expression<Func<Annotation, bool>> func);
    }
}
