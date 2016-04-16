using System;
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
using AppPieChart.PieChart.ColorUtils;
using AppPieChart.Utils;

namespace AppPieChart.PieChart
{
    /// <summary>
    /// Interaction logic for AppPieChart.xaml
    /// </summary>
    public partial class AppPieChart : UserControl
    {
        #region Initialization

        public AppPieChart()
        {
            InitializeComponent();

            this.DataContextChanged += (sender, args) =>
            {
                CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);

                // check ColorSelector
                if (collectionView != null && (ColorSelector == null
                    || ((IndexedColorSelector)ColorSelector).Brushes.Length < collectionView.Count))
                {
                    // create new color selector
                    ColorSelector = new IndexedColorSelector()
                    {
                        Brushes = ColorGenerator.GenerateBrashes(collectionView.Count)
                    };
                }
            };
        }

        #endregion

        #region IColorSelector Property

        /// <summary>
        /// A class which selects a color based on the item being rendered.
        /// </summary>
        public IColorSelector ColorSelector
        {
            get { return GetColorSelector(this); }
            set { SetColorSelector(this, value); }
        }

        // ColorSelector dependency property
        public static readonly DependencyProperty ColorSelectorProperty =
                       DependencyProperty.RegisterAttached("ColorSelector", typeof(IColorSelector), typeof(AppPieChart),
                       new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        // ColorSelector attached property accessors
        public static void SetColorSelector(UIElement element, IColorSelector value)
        {
            element.SetValue(ColorSelectorProperty, value);
        }

        public static IColorSelector GetColorSelector(UIElement element)
        {
            return (IColorSelector)element.GetValue(ColorSelectorProperty);
        }

        #endregion

        #region Title Property

        // TitleProperty dependency property
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.RegisterAttached("Title", typeof (string), typeof (AppPieChart),
                new FrameworkPropertyMetadata("", (o, args) =>
                {
                        (o as AppPieChart).Title = (string)args.NewValue ?? string.Empty;
                }));

        /// <summary>
        /// Title of the chart
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
                title.Content = value;
            }
        }

        #endregion

        #region Plotted Property

        /// <summary>
        /// The property of the bound object that will be plotted (CLR wrapper)
        /// </summary>
        public string PlottedProperty
        {
            get { return GetPlottedProperty(this); }
            set { SetPlottedProperty(this, value); }
        }

        // PlottedProperty dependency property
        public static readonly DependencyProperty PlottedPropertyProperty =
                       DependencyProperty.RegisterAttached("PlottedProperty", typeof(string), typeof(AppPieChart),
                       new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PlottedProperty attached property accessors
        public static void SetPlottedProperty(UIElement element, string value)
        {
            element.SetValue(PlottedPropertyProperty, value);
        }
        public static string GetPlottedProperty(UIElement element)
        {
            return (string)element.GetValue(PlottedPropertyProperty);
        }

        #endregion

        #region PlottedCaption Property

        /// <summary>
        /// Caption of the property of the bound object that will be plotted (CLR wrapper)
        /// </summary>
        public string PlottedCaptionProperty
        {
            get { return GetPlottedProperty(this); }
            set { SetPlottedProperty(this, value); }
        }

        // PlottedCaptionProperty dependency property
        public static readonly DependencyProperty PlottedCaptionPropertyProperty =
                       DependencyProperty.RegisterAttached("PlottedCaptionProperty", typeof(string), typeof(AppPieChart),
                       new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.Inherits));

        // PlottedCaptionProperty attached property accessors
        public static void SetPlottedCaptionProperty(UIElement element, string value)
        {
            element.SetValue(PlottedCaptionPropertyProperty, value);
        }
        public static string GetPlottedCaptionProperty(UIElement element)
        {
            return (string)element.GetValue(PlottedCaptionPropertyProperty);
        }

        #endregion
    }
}
