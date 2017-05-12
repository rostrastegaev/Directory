namespace DAL
{
    public interface IDataService
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity, new();
        int SaveChanges();
    }
}
