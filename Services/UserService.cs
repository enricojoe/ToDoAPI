using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<string> AuthenticateUserAsync(User user)
        {
            return _userRepository.AuthenticateUserAsync(user);
        }
        public Task<List<User>> GetAll()
        {
            return _userRepository.GetAll();
        }
        public Task<string> AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user);
        }
        public Task<User?> GetById(int id)
        {
            return _userRepository.GetById(id);
        }
        public Task<User?> GetByName(string name)
        {
            return _userRepository.GetByName(name);
        }
    }
}
