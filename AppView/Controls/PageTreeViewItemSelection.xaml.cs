using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppCommon.Commands;
using AppView.Help;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for PageTreeViewItemSelection.xaml
    /// </summary>
    public partial class PageTreeViewItemSelection : UserControl, INotifyPropertyChanged
    {
        #region Private fields

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand collapseAllCommand;
        private ICommand expandAllCommand;

        #endregion

        #region Initialization

        public PageTreeViewItemSelection()
        {
            InitializeComponent();
        }

        public PageTreeViewItemSelection(IEnumerable<TreeItem> items, Action<TreeItem> closeControlAction = null,  string header = null)
            : this()
        {
            Header = header;
            pageSpecificHeader.Content = header;

            InitializeCommands(closeControlAction);

            RebuildTree(items);
        }

        public void InitializeCommands(Action<TreeItem> closeControlAction)
        {
            CollapseAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    ExpandItemWithChildren((TreeViewItem)item, false);
                }

            }, () => true);

            ExpandAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    ExpandItemWithChildren((TreeViewItem)item, true);
                }

            }, () => true);

            SaveCommand = new Command(() =>
            {
                if (closeControlAction != null)
                    closeControlAction(SelectedItem);

            }, () => SelectedItem != null);

            CancelCommand = new Command(() =>
            {
                // clean selected item 
                SelectedItem = null;

                if (closeControlAction != null)
                    closeControlAction(SelectedItem);

            }, () => true);

            this.DataContext = this;
        }

        #endregion

        #region Public properties

        public TreeView TreeView { get { return treeView; } }
        public TreeItem SelectedItem { get; private set; }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                saveCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set
            {
                cancelCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand CollapseAllCommand
        {
            get { return collapseAllCommand; }
            set
            {
                collapseAllCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ExpandAllCommand
        {
            get { return expandAllCommand; }
            set
            {
                expandAllCommand = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Public methods

        public void RebuildTree(IEnumerable<TreeItem> items)
        {
            treeView.Items.Clear();

            foreach (var item in items)
            {
                // add tree item to tree view
                treeView.Items.Add(BuildTreeViewItem(item));
            }
        }

        #endregion

        #region Event handlers

        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeView.SelectedValue != null)
                SelectedItem = ((TreeViewItem)treeView.SelectedItem).DataContext as TreeItem;
            
            // update commands
            (SaveCommand as Command).ValidateCanExecute();
        }

        #endregion

        #region Private methods

        private TreeViewItem BuildTreeViewItem(TreeItem treeItem)
        {
            // create main item
            var treeViewItem = new TreeViewItem()
            {
                DataContext = treeItem,
                Header = treeItem.Description,
                IsExpanded = true
            };

            // add child items
            foreach (var childTreeItem in treeItem.Children)
            {
                treeViewItem.Items.Add(BuildTreeViewItem(childTreeItem));
            }

            return treeViewItem;
        }

        private void ExpandItemWithChildren(TreeViewItem treeViewItem, bool isExpanded)
        {
            treeViewItem.IsExpanded = isExpanded;

            foreach (var item in treeViewItem.Items)
            {
                ExpandItemWithChildren((TreeViewItem)item, isExpanded);
            }
        }
        
        #endregion

        #region IPage implementation

        public string Header { get; private set; }
        public string SpecificHeader { set { pageSpecificHeader.Content = value; } }
        public UserControl Control { get { return this; } }
        public EventHandler OnRefreshPage { get; set; }
        public void Refresh()
        {
            if (OnRefreshPage != null)
                OnRefreshPage(this, EventArgs.Empty);
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
