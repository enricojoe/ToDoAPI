﻿using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IToDoService
    {
        public Task<List<Todo>> GetAllTodoAsync();
        public Task<Todo> GetTodoByIdAsync(int id);
        public Task<string> AddTodoAsync(Todo todo);
        public Task<string> UpdateTodoAsync(int id, Todo todo);
        public Task<string> DeleteTodoAsync(int id);
    }
}
