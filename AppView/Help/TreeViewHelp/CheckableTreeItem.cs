using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppView.Help
{
    public class CheckableTreeItem : IComparable<CheckableTreeItem>, INotifyPropertyChanged
    {
        #region Private fields

        private bool? isChecked;

        #endregion

        #region Initialization

        public CheckableTreeItem()
        {
            Children = new List<CheckableTreeItem>();
        }

        public CheckableTreeItem(Object source, string description)
            : this()
        {
            Description = description;
            Source = source;
            IsChecked = false;
        }

        public CheckableTreeItem(CheckableTreeItem parent, Object source, string description)
            : this(source, description)
        {
            ParentItem = parent;
        }

        #endregion

        #region Public properties

        public CheckableTreeItem ParentItem { get; private set; }
        public List<CheckableTreeItem> Children { get; private set; }
        public string Description { get; private set; }
        public Object Source { get; private set; }
        public bool? IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region IComparable implementation

        public int CompareTo(CheckableTreeItem other)
        {
            return System.String.CompareOrdinal(this.Description, other.Description);
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
