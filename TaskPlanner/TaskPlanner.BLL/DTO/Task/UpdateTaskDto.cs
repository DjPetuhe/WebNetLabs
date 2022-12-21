using System;

namespace TaskPlanner.BLL.DTO.Task
{
    public class UpdateTaskDto
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
    }
}
