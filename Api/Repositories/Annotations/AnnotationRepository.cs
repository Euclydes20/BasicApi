using Api.Domain.Annotations;
using Api.Infra.Database;

namespace Api.Repositories.Annotations
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly DataContext dataContext;

        public AnnotationRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Annotation Create(Annotation annotation)
        {
            dataContext.Add(annotation);
            dataContext.SaveChanges();

            return annotation;
        }

        public Annotation Update(Annotation annotation)
        {
            dataContext.Update(annotation);
            dataContext.SaveChanges();

            return annotation;
        }

        public void Delete(int id)
        {
            dataContext.Remove(Get(id));
            dataContext.SaveChanges();
        }

        public IEnumerable<Annotation> Get()
        {
            return dataContext.Annotation.ToList();
        }

        public Annotation Get(int id)
        {
            return dataContext.Annotation
                    .FirstOrDefault(a => a.Id == id);
        }
    }
}
