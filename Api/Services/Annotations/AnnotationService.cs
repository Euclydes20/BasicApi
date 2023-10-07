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

        public Annotation Add(Annotation annotation)
        {
            return _annotationRepository.Add(annotation);
        }

        public Annotation Update(Annotation annotation)
        {
            return _annotationRepository.Update(annotation);
        }

        public void Remove(int id)
        {
            _annotationRepository.Remove(id);
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
