using Api.Domain.Annotations;
using Api.Domain.Users;
using System.Linq.Expressions;

namespace Api.Services.Annotations
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IAnnotationRepository _annotationRepository;

        public AnnotationService(IAnnotationRepository annotationRepository)
        {
            _annotationRepository = annotationRepository;
        }

        public async Task<Annotation> AddAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            annotation.Validate();

            return await _annotationRepository.AddAsync(annotation);
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            if (!await ExistingAsync(a => a.Id != annotation.Id))
                throw new ArgumentNullException("Anotação não localizada.");

            annotation.Validate();

            return await _annotationRepository.UpdateAsync(annotation);
        }

        public async Task RemoveAsync(int annotationId)
        {
            if (annotationId <= 0)
                throw new ArgumentException("O Id da anotação não foi informado.", nameof(annotationId));

            await RemoveAsync(await GetAsync(annotationId));
        }
        
        public async Task RemoveAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException("Anotação não localizada.");

            await _annotationRepository.RemoveAsync(annotation);
        }

        public async Task<IEnumerable<Annotation>> GetAsync()
        {
            return await _annotationRepository.GetAsync();
        }

        public async Task<Annotation?> GetAsync(int annotationId)
        {
            if (annotationId <= 0)
                throw new ArgumentException("O Id da anotação não foi informado.", nameof(annotationId));

            return await _annotationRepository.GetAsync(annotationId);
        }

        public async Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func)
        {
            return await _annotationRepository.ExistingAsync(func);
        }
    }
}
