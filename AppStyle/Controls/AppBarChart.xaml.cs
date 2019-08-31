using System;
using System.Collections;
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
using AppStyle.Controls;
using AppStyle.Utils;

namespace AppStyle.Controls
{
    /// <summary>
    /// Interaction logic for AppBarChart.xaml
    /// </summary>
    public partial class AppBarChart : UserControl, INotifyPropertyChanged
    {
        #region Private fields

        private List<LegendItem> legendItems;
        private Brush[] brushes;

        #endregion

        #region Initialization

        public AppBarChart()
        {
            InitializeComponent();

            legendItems = new List<LegendItem>();

            // set datacontext for legend visibility
            legend.DataContext = this;
        }

        #endregion

        #region Public properties

        #region Property BarsOrientation

        public static readonly DependencyProperty BarsOrientationProperty = DependencyProperty.Register(
            "BarsOrientation", typeof(Orientation), typeof(AppBarChart),
            new PropertyMetadata(Orientation.Vertical));

        public Orientation BarsOrientation
        {
            get { return (Orientation)GetValue(BarsOrientationProperty); }
            set
            {
                SetValue(BarsOrientationProperty, value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Property VerticalPropertyName

        public static readonly DependencyProperty VerticalPropertyNameProperty = DependencyProperty.Register("VerticalPropertyName", typeof(string), typeof(BarChart));

        public string VerticalPropertyName
        {
            get { return GetValue(VerticalPropertyNameProperty) == null ? string.Empty : GetValue(VerticalPropertyNameProperty).ToString(); }
            set
            {
                SetValue(VerticalPropertyNameProperty, value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Property HorizontalPropertyName

        public static readonly DependencyProperty HorizontalPropertyNameProperty = DependencyProperty.Register("HorizontalPropertyName", typeof(string), typeof(BarChart));

        public string HorizontalPropertyName
        {
            get { return GetValue(HorizontalPropertyNameProperty) == null ? string.Empty : GetValue(HorizontalPropertyNameProperty).ToString(); }
            set
            {
                SetValue(HorizontalPropertyNameProperty, value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Property LegendPropertyName

        public static readonly DependencyProperty LegendPropertyNameProperty = DependencyProperty.Register("LegendPropertyName", typeof(string), typeof(BarChart));

        public string LegendPropertyName
        {
            get { return GetValue(LegendPropertyNameProperty) == null ? string.Empty : GetValue(LegendPropertyNameProperty).ToString(); }
            set
            {
                SetValue(LegendPropertyNameProperty, value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Property LegendVisibility

        public static DependencyProperty LegendsVisibilityProperty = DependencyProperty.Register("LegendsVisibility", typeof(Visibility), typeof(BarChart),
            new PropertyMetadata(Visibility.Visible));

        public Visibility LegendVisibility
        {
            get
            {
                return GetValue(LegendsVisibilityProperty) == null ? Visibility.Visible : (Visibility)GetValue(LegendsVisibilityProperty);
            }
            set
            {
                SetValue(LegendsVisibilityProperty, value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Property ValueVisibility

        public static DependencyProperty ValueVisibilityProperty = DependencyProperty.Register("ValueVisibility", typeof(Visibility), typeof(BarChart),
            new FrameworkPropertyMetadata(Visibility.Visible, ValueVisibilityChangedCallBack));

        public Visibility ValueVisibility
        {
            get { return (Visibility)GetValue(ValueVisibilityProperty); }
            set
            {
                SetValue(ValueVisibilityProperty, value);
                NotifyPropertyChanged();
            }
        }

        protected static void ValueVisibilityChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AppBarChart).ValueVisibility = (Visibility)e.NewValue;
        }

        #endregion

        #region Property Items

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
            "Items", typeof(IEnumerable), typeof(AppBarChart),
            new PropertyMetadata(null));

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set
            {
                SetValue(ItemsProperty, value); 
                NotifyPropertyChanged();
            }
        }

        #endregion

        #endregion

        #region Public methods

        #endregion

        #region Private methods

        private void RefreshLegend()
        {
            // clean up
            legendItems.Clear();

            if (Items != null)
            {
                var distinctLegendsCount = 0;

                // update legend items
                foreach (var item in Items)
                {
                    var legendCaption = item.GetType().GetProperty(LegendPropertyName).GetValue(item, null);
                    var legendItem = legendItems.FirstOrDefault(it => it.Caption == legendCaption);

                    if (legendItem == null)
                    {
                        legendItem = new LegendItem {Caption = legendCaption.ToString()};
                        legendItems.Add(legendItem);

                        // update legend type count
                        distinctLegendsCount++;
                    }
                }

                // check brushes
                if (brushes == null || brushes.Length < distinctLegendsCount)
                {
                    brushes = ColorGenerator.GenerateBrashes(distinctLegendsCount);
                }

                // set brushes
                for (int i = 0; i < distinctLegendsCount; i++)
                {
                    legendItems[i].Brush = brushes[i];
                }
            }
        }

        private void Draw()
        {
            if (BarsOrientation == Orientation.Vertical)
            {
                
            }
            else
            {
                // TODO: Orientation.Horizontal
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected class LegendItem : INotifyPropertyChanged
        {
            #region Private fields

            private string caption;
            private Brush brush;

            #endregion

            #region Properties

            public string Caption
            {
                get { return caption; }
                set
                {
                    caption = value;
                    NotifyPropertyChanged();
                }
            }

            public Brush Brush
            {
                get { return brush; }
                set
                {
                    brush = value;
                    NotifyPropertyChanged();
                }
            }

            #endregion

            #region INotifyPropertyChanged implementation

            public event PropertyChangedEventHandler PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        }
    }
}
