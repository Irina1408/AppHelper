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
using AppView.Controls.Interfaces;
using AppView.Help;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for PageTreeViewItemListSelection.xaml
    /// </summary>
    public partial class PageTreeViewItemListSelection : UserControl, INotifyPropertyChanged, IPage
    {
        #region Private fields

        private ICommand saveCommand;
        private ICommand cancelCommand;
        private ICommand selectAll;
        private ICommand clearAll;
        private ICommand collapseAllCommand;
        private ICommand expandAllCommand;
        private bool selectionChanged;
        private Dictionary<CheckableTreeItem, bool?> originalState; 

        #endregion

        #region Initialization

        public PageTreeViewItemListSelection()
        {
            InitializeComponent();
        }

        public PageTreeViewItemListSelection(IEnumerable<CheckableTreeItem> items, Action<IEnumerable<CheckableTreeItem>> closeControlAction = null, string header = null)
            : this()
        {
            Header = header;
            pageSpecificHeader.Content = header;
            Items = items;
            selectionChanged = false;

            // save original state
            originalState = new Dictionary<CheckableTreeItem, bool?>();
            foreach (var checkableTreeItem in Items)
            {
                if (!originalState.ContainsKey(checkableTreeItem))
                    originalState.Add(checkableTreeItem, checkableTreeItem.IsChecked);
            }

            InitializeCommands(closeControlAction);

            RebuildTree(Items);
        }

        public void InitializeCommands(Action<IEnumerable<CheckableTreeItem>> closeControlAction)
        {
            SelectAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    (((TreeViewItem)item).DataContext as CheckableTreeItem).IsChecked = true;
                    SetStateForChildren((((TreeViewItem)item).DataContext as CheckableTreeItem), true);
                    selectionChanged = true;
                    // update commands
                    (SaveCommand as Command).ValidateCanExecute();
                }

            }, () => true);

            ClearAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    (((TreeViewItem)item).DataContext as CheckableTreeItem).IsChecked = false;
                    SetStateForChildren((((TreeViewItem)item).DataContext as CheckableTreeItem), false);
                    selectionChanged = true;
                    // update commands
                    (SaveCommand as Command).ValidateCanExecute();
                }

            }, () => true);

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
                    closeControlAction(Items);

            }, () => selectionChanged);

            CancelCommand = new Command(() =>
            {
                // revert changes
                foreach (var b in originalState)
                {
                    b.Key.IsChecked = b.Value;
                }

                if (closeControlAction != null)
                    closeControlAction(Items);

            }, () => true);

            this.DataContext = this;
        }

        #endregion

        #region Public properties

        public TreeView TreeView { get { return treeView; } }

        public IEnumerable<CheckableTreeItem> Items { get; private set; }

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

        public ICommand SelectAllCommand
        {
            get { return selectAll; }
            set
            {
                selectAll = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand ClearAllCommand
        {
            get { return clearAll; }
            set
            {
                clearAll = value;
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

        public void RebuildTree(IEnumerable<CheckableTreeItem> items)
        {
            treeView.Items.Clear();

            foreach (var item in items)
            {
                // add tree item to tree view
                treeView.Items.Add(BuildTreeViewItem(item));
            }
        }

        #endregion

        #region Private methods

        private TreeViewItem BuildTreeViewItem(CheckableTreeItem treeItem)
        {
            // create header with checkBox
            var header = new CheckBox()
            {
                Content = treeItem.Description,
                IsThreeState = false,
                Margin = new Thickness(2)
            };
            BindingOperations.SetBinding(header, System.Windows.Controls.CheckBox.IsCheckedProperty, new Binding("IsChecked")
            {
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.TwoWay,
                NotifyOnTargetUpdated = true
            });
            header.Click += (sender, args) =>
            {
                SetStateForChildren(treeItem, header.IsChecked);
                UpdateParentState(treeItem);
                selectionChanged = true;
                // update commands
                (SaveCommand as Command).ValidateCanExecute();
            };

            // create main item
            var treeViewItem = new TreeViewItem()
            {
                DataContext = treeItem,
                Header = header,
                IsExpanded = true
            };
            
            // add child items
            foreach (var childTreeItem in treeItem.Children)
            {
                treeViewItem.Items.Add(BuildTreeViewItem(childTreeItem));
            }

            return treeViewItem;
        }

        private void SetStateForChildren(CheckableTreeItem item, bool? state)
        {
            foreach (var child in item.Children)
            {
                child.IsChecked = state;
                SetStateForChildren(child, state);
            }
        }

        private void UpdateParentState(CheckableTreeItem item)
        {
            if (item.ParentItem == null) return;

            bool? parentState = item.IsChecked;

            if (item.ParentItem.Children.Any(child => parentState != child.IsChecked))
            {
                parentState = null;
            }

            item.ParentItem.IsChecked = parentState;

            // update next parent state
            UpdateParentState(item.ParentItem);
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
