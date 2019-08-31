using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using AppStyle.Utils;

namespace AppStyle.Controls
{
    /// <summary>
    /// Interaction logic for BarChart.xaml
    /// </summary>
    public partial class BarChart : UserControl, INotifyPropertyChanged
    {
        #region Private fields

        private bool _isDataSourceBinded = false;
        private IEnumerable _items = null;
        private Border brdLegends;

        #endregion

        #region Initialization

        public BarChart()
        {
            InitializeComponent();
            this.DataContext = this;
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
                NotifyPropertyChanged("ValueVisibility");
            }
        }
        
        protected static void ValueVisibilityChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as BarChart).ValueVisibility = (Visibility)e.NewValue;
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
                NotifyPropertyChanged("LegendVisibility");
            }
        }

        #endregion

        #region Property ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BarChart),
            new FrameworkPropertyMetadata() { PropertyChangedCallback = ItemsSourceChangedCallBack });

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
                IsDataSourceBinded = value != null;
                Refresh();
            }
        }

        protected static void ItemsSourceChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as BarChart).ItemsSource = (IEnumerable)e.NewValue;
        }

        #endregion

        #region Property Legends

        public static readonly DependencyProperty LegendsProperty = DependencyProperty.Register("Legends", typeof(ObservableCollection<Legend>), typeof(BarChart),
            new PropertyMetadata(new ObservableCollection<Legend>() { }));

        public ObservableCollection<Legend> Legends
        {
            get
            {
                object res = GetValue(LegendsProperty);
                if (res == null)
                    res = new ObservableCollection<Legend>();

                (res as ObservableCollection<Legend>).CollectionChanged += (s, e) => { DrawLegends(); };
                return GetValue(LegendsProperty) == new ObservableCollection<Legend>() ? null : (ObservableCollection<Legend>)GetValue(LegendsProperty);
            }
            set
            {
                SetValue(LegendsProperty, value);
                NotifyPropertyChanged("Legends");
            }
        }

        #endregion

        #region Property CanChangeLegendVisibility

        public static readonly DependencyProperty CanChangeLegendVisibilityProperty = DependencyProperty.Register("CanChangeLegendVisibility", typeof(bool), typeof(BarChart),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (s, e) => { (s as BarChart).CanChangeLegendVisibility = (bool)e.NewValue; }));
        
        public bool CanChangeLegendVisibility
        {
            get
            {
                return (bool)GetValue(CanChangeLegendVisibilityProperty);
            }
            set
            {
                SetValue(CanChangeLegendVisibilityProperty, value);
                NotifyPropertyChanged("CanChangeLegendVisibility");
                Draw();
            }
        }

        #endregion

        #region Properties

        public bool IsDataSourceBinded
        {
            get { return _isDataSourceBinded; }
            private set { _isDataSourceBinded = value; }
        }

        public IEnumerable Items
        {
            get
            {
                if (IsDataSourceBinded)
                    return ItemsSource;
                else
                    return _items;
            }
            set
            {
                if (_items != value)
                {
                    if (IsDataSourceBinded)
                        throw new Exception("Control is in DataSource mode.");

                    _items = value;
                    NotifyPropertyChanged("Items");
                    GetLegends();
                    Draw();
                }
            }
        }

        #endregion 
        
        #region Event handlers

        private void SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        #endregion

        #region Public methods

        public void Refresh()
        {
            GetLegends();
            Draw();
        }

        #endregion

        #region Private methods

        private void Draw(bool updateLegends = true)
        {
            cnvMain.Children.Clear();

            double verticalLineHorizontalMargin = 40;
            double horisontalLineVerticalMargin = 25;
            double legendsHorizontalMargin = 10;

            // Drawing Main lines
            var vLine = new Line
            {
                X1 = verticalLineHorizontalMargin,
                X2 = verticalLineHorizontalMargin,
                Y1 = 10,
                Y2 = cnvMain.ActualHeight - 10
            };

            var hLine = new Line
            {
                X1 = 10,
                X2 = cnvMain.ActualWidth - 10,
                Y1 = cnvMain.ActualHeight - horisontalLineVerticalMargin,
                Y2 = cnvMain.ActualHeight - horisontalLineVerticalMargin
            };
            
            cnvMain.Children.Add(vLine);
            cnvMain.Children.Add(hLine);

            //-------------------------------------------------------------------------------
            var tmpItems = new ArrayList();
            if (Items != null)
                foreach (object item in Items)
                    tmpItems.Add(item);

            if (tmpItems.Count == 0 ||
                String.IsNullOrEmpty(VerticalPropertyName) ||
                String.IsNullOrEmpty(HorizontalPropertyName) ||
                String.IsNullOrEmpty(LegendPropertyName))
                return;

            if (tmpItems[0].GetType().GetProperty(VerticalPropertyName) == null)
                throw new ArgumentException("VerticalPropertyName is not correct.");

            if (tmpItems[0].GetType().GetProperty(HorizontalPropertyName) == null)
                throw new ArgumentException("HorizontalPropertyName is not correct.");

            if (tmpItems[0].GetType().GetProperty(LegendPropertyName) == null)
                throw new ArgumentException("LegendPropertyName is not correct.");

            tmpItems.Sort(new ItemsComparer(HorizontalPropertyName));

            //-------------------------------------------------------------------------------

            var horizontalValues = new List<double>();
            double maxValue = 0;

            foreach (var item in tmpItems)
            {
                var verticalValue = item.GetType().GetProperty(VerticalPropertyName).GetValue(item, null);
                var horizontalValue = item.GetType().GetProperty(HorizontalPropertyName).GetValue(item, null);

                if (!horizontalValues.Exists(i => i == Convert.ToDouble(horizontalValue)))
                    horizontalValues.Add(Convert.ToDouble(horizontalValue));

                if (Convert.ToDouble(verticalValue) > maxValue)
                    maxValue = Convert.ToDouble(verticalValue);
            }

            if (cnvMain.ActualWidth == 0)
                return;

            //-------------------------------------------------------------------------------

            double drawingAreaWidth = (cnvMain.ActualWidth - verticalLineHorizontalMargin);
            double maxValueTopMargin = 10 + 20;

            var lMax = new Line
            {
                StrokeDashArray = new DoubleCollection() {2},
                X1 = verticalLineHorizontalMargin - 5,
                X2 = hLine.X2,
                Y1 = maxValueTopMargin,
                Y2 = maxValueTopMargin
            };
            cnvMain.Children.Add(lMax);

            var lAvg = new Line
            {
                StrokeDashArray = new DoubleCollection() {2},
                X1 = lMax.X1,
                X2 = lMax.X2,
                Y1 = (hLine.Y1 - lMax.Y1) / 2 + maxValueTopMargin,
                Y2 = (hLine.Y1 - lMax.Y1) / 2 + maxValueTopMargin
            };
            cnvMain.Children.Add(lAvg);

            var tbMax = new TextBlock {Text = maxValue.ToString()};

            var formattedMaxText = new FormattedText(tbMax.Text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tbMax.FontFamily, tbMax.FontStyle, tbMax.FontWeight, tbMax.FontStretch),
                    tbMax.FontSize,
                    Brushes.Black);

            Canvas.SetLeft(tbMax, verticalLineHorizontalMargin - formattedMaxText.Width - 10);
            Canvas.SetTop(tbMax, lMax.Y1 - formattedMaxText.Height / 2.0);
            cnvMain.Children.Add(tbMax);

            var tbAvg = new TextBlock {Text = (maxValue/2.0).ToString()};

            var formattedAvgText = new FormattedText(tbAvg.Text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tbAvg.FontFamily, tbAvg.FontStyle, tbAvg.FontWeight, tbAvg.FontStretch),
                    tbAvg.FontSize,
                    Brushes.Black);

            Canvas.SetLeft(tbAvg, verticalLineHorizontalMargin - formattedAvgText.Width - 10);
            Canvas.SetTop(tbAvg, lAvg.Y1 - formattedAvgText.Height / 2.0);
            cnvMain.Children.Add(tbAvg);

            int legendsCount = Legends.Count(f => f.IsVisible || !CanChangeLegendVisibility);
            double barsWidth = (drawingAreaWidth - (horizontalValues.Count * legendsHorizontalMargin)) / horizontalValues.Count / legendsCount - legendsHorizontalMargin / 2.0;
            
            if (Double.IsInfinity(barsWidth) || Double.IsNaN(barsWidth))
                barsWidth = 0;

            double HorItemWidth = Math.Ceiling((drawingAreaWidth - (horizontalValues.Count * legendsHorizontalMargin)) / horizontalValues.Count);

            for (int i = 0; i < horizontalValues.Count; i++)
            {
                Line l = new Line();
                l.X1 = (HorItemWidth * i) + verticalLineHorizontalMargin + ((legendsCount * barsWidth) / 2.0) + legendsHorizontalMargin;
                l.X2 = l.X1;
                l.Y1 = hLine.Y1;
                l.Y2 = l.Y1 + 5;
                cnvMain.Children.Add(l);

                var tb = new TextBlock {Text = horizontalValues[i].ToString()};

                var formattedText = new FormattedText(tb.Text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                    tb.FontSize,
                    Brushes.Black);

                Canvas.SetLeft(tb, l.X1 - (formattedText.Width / 2));
                Canvas.SetTop(tb, l.Y2 + 5);
                cnvMain.Children.Add(tb);
            }

            foreach (double horizontalIndex in horizontalValues)
            {
                var items = from object item in tmpItems
                            where Convert.ToDouble(item.GetType().GetProperty(HorizontalPropertyName).GetValue(item, null)) == horizontalIndex
                            orderby item.GetType().GetProperty(LegendPropertyName).GetValue(item, null)
                            select item;

                int legendValueIndex = 0;
                foreach (object item in items)
                {
                    var verticalValue = item.GetType().GetProperty(VerticalPropertyName).GetValue(item, null);
                    var horizontalValue = item.GetType().GetProperty(HorizontalPropertyName).GetValue(item, null);
                    var legendValue = item.GetType().GetProperty(LegendPropertyName).GetValue(item, null);

                    object currentLegend = null;
                    try
                    {
                        currentLegend = Legends.Where(i => i.LegendType.Equals(legendValue)).First();
                    }
                    catch
                    { }

                    if (currentLegend == null || (CanChangeLegendVisibility && !(currentLegend as Legend).IsVisible))
                        continue;

                    int horizontalValueIndex = horizontalValues.IndexOf(Convert.ToDouble(horizontalValue));

                    double barLeft = (HorItemWidth * horizontalValueIndex) + legendsHorizontalMargin +
                        verticalLineHorizontalMargin +
                        (legendValueIndex * barsWidth);

                    var b = new Border
                    {
                        Style = (Style) cnvMain.FindResource("BarStyle"),
                        Width = barsWidth,
                        Height = Convert.ToDouble(verticalValue)*(hLine.Y1 - lMax.Y1)/maxValue,
                        Background = (currentLegend as Legend).Brush
                    };

                    Canvas.SetLeft(b, barLeft);
                    Canvas.SetTop(b, hLine.Y1 - b.Height);
                    cnvMain.Children.Add(b);

                    var tbValue = new TextBlock();
                    Panel.SetZIndex(tbValue, 100);
                    tbValue.Text = verticalValue.ToString();

                    var binding = new Binding("ValueVisibility") {Source = this};
                    tbValue.SetBinding(TextBlock.VisibilityProperty, binding);

                    var formattedText = new FormattedText(tbValue.Text,
                        CultureInfo.CurrentUICulture,
                        FlowDirection.LeftToRight,
                        new Typeface(tbValue.FontFamily, tbValue.FontStyle, tbValue.FontWeight, tbValue.FontStretch),
                        tbValue.FontSize,
                        Brushes.Black);

                    Canvas.SetLeft(tbValue, barLeft + (((barLeft + barsWidth) - barLeft) / 2 - formattedText.Width / 2));
                    Canvas.SetTop(tbValue, hLine.Y1 - b.Height - formattedText.Height - 5);
                    cnvMain.Children.Add(tbValue);

                    legendValueIndex++;
                }
            }

            if (updateLegends)
                DrawLegends();
            else
                cnvMain.Children.Add(brdLegends);
        }

        private void DrawLegends()
        {
            FrameworkElement legendbrd = null;
            foreach (FrameworkElement elem in cnvMain.Children)
            {
                if (elem is Border && elem.Name == "brdLegends")
                {
                    legendbrd = elem;
                    break;
                }
            }

            if (legendbrd != null)
                cnvMain.Children.Remove(legendbrd);

            // Creating Legends List
            brdLegends = new Border();
            brdLegends.Name = "brdLegends";
            brdLegends.CornerRadius = new CornerRadius(5);
            brdLegends.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            Binding legVisibilityBinding = new Binding("LegendsVisibility");
            legVisibilityBinding.Source = this;
            brdLegends.SetBinding(Border.VisibilityProperty, legVisibilityBinding);

            brdLegends.BorderBrush = Brushes.DarkGray;
            brdLegends.BorderThickness = new Thickness(1);

            ListBox lstLegends = new ListBox();
            lstLegends.Margin = new Thickness(5);
            lstLegends.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            lstLegends.SelectionChanged += (slst, elst) =>
            {
                (slst as ListBox).UnselectAll();
            };
            lstLegends.BorderThickness = new Thickness(0);

            Binding legendsBinding = new Binding("Legends");
            legendsBinding.Source = this;
            lstLegends.SetBinding(ListBox.ItemsSourceProperty, legendsBinding);

            lstLegends.ItemTemplate = (DataTemplate)this.FindResource("LegendItemTemplate");

            brdLegends.Child = lstLegends;
            cnvMain.Children.Add(brdLegends);

            brdLegends.UpdateLayout();
            Canvas.SetLeft(brdLegends, cnvMain.ActualWidth - brdLegends.ActualWidth - 10);
            Canvas.SetTop(brdLegends, 10);
        }

        private void GetLegends()
        {
            if (Legends == null)
            {
                Legends = GenerateLegends();
                return;
            }

            if (Items == null)
                return;

            // add missed legends
            Random rand = new Random(DateTime.Now.Millisecond);
            foreach (var item in Items)
            {
                var legendValue = item.GetType().GetProperty(LegendPropertyName).GetValue(item, null);

                var leg = from Legend lc in Legends where lc.LegendType.Equals(legendValue) select lc;
                if (leg.Count() == 0)
                {
                    Legend legend = new Legend();
                    legend.LegendType = legendValue;
                    Color c = Color.FromRgb(Convert.ToByte(rand.Next(256)), Convert.ToByte(rand.Next(256)), Convert.ToByte(rand.Next(256)));
                    legend.Brush = new SolidColorBrush(c);
                    Legends.Add(legend);
                }
            }

            // update event handlers
            foreach (var legend in Legends)
            {
                legend.IsVisibleChanged += (s, e) => Draw(false);
            }
        }

        private ObservableCollection<Legend> GenerateLegends()
        {
            var legends = new ObservableCollection<Legend>();
            if (Items == null) return legends;

            var distinctLegendsCount = 0;

            foreach (var item in Items)
            {
                var legendValue = item.GetType().GetProperty(LegendPropertyName).GetValue(item, null);
                var legend = legends.FirstOrDefault(it => it.LegendType == legendValue);

                if (legend == null)
                {
                    legend = new Legend();
                    legend.LegendType = legendValue;
                    legend.IsVisibleChanged += (s, e) => Draw(false);

                    // update legend type count
                    distinctLegendsCount++;
                }
            }

            // generate brushes 
            var brushes = ColorGenerator.GenerateBrashes(distinctLegendsCount);

            // update brush for every legend
            for (int i = 0; i < distinctLegendsCount; i++)
            {
                legends[i].Brush = brushes[i];
            }

            return legends;
        }

        #endregion

        #region Comparer for sorting items

        class ItemsComparer : IComparer
        {
            #region Private fields

            private string horizontalPropertyName = String.Empty;

            #endregion 

            #region Constructors

            public ItemsComparer(string horizontalPropertyName)
                : base()
            {
                this.horizontalPropertyName = horizontalPropertyName;
            }

            #endregion

            #region IComparer implementation

            public int Compare(object x, object y)
            {
                if (String.IsNullOrEmpty(horizontalPropertyName))
                    throw new ArgumentException();

                var xHorValue = Convert.ToDouble(x.GetType().GetProperty(horizontalPropertyName).GetValue(x, null));
                var yHorValue = Convert.ToDouble(y.GetType().GetProperty(horizontalPropertyName).GetValue(y, null));

                return xHorValue.CompareTo(yHorValue);
            }

            #endregion 
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion 
    }

    [System.ComponentModel.DefaultEvent("IsVisibleChanged")]
    [System.ComponentModel.DefaultProperty("LegendType")]
    public class Legend : DependencyObject, INotifyPropertyChanged
    {
        #region Private fields

        private object _legend;
        private string _displayName;
        private Brush _brush;
        private bool _isVisible;

        #endregion

        #region Initialization

        public Legend()
        {
            _displayName = String.Empty;
            _isVisible = true;
        }

        #endregion

        #region Properties

        public object LegendType
        {
            get { return _legend; }
            set
            {
                _legend = value;
                NotifyPropertyChanged("Legend");
            }
        }

        public string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName))
                    if (LegendType == null)
                        return String.Empty;
                    else
                        return LegendType.ToString();
                else
                    return _displayName;
            }
            set
            {
                _displayName = value;
                NotifyPropertyChanged("DisplayName");
            }
        }

        public Brush Brush
        {
            get { return _brush; }
            set
            {
                _brush = value;
                NotifyPropertyChanged("Brush");
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    NotifyPropertyChanged("IsVisible");
                    if (IsVisibleChanged != null)
                        IsVisibleChanged(this, new RoutedEventArgs());
                }
            }
        }

        #endregion 

        #region Events

        public event RoutedEventHandler IsVisibleChanged;

        #endregion 

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion 
    }
}
