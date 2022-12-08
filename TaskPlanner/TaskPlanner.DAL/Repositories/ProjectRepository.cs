using System;
using System.Threading.Tasks;
using TaskPlanner.DAL.Context;
using TaskPlanner.DAL.Entities;
using System.Collections.Generic;
using TaskPlanner.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace TaskPlanner.DAL.Repositories
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly TaskPlannerDbContext _context;

        public ProjectRepository(TaskPlannerDbContext context)
        {
            _context = context;
        }

        public async Task Add(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task DeleteById(int projectID)
        {
            Project? project = await GetById(projectID);
            if (project is null) throw new NullReferenceException(nameof(project));
            Delete(project);
        }

        public void Delete(Project project)
        {
            _context.Projects.Remove(project);
        }

        public void Update(Project project)
        {
            _context.Projects.Attach(project);
            _context.Entry(project).State = EntityState.Modified;
        }

        public Task<List<Project>> GetAll()
        {
            return _context.Projects.ToListAsync();
        }

        public Task<Project?> GetById(int projectID)
        {
            return _context.Projects.FindAsync(projectID).AsTask();
        }
    }
}
