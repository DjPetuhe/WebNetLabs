using System;

namespace TaskPlanner.BLL.DTO.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int ProjectID { get; set; }
    }
}
