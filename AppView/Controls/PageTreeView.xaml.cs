using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for PageTreeView.xaml
    /// </summary>
    public partial class PageTreeView : UserControl, IPage
    {
        #region Initialization

        public PageTreeView()
        {
            InitializeComponent();
        }

        public PageTreeView(string header, bool isTreeContextMenuVisible = true)
            : this()
        {
            Header = header;
            pageSpecificHeader.Content = header;
            treeContextMenu.Visibility = isTreeContextMenuVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public PageTreeView(CommandContainer commandContainer, string header = null, bool isTreeContextMenuVisible = true)
            : this(header, isTreeContextMenuVisible)
        {
            InitializeCommands(commandContainer);
        }

        public PageTreeView(IEnumerable<TreeItem> items, string header = null, bool isTreeContextMenuVisible = true)
            : this()
        {
            Header = header;
            pageSpecificHeader.Content = header;
            treeContextMenu.Visibility = isTreeContextMenuVisible ? Visibility.Visible : Visibility.Collapsed;

            RebuildTree(items);
        }

        public PageTreeView(IEnumerable<TreeItem> items, CommandContainer commandContainer, string header = null, bool isTreeContextMenuVisible = true)
            : this(items, header, isTreeContextMenuVisible)
        {
            InitializeCommands(commandContainer);
        }

        public void InitializeCommands(CommandContainer commandContainer)
        {
            var treeViewCommands = new TreeViewCommandContainer(commandContainer);

            treeViewCommands.CollapseAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    ExpandItemWithChildren((TreeViewItem) item, false);
                }

            }, () => true);

            treeViewCommands.ExpandAllCommand = new Command(() =>
            {
                foreach (var item in treeView.Items)
                {
                    ExpandItemWithChildren((TreeViewItem)item, true);
                }

            }, () => true);

            Commands = treeViewCommands;

            this.DataContext = treeViewCommands;
        }

        #endregion

        #region Public properties

        public TreeView TreeView { get { return treeView; } }

        public CommandContainer Commands { get; private set; }

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

        #region Private methods

        private TreeViewItem BuildTreeViewItem(TreeItem treeItem)
        {
            // create main item
            var treeViewItem = new TreeViewItem()
            {
                DataContext = treeItem.Source,
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
        
    }

    public class TreeViewCommandContainer : CommandContainer
    {
        private ICommand collapseAllCommand;
        private ICommand expandAllCommand;

        public TreeViewCommandContainer()
        { }

        public TreeViewCommandContainer(CommandContainer commandContainer)
        {
            base.AddCommand = commandContainer.AddCommand;
            base.EditCommand = commandContainer.EditCommand;
            base.DeleteCommand = commandContainer.DeleteCommand;
            base.SaveCommand = commandContainer.SaveCommand;
            base.CancelCommand = commandContainer.CancelCommand;
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
    }
}
