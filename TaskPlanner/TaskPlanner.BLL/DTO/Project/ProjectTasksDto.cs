using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.DTO.Project
{
    public class ProjectTasksDto
    {
        public int ProjectID { get; set; }
        public IEnumerable<int>? TasksID { get; set; }
    }
}
