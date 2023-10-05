namespace Api.Domain.Annotations
{
    public interface IAnnotationRepository
    {
        Annotation Create(Annotation annotation);
        Annotation Update(Annotation annotation);
        void Delete(int id);
        IEnumerable<Annotation> Get();
        Annotation Get(int id);
    }
}
