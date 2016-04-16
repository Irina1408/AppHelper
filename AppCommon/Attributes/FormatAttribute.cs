using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    public class FormatAttribute : Attribute
    {
        public FormatAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; private set; }
    }
}
