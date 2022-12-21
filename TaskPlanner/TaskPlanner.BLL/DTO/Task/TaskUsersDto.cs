using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.DTO.Task
{
    public class TaskUsersDto
    {
        public int TaskID { get; set; }
        public IEnumerable<int>? UsersID { get; set; }
    }
}
