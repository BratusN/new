using System.Threading.Tasks;

namespace UserService.DataAccess.Interfaces
{
    public interface IBaseManager<T> where T : class
    {
        Task<T> CreateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<T> Update(T entity);
    }
}
