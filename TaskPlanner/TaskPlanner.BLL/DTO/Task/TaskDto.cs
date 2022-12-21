using System;

namespace TaskPlanner.BLL.DTO.Task
{
    public class TaskDto
    {
        public int TaskID { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int ProjectID { get; set; }
    }
}
