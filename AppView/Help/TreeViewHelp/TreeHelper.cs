using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppView.Help.TreeViewHelp
{
    public static class TreeHelper
    {
        public static IEnumerable<CheckableTreeItem> GetChildSelectedItems(CheckableTreeItem treeItem)
        {
            var childSelectedItems = new List<CheckableTreeItem>();
            foreach (var checkableTreeItem in treeItem.Children.Where(item => !item.IsChecked.HasValue || item.IsChecked.Value))
            {
                childSelectedItems.Add(checkableTreeItem);
                childSelectedItems.AddRange(GetChildSelectedItems(checkableTreeItem));
            }

            return childSelectedItems;
        }
    }
}
