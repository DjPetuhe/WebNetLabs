using System;
using System.Threading.Tasks;
using TaskPlanner.DAL.Context;
using TaskPlanner.DAL.Entities;
using TaskPlanner.DAL.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly TaskPlannerDbContext _context;

        public UserRepository(TaskPlannerDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task DeleteById(int userID)
        {
            User? user = await GetById(userID);
            if (user is null) throw new NullReferenceException(nameof(user));
            Delete(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public void Update(User user)
        {
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
        }

        public Task<List<User>> GetAll()
        {
            return _context.Users.ToListAsync();
        }

        public Task<User?> GetById(int userID)
        {
            return _context.Users.FindAsync(userID).AsTask();
        }

        public Task<User?> GetByUserName(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
