using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Exceptions
{
    public class ReferenceConstraintException : Exception
    {
        public ReferenceConstraintException()
            : base()
        {

        }

        public ReferenceConstraintException(string message)
            : base(message)
        {

        }

        public ReferenceConstraintException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
