using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.DTO.User
{
    public class UserTasksDto
    {
        public int UserID { get; set; }
        public IEnumerable<int>? TasksID { get; set; }
    }
}
