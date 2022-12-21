using System;

namespace TaskPlanner.BLL.Exceptions
{
    public class UserNameDuplicateException : Exception
    {
        public UserNameDuplicateException() { }

        public UserNameDuplicateException(string message) : base(message) { }

        public UserNameDuplicateException(string message, Exception inner) : base(message, inner) { }
    }
}
