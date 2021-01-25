using System;
using System.Collections.Generic;
using System.Text;

namespace MB2_Workbench.Classes.Exceptions
{
    public class EnumNotFoundException : Exception
    {
        public EnumNotFoundException()
        {

        }

        public EnumNotFoundException(string message)
            : base(message)
        {
        }

        public EnumNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
