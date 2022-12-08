using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskPlanner.DAL.Entities
{
    public class Project
    {
        //Data fields
        public int ProjectID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        //Relations
        public virtual ICollection<Task>? Tasks { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
