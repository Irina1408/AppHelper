using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Set read only property
    /// </summary>
    public class IsReadOnlyAttribute : Attribute
    {
        public IsReadOnlyAttribute()
        {
            IsReadOnly = true;
        }

        public IsReadOnlyAttribute(bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; private set; }
    }
}
