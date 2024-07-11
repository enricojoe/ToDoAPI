using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUserRepository
    {
        public Task<string> AuthenticateUserAsync(User user);
        public Task<List<User>> GetAll();
        public Task<string> AddUserAsync(User user);
        public Task<User?> GetById(int id);
        public Task<User?> GetByName(string name);
        //public Task<User?> AddAndUpdateUser(User user);
    }
}
