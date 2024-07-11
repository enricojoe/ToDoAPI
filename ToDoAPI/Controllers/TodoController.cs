using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using ToDoAPI.Attributes;

namespace ToDoAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("todo/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IToDoService _service;
        public TodoController(IToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<List<Todo>> GetAllTodo()
        {
            return await _service.GetAllTodoAsync();
        }

        [HttpGet("{id}")]
        public async Task<Todo> GetTodoById(int id)
        {
            return await _service.GetTodoByIdAsync(id);
        }

        [HttpPost]
        public async Task<string> AddTodo(Todo todo)
        {
            return await _service.AddTodoAsync(todo);
        }

        [HttpPut("{id}")]
        public async Task<string> EditTodo(int id, Todo todo)
        {
            return await _service.UpdateTodoAsync(id, todo);
        }

        [HttpDelete]
        public async Task<string> DeleteTodo(int id)
        {
            return await _service.DeleteTodoAsync(id);
        }
    }
    [ApiController]
    [Route("auth/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("Register")]
        public Task<string> Register(User user)
        {
            return _userService.AddUserAsync(user);
        }
        [HttpPost("Login")]
        public async Task<string> Login(User user)
        {
            var token = await _userService.AuthenticateUserAsync(user);
            return token;
        }
    }
}
