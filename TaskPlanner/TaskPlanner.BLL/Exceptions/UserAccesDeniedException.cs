﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPlanner.BLL.Exceptions
{
    public class UserAccesDeniedException : Exception
    {
        public UserAccesDeniedException() { }

        public UserAccesDeniedException(string message) : base(message) { }

        public UserAccesDeniedException(string message, Exception inner) : base(message, inner) { }
    }
}

