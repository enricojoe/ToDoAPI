using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _iTodoRepository;
        public ToDoService(IToDoRepository iTodoRepository)
        { 
            _iTodoRepository = iTodoRepository; 
        }
        public async Task<List<Todo>> GetAllTodoAsync()
        {
            IEnumerable<Todo> todo = await _iTodoRepository.GetAllTodoAsync();
            return todo.ToList();
        }
        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _iTodoRepository.GetTodoByIdAsync(id);
        }
        public async Task<string> AddTodoAsync(Todo todo)
        {
            return await _iTodoRepository.AddTodoAsync(todo);
        }
        public async Task<string> UpdateTodoAsync(int id, Todo todo)
        {
            return await _iTodoRepository.UpdateTodoAsync(id, todo);
        }
        public async Task<string> DeleteTodoAsync(int id)
        {
            return await _iTodoRepository.DeleteTodoAsync(id);
        }
    }
}
