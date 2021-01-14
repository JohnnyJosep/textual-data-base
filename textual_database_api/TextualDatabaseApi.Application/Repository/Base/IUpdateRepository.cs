namespace TextualDatabaseApi.Application.Repository.Base
{
    public interface IUpdateRepository<in T> where T : class
    {
        void Update(T entity);
    }
}