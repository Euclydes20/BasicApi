using Api.Domain.Annotations;

namespace Api.Services.Annotations
{
    public class AnnotationService : IAnnotationService
    {
        private readonly IAnnotationRepository _annotationRepository;

        public AnnotationService(IAnnotationRepository annotationRepository)
        {
            _annotationRepository = annotationRepository;
        }

        public Annotation Create(Annotation annotation)
        {
            return _annotationRepository.Create(annotation);
        }

        public Annotation Update(Annotation annotation)
        {
            return _annotationRepository.Update(annotation);
        }

        public void Delete(int id)
        {
            _annotationRepository.Delete(id);
        }

        public IEnumerable<Annotation> Get()
        {
            return _annotationRepository.Get();
        }

        public Annotation Get(int id)
        {
            return _annotationRepository.Get(id);
        }
    }
}
