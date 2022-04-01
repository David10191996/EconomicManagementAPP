using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts accounts);
        Task<bool> Exist(string Name);
        Task<IEnumerable<Accounts>> getAccounts();
        Task Update(Accounts accounts);
        Task<Accounts> getAccountsById(int Id);
        Task Delete(int Id);
    }

}
