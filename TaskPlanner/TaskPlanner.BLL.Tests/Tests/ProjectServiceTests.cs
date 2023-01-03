using Xunit;
using System;
using System.Linq;
using TaskPlanner.DAL;
using TaskPlanner.DAL.Context;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.BLL.Services;
using TaskPlanner.BLL.DTO.Task;
using System.Collections.Generic;
using TaskPlanner.DAL.Interfaces;
using TaskPlanner.BLL.DTO.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskPlanner.BLL.Tests.Tests
{
    public class ProjectServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly TestDbContext _context;

        public ProjectServiceTests()
        {
            var builder = new DbContextOptionsBuilder<TaskPlannerDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            builder.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            builder.EnableSensitiveDataLogging();

            _context = new TestDbContext(builder.Options);
            _unitOfWork = new UnitOfWork(_context);
            _projectService = new ProjectService(_unitOfWork);
            _taskService = new TaskService(_unitOfWork);
            _userService = new UserService(_unitOfWork);
        }

        [Fact]
        public async void GetById_CorrectResult()
        {
            CreateProjectDto project = new()
            {
                Name = "TestProject"
            };
            var createdProject = await _projectService.Create(project);

            var resultProject = await _projectService.GetById(createdProject.ProjectID);

            Assert.NotNull(resultProject);
            Assert.Equal(createdProject.ProjectID, resultProject.ProjectID);
        }

        [Fact]
        public async void GetById_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.GetById(-1));
        }

        [Fact]
        public async void GetAll_CorrectResult()
        {
            CreateProjectDto project1 = new()
            {
                Name = "TestProject1"
            };
            CreateProjectDto project2 = new()
            {
                Name = "TestProject2"
            };
            var createdProject1 = await _projectService.Create(project1);
            var createdProject2 = await _projectService.Create(project2);

            var result = await _projectService.GetAll();
            var resultProject1 = result.FirstOrDefault(p => p.ProjectID == createdProject1.ProjectID);
            var resultProject2 = result.FirstOrDefault(p => p.ProjectID == createdProject2.ProjectID);

            Assert.NotNull(result);
            Assert.NotNull(resultProject1);
            Assert.NotNull(resultProject2);
            Assert.Equal(2, result.Count);
            Assert.Equal(createdProject1.ProjectID, resultProject1?.ProjectID);
            Assert.Equal(createdProject2.ProjectID, resultProject2?.ProjectID);
        }

        [Fact]
        public async void Update_CorrectResult()
        {
            CreateProjectDto createProject = new()
            {
                Name = "TestProject"
            };
            var createdProject = await _projectService.Create(createProject);

            UpdateProjectDto updateProject = new()
            {
                ProjectID = createdProject.ProjectID,
                Name = "UpdatedTestProject"
            };
            await _projectService.Update(updateProject);
            var resultProject = await _projectService.GetById(createdProject.ProjectID);

            Assert.NotNull(resultProject);
            Assert.Equal("UpdatedTestProject", resultProject.Name);
            Assert.Equal(createdProject.ProjectID, resultProject.ProjectID);
        }

        [Fact]
        public async void Update_KeyNotFoundException()
        {
            UpdateProjectDto updateProject = new()
            {
                ProjectID = -1,
                Name = "TestProject"
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.Update(updateProject));
        }

        [Fact]
        public async void Delete_CorrectResult()
        {
            CreateProjectDto createProject = new()
            {
                Name = "TestProject"
            };
            var createdProject = await _projectService.Create(createProject);

            await _projectService.Delete(createdProject.ProjectID);
            var result = await _projectService.GetAll();
            var deletedResult = result.FirstOrDefault(p => p.ProjectID == createdProject.ProjectID);

            Assert.Null(deletedResult);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async void Delete_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.Delete(-1));
        }

        [Fact]
        public async void GetProjectTasks_CorrectResult()
        {
            CreateProjectDto createProject = new()
            {
                Name = "TestProject"
            };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask1 = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle1",
                Description = "TestDescription1",
                Deadline = DateTime.Today,
            };
            CreateTaskDto createTask2 = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Deadline = DateTime.Today,
            };
            var createdTask1 = await _taskService.Create(createTask1);
            var createdTask2 = await _taskService.Create(createTask2);

            var result = await _projectService.GetProjectTasksById(createdProject.ProjectID);

            Assert.NotNull(result);
            Assert.Contains(createdTask1.TaskID, result.TasksID);
            Assert.Contains(createdTask2.TaskID, result.TasksID);
        }

        [Fact]
        public async void GetProjectTasks_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.GetProjectTasksById(-1));
        }

        [Fact]
        public async void GetProjectUsers_CorrectResult()
        {
            CreateProjectDto createProject = new()
            {
                Name = "TestProject"
            };
            var createdProject = await _projectService.Create(createProject);
            CreateUserDto createUser1 = new()
            {
                FirstName = "TestName1",
                LastName = "TestSurname1",
                UserName = "TestUsername1",
                Passwords = "TestPassword1"
            };
            CreateUserDto createUser2 = new()
            {
                FirstName = "TestName2",
                LastName = "TestSurname2",
                UserName = "TestUsername2",
                Passwords = "TestPassword2"
            };
            var createdUser1 = await _userService.Create(createUser1);
            var createdUser2 = await _userService.Create(createUser2);
            await _userService.UpdateUserProjectById(createdUser1.UserID, createdProject.ProjectID);
            await _userService.UpdateUserProjectById(createdUser2.UserID, createdProject.ProjectID);

            var result = await _projectService.GetProjectUsersById(createdProject.ProjectID);

            Assert.NotNull(result);
            Assert.Contains(createdUser1.UserID, result.UsersID);
            Assert.Contains(createdUser2.UserID, result.UsersID);
        }

        [Fact]
        public async void GetProjectUsers_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _projectService.GetProjectUsersById(-1));
        }
    }
}
