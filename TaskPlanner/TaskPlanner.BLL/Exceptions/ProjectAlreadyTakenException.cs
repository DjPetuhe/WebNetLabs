using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.Exceptions
{
    public class ProjectAlreadyTakenException : Exception
    {
        public ProjectAlreadyTakenException() { }

        public ProjectAlreadyTakenException(string message) : base(message) { }

        public ProjectAlreadyTakenException(string message, Exception inner) : base(message, inner) { }
    }
}
