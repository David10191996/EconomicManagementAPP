using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieCategories
    {
        Task Create(Categories categories); 
        Task<bool> Exist(string Name);
        Task<IEnumerable<Categories>> getCategories();
        Task Update(Categories categories);
        Task<Categories> getCategoriesById(int Id); 
        Task Delete(int Id);
    }
}
