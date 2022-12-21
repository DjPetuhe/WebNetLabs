using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.Exceptions
{
    public class TaskNotTakenException : Exception
    {
        public TaskNotTakenException() { }

        public TaskNotTakenException(string message) : base(message) { }

        public TaskNotTakenException(string message, Exception inner) : base(message, inner) { }
    }
}
