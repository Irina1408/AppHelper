using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Hide property in details screen only when edited entity is not new
    /// </summary>
    public class HideInDetailsOnEditingAttribute : Attribute
    {
        public HideInDetailsOnEditingAttribute()
        {
            Hide = true;
        }

        public HideInDetailsOnEditingAttribute(bool hide)
        {
            Hide = hide;
        }

        public bool Hide { get; private set; }
    }
}
