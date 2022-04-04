using EconomicManagementAPP.Models;
using Microsoft.AspNetCore.Identity;

namespace EconomicManagementAPP.Interfaces
{
    public interface IRepositorieUser 
    {

        Task<int> Create(User user);
        Task<IEnumerable<User>> getUser();
        Task<bool> Exist(string Email);
        Task Delete(int Id);
        Task<User> getUserById(int Id);
        Task UpdateUser(User user);
        Task <User> Login(string Email, string Password);


    }
}
