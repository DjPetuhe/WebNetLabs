using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.DTO.Project
{
    public class ProjectUsersDto
    {
        public int ProjectID { get; set; }
        public IEnumerable<int>? UsersID { get; set; }
    }
}
