using DAL;

namespace BL
{
    public interface IModel<T> where T : class, IEntity
    {
        int Id { get; }
        T ToEntity();
    }
}
