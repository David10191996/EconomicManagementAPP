using EconomicManagementAPP.Models;
namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieOperationTypes
    {
        Task Create(OperationTypes operationTypes);
        Task<bool> Exist(string Description);
        Task<IEnumerable<OperationTypes>> getOperationTypes();
        Task Update(OperationTypes operationTypes);
        Task<OperationTypes> getOperationTypesById(int Id);
        Task Delete(int Id);
    }
}
