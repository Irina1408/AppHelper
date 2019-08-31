using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppView.Help
{
    public class TreeItem : IComparable<TreeItem>
    {
        public TreeItem()
        {
            Children = new List<TreeItem>();
        }

        public TreeItem(Object source, string description)
            : this()
        {
            Description = description;
            Source = source;
        }

        public TreeItem(TreeItem parent, Object source, string description)
            : this(source, description)
        {
            ParentItem = parent;
        }

        public TreeItem ParentItem { get; private set; }
        public List<TreeItem> Children { get; private set; }
        public string Description { get; private set; }
        public Object Source { get; private set; }

        #region IComparable implementation

        public int CompareTo(TreeItem other)
        {
            return System.String.CompareOrdinal(this.Description, other.Description);
        }

        #endregion
    }
}
