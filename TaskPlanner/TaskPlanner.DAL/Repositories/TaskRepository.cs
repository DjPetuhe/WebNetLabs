using System;
using System.Threading.Tasks;
using TaskPlanner.DAL.Context;
using TaskPlanner.DAL.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Repositories
{
    internal class TaskRepository : ITaskRepository
    {
        private readonly TaskPlannerDbContext _context;

        public TaskRepository(TaskPlannerDbContext context)
        {
            _context = context;
        }

        public async Task Add(Entities.Task task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task DeleteById(int taskID)
        {
            Entities.Task? task = await GetById(taskID);
            if (task is null) throw new NullReferenceException(nameof(task));
            Delete(task);
        }

        public void Delete(Entities.Task task)
        {
            _context.Tasks.Remove(task);
        }

        public void Update(Entities.Task task)
        {
            _context.Tasks.Attach(task);
            _context.Entry(task).State = EntityState.Modified;
        }

        public Task<List<Entities.Task>> GetAll()
        {
            return _context.Tasks.ToListAsync();
        }

        public Task<Entities.Task?> GetById(int taskID)
        {
            return _context.Tasks.FindAsync(taskID).AsTask();
        }
    }
}
