using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AppStyle.Controls;

namespace AppView.Help
{
    public static class GridFiller
    {
        public static void FillGrid(Grid grid, IEnumerable<BindPropertyInfo> detailsInfo, IEnumerable<string> excudedItems = null)
        {
            var colDef = new ColumnDefinition() { Width = GridLength.Auto };
            var colDef1 = new ColumnDefinition() { Width = GridLength.Auto };

            grid.ColumnDefinitions.Add(colDef);
            grid.ColumnDefinitions.Add(colDef1);

            foreach (var bindPropertyInfo in detailsInfo.Where(item => excudedItems == null || (excudedItems != null && !excudedItems.Contains(item.Name))).OrderBy(item => item.Order))
            {
                if (bindPropertyInfo.HasVariants)
                {
                    AddToGrid.ComboBox(grid, bindPropertyInfo);
                }
                else if (bindPropertyInfo.DataType == typeof(bool))
                {
                    AddToGrid.CheckBox(grid, bindPropertyInfo);
                }
                else if (bindPropertyInfo.DataType == typeof(DateTime))
                {
                    AddToGrid.DatePicker(grid, bindPropertyInfo);
                }
                else if (bindPropertyInfo.DataType == typeof(int))
                {
                    AddToGrid.NumericBox(grid, bindPropertyInfo, MaskType.Integer);
                }
                else if (bindPropertyInfo.DataType == typeof(decimal)
                    || bindPropertyInfo.DataType == typeof(double))
                {
                    AddToGrid.NumericBox(grid, bindPropertyInfo, MaskType.Decimal);
                }
                else //if (BindPropertyInfo.DataType == typeof (string))
                {
                    AddToGrid.TextBox(grid, bindPropertyInfo);
                }
            }
        }
    }
}
