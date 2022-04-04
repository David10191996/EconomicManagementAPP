using Dapper;
using EconomicManagementAPP.Models;
using Microsoft.Data.SqlClient;
using EconomicManagementAPP.Interfaces;


namespace EconomicManagementAPP.Services
{

    public class RepositorieUser : IRepositorieUser
    {

        private readonly string connectionString;

        public RepositorieUser(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> Create(User users)
        {
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Users
                                                (Email, StandarEmail, Password) 
                                                VALUES (@Email, @StandarEmail, @Pass); SELECT SCOPE_IDENTITY();", users);
            users.Id = id;

            return id;
        }

        public async Task<User> getUserById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(@"SELECT *
                                                                    FROM Users 
                                                                    WHERE Id = @Id", new { Id });
        }
        public async Task<IEnumerable<User>> getUser()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<User>(@"SELECT Id, Email, StandarEmail FROM Users");

        }

        public async Task<bool> Exist(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                @"SELECT 1 FROM Users 
                WHERE Email= @Email;",
                new { Email });
            return exist ==1;
        }

        public async Task UpdateUser(User user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                            SET Password = @Pass, StandarEmail=@StandarEmail
                                            WHERE Id = @Id", user);
        }


        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { Id });
        }

        public async Task<User> Login(string Email, string Password)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(@"SELECT * FROM Users 
                                                                    WHERE Email = @Email 
                                                                    AND Password=@Password",
                                                                    new { Email, Password });

            
        }
    }
}
