using Microsoft.Data.SqlClient;
using Dapper;
using EconomicManagementAPP.Models;
using EconomicManagementAPP.Interfaces;

namespace EconomicManagementAPP.Services
{
    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Accounts 
                                                (Name, AccountTypeId, Balance, Description) 
                                                VALUES (@Name, @AccountTypeId, @Balance,
                                                @Description); SELECT SCOPE_IDENTITY();", accounts);
            accounts.Id = id;
        }

        public async Task<bool> Exist(string Name)
        {
            using var connection = new SqlConnection(connectionString);

            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                                            @"SELECT 1
                                                            FROM AccountTypes
                                                            WHERE Name = @Name AND UserId = @UserId;",
                                                            new { Name });
            return exist == 1;
        }


        public async Task<IEnumerable<Accounts>> getAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, 
                                                            Balance, Description
                                                            FROM Accounts");
        }

        public async Task Update(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                                            SET Name = @Name, AccountTypeId=@AccountTypeId, 
                                            Balance=@Balance, Description=@Description
                                            HERE Id = @Id", accounts);
        }

        public async Task<Accounts> getAccountsById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Accounts>(@"
                                                                SELECT Id, Name, AccountTypeId, Balance, Description
                                                                FROM Accounts
                                                                WHERE Id = @Id",
                                                                new { id });
        }
        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { id });
        }
    }
}
