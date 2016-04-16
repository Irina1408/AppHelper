using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Hide property in details screen
    /// </summary>
    public class HideInDetailsAttribute : Attribute
    {
        public HideInDetailsAttribute()
        {
            Hide = true;
        }

        public HideInDetailsAttribute(bool hide)
        {
            Hide = hide;
        }

        public bool Hide { get; private set; }
    }
}
