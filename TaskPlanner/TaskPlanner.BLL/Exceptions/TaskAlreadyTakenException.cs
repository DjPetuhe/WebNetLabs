using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.Exceptions
{
    public class TaskAlreadyTakenException : Exception
    {
        public TaskAlreadyTakenException() { }

        public TaskAlreadyTakenException(string message) : base(message) { }

        public TaskAlreadyTakenException(string message, Exception inner) : base(message, inner) { }
    }
}
