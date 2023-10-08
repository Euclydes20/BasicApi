using Api.Domain.Annotations;
using Api.Domain.Users;
using System.Linq.Expressions;

namespace Api.Services.Annotations
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IAnnotationRepository _annotationRepository;
        private readonly IUserService _userService;

        public AnnotationService(IAnnotationRepository annotationRepository, IUserService userService)
        {
            _annotationRepository = annotationRepository;
            _userService = userService;
        }

        private async Task Validate(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            if (!await _userService.ExistingAsync(u => u.Id == annotation.UserId))
                throw new Exception("Usuário (Autor) não localizado.");
        }

        private async Task IncludeFKs(Annotation annotation)
        {
            if (annotation is null)
                return;

            annotation.User = await _userService.GetAsync(annotation.UserId);
        }

        private async Task IncludeFKs(List<Annotation> annotations)
        {
            if (annotations is null || !annotations.Any())
                return;

            foreach (var annotation in annotations)
                annotation.User = await _userService.GetAsync(annotation.UserId);
        }

        public async Task<Annotation> AddAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            annotation.CreationDate = DateTime.Now;
            annotation.LastChange = DateTime.Now;

            annotation.Validate();
            await Validate(annotation);

            return await _annotationRepository.AddAsync(annotation);
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            if (!await ExistingAsync(a => a.Id != annotation.Id))
                throw new ArgumentNullException("Anotação não localizada.");

            annotation.LastChange = DateTime.Now;

            annotation.Validate();
            await Validate(annotation);

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
            var annotations = (await _annotationRepository.GetAsync())
                .ToList();

            await IncludeFKs(annotations as List<Annotation>);

            return annotations;
        }

        public async Task<Annotation?> GetAsync(int annotationId)
        {
            if (annotationId <= 0)
                throw new ArgumentException("O Id da anotação não foi informado.", nameof(annotationId));

            var annotation =  await _annotationRepository.GetAsync(annotationId);

            await IncludeFKs(annotation);

            return annotation;
        }

        public async Task<bool> ExistingAsync(Expression<Func<Annotation, bool>> func)
        {
            return await _annotationRepository.ExistingAsync(func);
        }
    }
}