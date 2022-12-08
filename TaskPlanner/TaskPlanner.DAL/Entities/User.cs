using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskPlanner.DAL.Entities
{
    public class User
    {
        //Data fields
        public int UserID { get; set; }
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string Passwords { get; set; }

        //Relations
        public virtual ICollection<Project>? Projects { get; set; }
        public virtual ICollection<Task>? Tasks { get; set; }
    }
}
