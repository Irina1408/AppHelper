using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppView.Help
{
    /// <summary>
    /// Used for binging in the datagrid columns
    /// </summary>
    public class SimpleBindPropertyInfo
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public Type DataType { get; set; }
        public string Format { get; set; }
        public bool IsReadOnly { get; set; }
        public int Order { get; set; }
    }
}
