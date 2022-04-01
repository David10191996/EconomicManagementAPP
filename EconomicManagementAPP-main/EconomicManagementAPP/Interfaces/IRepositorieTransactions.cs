using EconomicManagementAPP.Models;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieTransactions
    {
        Task Create(Transactions transactions); // Se agrega task por el asincronismo        
        Task<IEnumerable<Transactions>> getTransactions();
        Task Update(Transactions transactions);
        Task<Transactions> getTransactionsById(int Id); // para el modify
        Task Delete(int Id);
    }
}
