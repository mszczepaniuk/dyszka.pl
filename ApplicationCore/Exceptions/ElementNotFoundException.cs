using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Exceptions
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException()
        {
            
        }

        public ElementNotFoundException(string message) : base(message)
        {
                
        }

        public ElementNotFoundException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
