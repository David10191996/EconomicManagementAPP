using System.Linq.Expressions;

namespace EconomicManagementAPP.Service
{
    public interface IGenericRepositorie<T> where T : class
    {
        Task Create(T entity); 
        Task<IEnumerable<T>> ListData();
        Task<T> getById(int Id); 
        Task Delete(int Id);
        Task Modify(int Id, T entity);
        Task<bool> Exist(Expression<Func<T, bool>> expression);
    }
}
