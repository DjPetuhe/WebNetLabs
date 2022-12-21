using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.Exceptions
{
    public class ProjectNotTakenException : Exception
    {
        public ProjectNotTakenException() { }

        public ProjectNotTakenException(string message) : base(message) { }

        public ProjectNotTakenException(string message, Exception inner) : base(message, inner) { }
    }
}
