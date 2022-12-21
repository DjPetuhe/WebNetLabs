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
    public class UserRepository : IUserRepository
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

        public Task<User?> GetByIdWithTasks(int userID)
        {
            return _context.Users.Include(u => u.Tasks)
                                 .FirstOrDefaultAsync(u => u.UserID == userID);
        }

        public Task<User?> GetByIdWithProjects(int userID)
        {
            return _context.Users.Include(u => u.Projects)
                                 .FirstOrDefaultAsync(u => u.UserID == userID);
        }

        public Task<User?> GetByIdWithAll(int userID)
        {
            return _context.Users.Include(u => u.Projects)
                                 .Include(u => u.Tasks)
                                 .FirstOrDefaultAsync(u => u.UserID == userID);
        }

        public Task<User?> GetByUserName(string username)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task AddToProject(int userID, Project project)
        {
            User user = await GetById(userID) ?? throw new NullReferenceException(nameof(user));
            if (user.Projects is null) user.Projects = new List<Project>() { project };
            else user.Projects.Add(project);
        }

        public async Task AddToTask(int userID, Entities.Task task)
        {
            User user = await GetById(userID) ?? throw new NullReferenceException(nameof(user));
            if (user.Tasks is null) user.Tasks = new List<Entities.Task>() { task };
            else user.Tasks.Add(task);
        }

        public async Task DeleteFromProject(int userID, Project project)
        {
            User user = await GetById(userID) ?? throw new NullReferenceException(nameof(user));
            if (user.Projects is null || !user.Projects.Contains(project)) 
                throw new KeyNotFoundException($"Project with id {project.ProjectID} was not found in projects of user with id {userID}");
            else user.Projects.Remove(project);
        }

        public async Task DeleteFromTask(int userID, Entities.Task task)
        {
            User user = await GetById(userID) ?? throw new NullReferenceException(nameof(user));
            if (user.Tasks is null || !user.Tasks.Contains(task)) 
                throw new KeyNotFoundException($"Task with id {task.TaskID} was not found in tasks of user with id {userID}");
            else user.Tasks.Remove(task);
        }
    }
}
