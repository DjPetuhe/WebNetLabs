using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.DTO.User
{
    public class UserProjectsDto
    {
        public int UserID { get; set; }
        public IEnumerable<int>? ProjectsID { get; set; }
    }
}
