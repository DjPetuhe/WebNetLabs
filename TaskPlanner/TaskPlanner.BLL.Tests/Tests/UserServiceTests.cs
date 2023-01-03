using Xunit;
using System;
using System.Linq;
using TaskPlanner.DAL;
using TaskPlanner.DAL.Context;
using TaskPlanner.BLL.DTO.User;
using TaskPlanner.BLL.Services;
using TaskPlanner.BLL.DTO.Task;
using TaskPlanner.BLL.Exceptions;
using System.Collections.Generic;
using TaskPlanner.DAL.Interfaces;
using TaskPlanner.BLL.DTO.Project;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TaskPlanner.BLL.Tests.Tests
{
    public class UserServiceTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProjectService _projectService;
        private readonly TaskService _taskService;
        private readonly UserService _userService;
        private readonly TestDbContext _context;

        public UserServiceTests()
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
        public async void Create_UsernameDuplicateException()
        {
            CreateUserDto user1 = new()
            {
                FirstName = "TestName1",
                LastName = "TestSurname1",
                UserName = "TestUsername1",
                Passwords = "TestPassword1"
            };
            _ = await _userService.Create(user1);
            CreateUserDto user2 = new()
            {
                FirstName = "TestName2",
                LastName = "TestSurname2",
                UserName = "TestUsername1",
                Passwords = "TestPassword2"
            };

            await Assert.ThrowsAsync<UserNameDuplicateException>(() => _userService.Create(user1));
        }

        [Fact]
        public async void GetById_CorrectResult()
        {
            CreateUserDto user = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(user);

            var resultUser = await _userService.GetById(createdUser.UserID);

            Assert.NotNull(resultUser);
            Assert.Equal(createdUser.UserID, resultUser.UserID);
        }

        [Fact]
        public async void GetById_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetById(-1));
        }

        [Fact]
        public async void GetAll_CorrectResult()
        {
            CreateUserDto user1 = new()
            {
                FirstName = "TestName1",
                LastName = "TestSurname1",
                UserName = "TestUsername1",
                Passwords = "TestPassword1"
            };
            CreateUserDto user2 = new()
            {
                FirstName = "TestName2",
                LastName = "TestSurname2",
                UserName = "TestUsername2",
                Passwords = "TestPassword2"
            };
            var createdUser1 = await _userService.Create(user1);
            var createdUser2 = await _userService.Create(user2);

            var result = await _userService.GetAll();
            var resultUser1 = result.FirstOrDefault(u => u.UserID == createdUser1.UserID);
            var resultUser2 = result.FirstOrDefault(u => u.UserID == createdUser2.UserID);

            Assert.NotNull(result);
            Assert.NotNull(resultUser1);
            Assert.NotNull(resultUser2);
            Assert.Equal(2, result.Count);
            Assert.Equal(createdUser1.UserID, resultUser1?.UserID);
            Assert.Equal(createdUser2.UserID, resultUser2?.UserID);
        }

        [Fact]
        public async void Update_CorrectResult()
        {
            CreateUserDto user = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(user);

            UpdateUserDto updateProject = new()
            {
                UserID = createdUser.UserID,
                FirstName = "UpdatedName",
                LastName = "UpdatedSurname",
                UserName = "UpdatedUsername",
                Passwords = "UpdatedPassword"
            };
            await _userService.Update(updateProject);
            var resultUser = await _userService.GetById(createdUser.UserID);

            Assert.NotNull(resultUser);
            Assert.Equal("UpdatedName", resultUser.FirstName);
            Assert.Equal("UpdatedSurname", resultUser.LastName);
            Assert.Equal("UpdatedUsername", resultUser.UserName);
            Assert.Equal("UpdatedPassword", resultUser.Passwords);
            Assert.Equal(createdUser.UserID, resultUser.UserID);
        }

        [Fact]
        public async void Update_KeyNotFoundException()
        {

            UpdateUserDto updateUser = new()
            {
                UserID = -1,
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.Update(updateUser));
        }

        [Fact]
        public async void Update_UsernameDuplicateException()
        {
            CreateUserDto user1 = new()
            {
                FirstName = "TestName1",
                LastName = "TestSurname1",
                UserName = "TestUsername1",
                Passwords = "TestPassword1"
            };
            CreateUserDto user2 = new()
            {
                FirstName = "TestName2",
                LastName = "TestSurname2",
                UserName = "TestUsername2",
                Passwords = "TestPassword2"
            };
            var createdUser1 = await _userService.Create(user1);
            var createdUser2 = await _userService.Create(user2);

            UpdateUserDto updateUser = new()
            {
                UserID = createdUser2.UserID,
                FirstName = "UpdatedName",
                LastName = "UpdatedSurname",
                UserName = "TestUsername1",
                Passwords = "UpdatedPassword"
            };

            await Assert.ThrowsAsync<UserNameDuplicateException>(() => _userService.Update(updateUser));
        }

        [Fact]
        public async void Delete_CorrectResult()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);

            await _userService.Delete(createdUser.UserID);
            var result = await _userService.GetAll();
            var deletedResult = result.FirstOrDefault(u => u.UserID == createdUser.UserID);

            Assert.Null(deletedResult);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public async void Delete_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.Delete(-1));
        }

        [Fact]
        public async void GetUserProjectsById_CorrectResults()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject1 = new() { Name = "TestProject1" };
            CreateProjectDto createProject2 = new() { Name = "TestProject2" };
            var createdProject1 = await _projectService.Create(createProject1);
            var createdProject2 = await _projectService.Create(createProject2);
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject1.ProjectID);
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject2.ProjectID);

            var result = await _userService.GetUserProjectsById(createdUser.UserID);

            Assert.NotNull(result);
            Assert.Contains(createdProject1.ProjectID, result.ProjectsID);
            Assert.Contains(createdProject2.ProjectID, result.ProjectsID);
        }

        [Fact]
        public async void GetUserProjectsById_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserProjectsById(-1));
        }

        [Fact]
        public async void GetUserTasksById_CorrectResults()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
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
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);
            await _userService.UpdateUserTaskById(createdUser.UserID, createdTask1.TaskID);
            await _userService.UpdateUserTaskById(createdUser.UserID, createdTask2.TaskID);

            var result = await _userService.GetUserTasksById(createdUser.UserID);

            Assert.NotNull(result);
            Assert.Contains(createdTask1.TaskID, result.TasksID);
            Assert.Contains(createdTask2.TaskID, result.TasksID);
        }

        [Fact]
        public async void GetUserTasksById_KeyNotFoundException()
        {
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.GetUserTasksById(-1));
        }

        [Fact]
        public async void UpdateUserProjectById_User_KeyNotFoundException()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateUserProjectById(-1, createdProject.ProjectID));
        }

        [Fact]
        public async void UpdateUserProjectById_Project_KeyNotFoundException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateUserProjectById(createdUser.UserID, -1));
        }

        [Fact]
        public async void UpdateUserProjectById_ProjectAlreadyTakenException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);

            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);
            await Assert.ThrowsAsync<ProjectAlreadyTakenException>(() => _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID));
        }
        
        [Fact]
        public async void UpdateUserTaskById_User_KeyNotFoundException()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateUserTaskById(-1, createdTask.TaskID));
        }

        [Fact]
        public async void UpdateUserTaskById_Task_KeyNotFoundException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.UpdateUserTaskById(createdUser.UserID, -1));
        }

        [Fact]
        public async void UpdateUserTaskById_UserAccessDeniedException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);

            await Assert.ThrowsAsync<UserAccesDeniedException>(() => _userService.UpdateUserTaskById(createdUser.UserID, createdTask.TaskID));
        }

        [Fact]
        public async void UpdateUserTaskById_TaskAlreadyTakenException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);

            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);
            await _userService.UpdateUserTaskById(createdUser.UserID, createdTask.TaskID);

            await Assert.ThrowsAsync<TaskAlreadyTakenException>(() => _userService.UpdateUserTaskById(createdUser.UserID, createdTask.TaskID));
        }

        [Fact]
        public async void DeleteUserProjectById_CorrectResults()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);

            await _userService.DeleteUserProjectById(createdUser.UserID, createdProject.ProjectID);
            var result = await _userService.GetUserProjectsById(createdUser.UserID);

            Assert.DoesNotContain(createdProject.ProjectID, result.ProjectsID);
        }

        [Fact]
        public async void DeleteUserProjectById_Project_KeyNotFoundException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserProjectById(createdUser.UserID, -1));
        }

        [Fact]
        public async void DeleteUserProjectById_User_KeyNotFoundException()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserProjectById(-1, createdProject.ProjectID));
        }

        [Fact]
        public async void DeleteUserProjectById_PrjectNotTakenException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);

            await Assert.ThrowsAsync<ProjectNotTakenException>(() => _userService.DeleteUserProjectById(createdUser.UserID, createdProject.ProjectID));
        }

        [Fact]
        public async void DeleteUserTaskById_CorrectResults()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);
            await _userService.UpdateUserTaskById(createdUser.UserID, createdTask.TaskID);

            await _userService.DeleteUserTaskById(createdUser.UserID, createdTask.TaskID);
            var result = await _userService.GetUserTasksById(createdUser.UserID);

            Assert.DoesNotContain(createdTask.TaskID, result.TasksID);
        }

        [Fact]
        public async void DeleteUserTaskById_Task_KeyNotFoundException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserTaskById(createdUser.UserID, -1));
        }

        [Fact]
        public async void DeleteUserTaskById_User_KeyNotFoundException()
        {
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _userService.DeleteUserTaskById(-1, createdTask.TaskID));
        }

        [Fact]
        public async void DeleteUserTaskById_TaskNotTakenException()
        {
            CreateUserDto createUser = new()
            {
                FirstName = "TestName",
                LastName = "TestSurname",
                UserName = "TestUsername",
                Passwords = "TestPassword"
            };
            var createdUser = await _userService.Create(createUser);
            CreateProjectDto createProject = new() { Name = "TestProject" };
            var createdProject = await _projectService.Create(createProject);
            CreateTaskDto createTask = new()
            {
                ProjectID = createdProject.ProjectID,
                Title = "TestTitle",
                Description = "TestDescription",
                Deadline = DateTime.Today,
            };
            var createdTask = await _taskService.Create(createTask);
            await _userService.UpdateUserProjectById(createdUser.UserID, createdProject.ProjectID);

            await Assert.ThrowsAsync<TaskNotTakenException>(() => _userService.DeleteUserTaskById(createdUser.UserID, createdTask.TaskID));
        }
    }
}
