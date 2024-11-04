using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp4
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<TaskModel>> GetAllTasksAsync()
        {
            const string query = "SELECT * FROM Tasks";
            using (var connection = CreateConnection())
            {
                return await connection.QueryAsync<TaskModel>(query);
            }
        }

        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            const string query = "SELECT * FROM Tasks WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<TaskModel>(query, new { Id = id });
            }
        }

        public async Task<int> AddTaskAsync(TaskModel task)
        {
            const string query = @"INSERT INTO Tasks (Title, Description, DueDate, IsCompleted) 
                               VALUES (@Title, @Description, @DueDate, @IsCompleted); 
                               SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(query, task);
            }
        }

        public async Task<int> UpdateTaskAsync(TaskModel task)
        {
            const string query = @"UPDATE Tasks SET 
                               Title = @Title, 
                               Description = @Description, 
                               DueDate = @DueDate, 
                               IsCompleted = @IsCompleted 
                               WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(query, task);
            }
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            const string query = "DELETE FROM Tasks WHERE Id = @Id";
            using (var connection = CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
