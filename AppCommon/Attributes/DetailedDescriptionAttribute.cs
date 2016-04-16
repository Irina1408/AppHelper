using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AppCommon.Attributes
{
    /// <summary>
    /// Detailed description. Shown in tool tips.
    /// </summary>
    public class DetailedDescriptionAttribute : Attribute
    {
        public DetailedDescriptionAttribute()
        { }

        public DetailedDescriptionAttribute(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
