using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IUserRepository
    {
        Task<int> AddUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task UpdateUserAsync(User user);
    }
}
