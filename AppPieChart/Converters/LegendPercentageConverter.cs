using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using AppPieChart.PieChart;

namespace AppPieChart.Converters
{
    public class LegendPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // the item which we are displaying is bound to the Tag property
            TextBlock label = (TextBlock)value;
            object item = label.Tag;

            // find the item container
            DependencyObject container = (DependencyObject)Helpers.FindElementOfTypeUp((Visual)value, typeof(ListBoxItem));

            // locate the items control which it belongs to
            ItemsControl owner = ItemsControl.ItemsControlFromItemContainer(container);

            // locate the legend
            Legend legend = (Legend)Helpers.FindElementOfTypeUp(owner, typeof(Legend));

            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(legend.DataContext);
            if (myCollectionView == null) return null;

            PropertyDescriptorCollection filterPropDesc = TypeDescriptor.GetProperties(item);

            double totalValue = 0;

            try
            {
                foreach (var viewItem in myCollectionView)
                {
                    totalValue += Double.Parse(filterPropDesc[legend.PlottedProperty].GetValue(viewItem).ToString());
                }
            }
            catch (Exception)
            {
                return null;
            }

            double itemValue = Double.Parse(filterPropDesc[legend.PlottedProperty].GetValue(item).ToString());
            return totalValue != 0 ? itemValue / totalValue : 0;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
