using Xunit;
using System;
using System.Linq;
using TaskPlanner.DAL;
using TaskPlanner.DAL.Context;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.BLL.Services;
using TaskPlanner.DAL.Interfaces;
using System.Collections.Generic;
using TaskPlanner.BLL.DTO.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskPlanner.BLL.Tests.Tests
{
    public class TaskServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly TestDbContext _context;

        public TaskServiceTests()
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
        public async void Create_KeyNotFoundException()
        {
            CreateTaskDto task = new()
            {
                ProjectID = -1,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.Create(task));
        }

        [Fact]
        public async void GetById_CorrectResult()
        {
            CreateProjectDto project = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(project);
            CreateTaskDto task = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            var createdTask = await _taskService.Create(task);

            var resultTask = await _taskService.GetById(createdTask.TaskID);

            Assert.NotNull(resultTask);
            Assert.Equal(createdTask.TaskID, resultTask.TaskID);
        }

        [Fact]
        public async void GetById_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetById(-1));
        }

        [Fact]
        public async void GetAll_CorrectResult()
        {
            CreateProjectDto project = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(project);
            CreateTaskDto task1 = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle1",
                Description = "TestDescription1",
                Deadline = DateTime.Today
            };
            CreateTaskDto task2 = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle2",
                Description = "TestDescription2",
                Deadline = DateTime.Today
            };
            var createdTask1 = await _taskService.Create(task1);
            var createdTask2 = await _taskService.Create(task2);

            var result = await _taskService.GetAll();
            var resultTask1 = result.FirstOrDefault(t => t.TaskID == createdTask1.TaskID);
            var resultTask2 = result.FirstOrDefault(t => t.TaskID == createdTask2.TaskID);

            Assert.NotNull(result);
            Assert.NotNull(resultTask1);
            Assert.NotNull(resultTask2);
            Assert.Equal(2, result.Count);
            Assert.Equal(createdTask1.TaskID, resultTask1?.TaskID);
            Assert.Equal(createdTask2.TaskID, resultTask2?.TaskID);
        }

        [Fact]
        public async void Update_CorrectResult()
        {
            CreateProjectDto project = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(project);
            CreateTaskDto task = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            var createdTask = await _taskService.Create(task);

            UpdateTaskDto updateTask = new()
            {
                TaskID = createdTask.TaskID,
                Title = "UpdatedTitle",
                Description = "UpdatedDescription",
                Deadline = DateTime.Today.AddDays(1)
            };
            await _taskService.Update(updateTask);
            var resultTask = await _taskService.GetById(createdProject.ProjectID);

            Assert.NotNull(resultTask);
            Assert.Equal("UpdatedTitle", resultTask.Title);
            Assert.Equal("UpdatedDescription", resultTask.Description);
            Assert.Equal(DateTime.Today.AddDays(1), resultTask.Deadline);
            Assert.Equal(createdTask.ProjectID, resultTask.ProjectID);
            Assert.Equal(createdTask.TaskID, resultTask.TaskID);
        }

        [Fact]
        public async void Update_KeyNotFoundException()
        {
            UpdateTaskDto updateTask = new()
            {
                TaskID = -1,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.Update(updateTask));
        }

        [Fact]
        public async void Delete_CorrectResult()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto task = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            var createdTask = await _taskService.Create(task);

            await _taskService.Delete(createdTask.TaskID);
            var result = await _taskService.GetAll();
            var deletedResult = result.FirstOrDefault(t => t.TaskID == createdTask.TaskID);

            Assert.Null(deletedResult);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async void Delete_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.Delete(-1));
        }

        [Fact]
        public async void GetTaskUsers_CorrectResult()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto task = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today
            };
            var createdTask = await _taskService.Create(task);
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
            await _userService.UpdateUserTaskById(createdUser1.UserID, createdTask.TaskID);
            await _userService.UpdateUserTaskById(createdUser2.UserID, createdTask.TaskID);

            var result = await _taskService.GetTaskUsersById(createdTask.TaskID);

            Assert.NotNull(result);
            Assert.Contains(createdUser1.UserID, result.UsersID);
            Assert.Contains(createdUser2.UserID, result.UsersID);
        }

        [Fact]
        public async void GetTaskUsers_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetTaskUsersById(-1));
        }
    }
}
