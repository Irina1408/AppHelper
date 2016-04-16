using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Hide column property in data grid
    /// </summary>
    public class HideInDataDridAttribute : Attribute
    {
        public HideInDataDridAttribute()
        {
            Hide = true;
        }

        public HideInDataDridAttribute(bool hide)
        {
            Hide = hide;
        }

        public bool Hide { get; private set; }
    }
}
