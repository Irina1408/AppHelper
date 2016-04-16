using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCommon.Attributes
{
    /// <summary>
    /// For "class" properties. Contains selected value path (of this class).
    /// </summary>
    public class BindSelectionAttribute : Attribute
    {
        public BindSelectionAttribute()
        { }

        public BindSelectionAttribute(string sourcePath)
        {
            SourcePath = sourcePath;
        }

        public string SourcePath { get; private set; }
    }
}
