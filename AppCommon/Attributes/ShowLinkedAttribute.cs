using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// For "class" properties. Contains property name of current "class" property should be shown
    /// </summary>
    public class ShowLinkedAttribute: Attribute
    {
        public ShowLinkedAttribute()
        { }

        public ShowLinkedAttribute(string properyName)
        {
            PropertyName = properyName;
        }

        public string PropertyName { get; private set; }
    }
}
