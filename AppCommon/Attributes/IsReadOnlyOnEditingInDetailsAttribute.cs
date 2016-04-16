using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Attribute shows should be editable proberty when is not new when user wants to edit it
    /// </summary>
    public class IsReadOnlyOnEditingInDetailsAttribute: Attribute
    {
        public IsReadOnlyOnEditingInDetailsAttribute()
        {
            IsReadOnly = true;
        }

        public IsReadOnlyOnEditingInDetailsAttribute(bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
        }

        public bool IsReadOnly { get; private set; }
    }
}
