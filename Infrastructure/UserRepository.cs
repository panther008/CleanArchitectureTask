using Dapper;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddUserAsync(User user)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = "INSERT INTO Users (Username, PasswordHash, FirstName, LastName, Device, IpAddress, Balance) " +
                          "VALUES (@Username, @PasswordHash, @FirstName, @LastName, @Device, @IpAddress, @Balance)";
                return await connection.ExecuteAsync(sql, user);
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var sql = "SELECT * FROM Users WHERE Username = @Username";
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            using (var connection = _dbContext.CreateConnection())
            {
                var query = @"
            UPDATE Users 
            SET FirstName = @FirstName, 
                LastName = @LastName, 
                PasswordHash = @PasswordHash, 
                Device = @Device, 
                IpAddress = @IpAddress, 
                Balance = @Balance
            WHERE Username = @Username";

                await connection.ExecuteAsync(query, new
                {
                    user.FirstName,
                    user.LastName,
                    user.PasswordHash,
                    user.Device,
                    user.IpAddress,
                    user.Balance,
                    user.Username
                });
            }
        }

    }
}
