using Api.Domain.Annotations;
using Api.Domain.Secutiry;
using Api.Domain.Users;
using System.Linq.Expressions;

namespace Api.Services.Annotations
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IAnnotationRepository _annotationRepository;
        private readonly IUserService _userService;
        private readonly IClaimsRepository _claimsRepository;

        public AnnotationService(IAnnotationRepository annotationRepository, IUserService userService, IClaimsRepository claimsRepository)
        {
            _annotationRepository = annotationRepository;
            _userService = userService;
            _claimsRepository = claimsRepository;
        }

        private async Task Validate(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            if (!await _userService.ExistsAsync(u => u.Id == annotation.UserId))
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
            annotation.UserId = _claimsRepository.UserId ?? 0;

            annotation.Validate();
            await Validate(annotation);

            return await _annotationRepository.AddAsync(annotation);
        }

        public async Task<Annotation> UpdateAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException(nameof(annotation), "Dados da anotação é inválido.");

            var annotationSaved = await _annotationRepository.GetAsync(annotation.Id) 
                ?? throw new ArgumentNullException("Anotação não localizada.");

            annotation.CreationDate = DateTime.Now;
            annotation.LastChange = DateTime.Now;
            annotation.UserId = annotationSaved.UserId;

            annotation.Validate();
            await Validate(annotation);

            return await _annotationRepository.UpdateAsync(annotation);
        }

        public async Task DeleteAsync(int annotationId)
        {
            if (annotationId <= 0)
                throw new ArgumentException("O Id da anotação não foi informado.", nameof(annotationId));

            await DeleteAsync(await GetAsync(annotationId));
        }
        
        public async Task DeleteAsync(Annotation annotation)
        {
            if (annotation is null)
                throw new ArgumentNullException("Anotação não localizada.");

            await _annotationRepository.DeleteAsync(annotation);
        }

        public async Task<IList<Annotation>> GetAsync()
        {
            var userId = _claimsRepository.UserId ?? 0;
            var userSuper = _claimsRepository.UserSuper ?? false;
            if (!userSuper && userId > 0)
                return await GetByUserAsync(userId);

            var annotations = (await _annotationRepository.GetAsync())
                .ToList();

            await IncludeFKs(annotations);

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

        public async Task<IList<Annotation>> GetByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            var annotations = (await _annotationRepository.GetByUserAsync(userId))
                .ToList();

            await IncludeFKs(annotations);

            return annotations;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Annotation, bool>> func)
        {
            return await _annotationRepository.ExistsAsync(func);
        }
    }
}