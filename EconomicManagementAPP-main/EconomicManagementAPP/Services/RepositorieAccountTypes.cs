using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.Interfaces;
namespace EconomicManagementAPP.Services
{
 
    public class RepositorieAccountTypes : IRepositorieAccountTypes 
    {
        private readonly string connectionString;

        public RepositorieAccountTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
       
        public async Task Create(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
            
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO AccountTypes 
                                                (Name, UserId, OrderAccount) 
                                                VALUES (@Name, @UserId, @OrderAccount); SELECT SCOPE_IDENTITY();", accountTypes);
            accountTypes.Id = id;
        }

        public async Task<bool> Exist(string Name, int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            
            var exist = await connection.QueryFirstOrDefaultAsync<int>( 
                                                            @"SELECT 1
                                                            FROM AccountTypes
                                                            WHERE Name = @Name AND UserId = @UserId;",
                                                            new { Name, UserId });
            return exist == 1;
        }

      
        public async Task<IEnumerable<AccountTypes>> getAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<AccountTypes>(@"SELECT *
                                                            FROM AccountTypes"); 
        }
     
        public async Task Update(AccountTypes accountTypes)
        {
            using var connection = new SqlConnection(connectionString);
                    await connection.ExecuteAsync(@"UPDATE AccountTypes
                                                    SET Name = @Name
                                                    WHERE Id = @Id", accountTypes);
        }

        public async Task<AccountTypes> getAccountById(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountTypes>(@"
                                                                SELECT Id, Name, UserId, OrderAccount
                                                                FROM AccountTypes
                                                                WHERE Id = @Id AND UserID = @UserID",
                                                                new { id, userId });
        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE AccountTypes WHERE Id = @Id", new { id });
        }
    }
}
