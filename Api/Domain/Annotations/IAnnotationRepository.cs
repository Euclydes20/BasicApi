namespace Api.Domain.Annotations
{
    public interface IAnnotationRepository
    {
        Annotation Add(Annotation annotation);
        Annotation Update(Annotation annotation);
        void Remove(int id);
        IEnumerable<Annotation> Get();
        Annotation Get(int id);
    }
}
