using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskPlanner.DAL.Entities
{
    public class Task
    {
        //Data fields
        public int TaskID { get; set; }
        [StringLength(50)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int ProjectID { get; set; }
        
        //Relations
        public virtual Project Project { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
