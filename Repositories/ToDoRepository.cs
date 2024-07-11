using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Dapper;

namespace Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly IConfiguration _configuration;
        public ToDoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
        public async Task<List<Todo>> GetAllTodoAsync()
        {
            using (IDbConnection connection = Connection)
            {
                var result = await connection.QueryAsync<Todo>("SELECT * FROM [todo]");
                return result.ToList();
            }
        }
        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            using (IDbConnection connection = Connection)
            {
                var result = await connection.QueryFirstOrDefaultAsync<Todo>("SELECT * FROM [todo] WHERE id = @id", new { id });
                return result;
            }
        }
        public async Task<string> AddTodoAsync(Todo todo)
        {
            using (IDbConnection connection = Connection)
            {
                var query = "INSERT INTO [todo] (name, urgent, done) VALUES(@name, @urgent, @done)";
                var result = await connection.ExecuteAsync(query, todo);
                if (result > 0)
                {
                    return "Todo added successfully.";
                }
                else
                {
                    return "Failed to add Todo.";
                }
            }
        }
        public async Task<string> UpdateTodoAsync(int id, Todo todo)
        {
            using (IDbConnection connection = Connection)
            {
                todo.id = id;
                var query = "UPDATE [todo] SET name = @name, urgent = @urgent, done = @done WHERE id = @id";
                var result = await connection.ExecuteAsync(query, todo);
                if (result > 0)
                {
                    return "Todo updated successfully.";
                }
                else
                {
                    return "Failed to update Todo.";
                }
            }
        }
        public async Task<string> DeleteTodoAsync(int id)
        {
            using (IDbConnection connection = Connection)
            {
                var query = "DELETE FROM [todo] WHERE id = @id";
                var result = await connection.ExecuteAsync(query, new { id });
                if (result > 0)
                {
                    return "Todo deleted successfully.";
                }
                else
                {
                    return "Failed to delete todo.";
                }
            }
        }
    }
}
