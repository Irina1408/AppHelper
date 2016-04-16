﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AppPieChart.PieChart.ColorUtils;
using AppPieChart.Shapes;
using AppPieChart.Utils;

namespace AppPieChart.PieChart
{
    /// <summary>
    /// Interaction logic for PiePlotter.xaml
    /// </summary>
    public partial class PiePlotter : UserControl
    {
        #region Private fields

        private List<PiePiece> piePieces = new List<PiePiece>();

        #endregion

        #region Initialization

        public PiePlotter()
        {
            // register any dependency property change handlers
            DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromProperty(AppPieChart.PlottedPropertyProperty, typeof(PiePlotter));
            dpd.AddValueChanged(this, PlottedPropertyChanged);

            InitializeComponent();

            DependencyPropertyDescriptor dpdp = DependencyPropertyDescriptor.FromProperty(AppPieChart.ActualHeightProperty, typeof(PiePlotter));
            dpdp.AddValueChanged(this, PlottedPropertyChanged);

            DependencyPropertyDescriptor dpdp1 = DependencyPropertyDescriptor.FromProperty(AppPieChart.ActualWidthProperty, typeof(PiePlotter));
            dpdp1.AddValueChanged(this, PlottedPropertyChanged);

            this.DataContextChanged += DataContextChangedHandler;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The property of the bound object that will be plotted
        /// </summary>
        public String PlottedProperty
        {
            get { return AppPieChart.GetPlottedProperty(this); }
            set { AppPieChart.SetPlottedProperty(this, value); }
        }

        /// <summary>
        /// Caption of the property of the bound object that will be plotted
        /// </summary>
        public String PlottedCaptionProperty
        {
            get { return AppPieChart.GetPlottedCaptionProperty(this); }
            set { AppPieChart.SetPlottedCaptionProperty(this, value); }
        }

        /// <summary>
        /// A class which selects a color based on the item being rendered.
        /// </summary>
        public IColorSelector ColorSelector
        {
            get { return AppPieChart.GetColorSelector(this); }
            set { AppPieChart.SetColorSelector(this, value); }
        }

        #endregion

        #region HoleSize property

        /// <summary>
        /// The size of the hole in the centre of circle (as a percentage)
        /// </summary>
        public double HoleSize
        {
            get { return (double)GetValue(HoleSizeProperty); }
            set
            {
                SetValue(HoleSizeProperty, value);
                ConstructPiePieces();
            }
        }

        public static readonly DependencyProperty HoleSizeProperty =
                       DependencyProperty.Register("HoleSize", typeof(double), typeof(PiePlotter), new UIPropertyMetadata(0.0));
        
        #endregion

        #region property change handlers

        /// <summary>
        /// Handle changes in the datacontext. When a change occurs handlers are registered for events which
        /// occur when the collection changes or any items within teh collection change.
        /// </summary>
        void DataContextChangedHandler(object sender, DependencyPropertyChangedEventArgs e)
        {
            // handle the events that occur when the bound collection changes
            if (this.DataContext is INotifyCollectionChanged)
            {
                INotifyCollectionChanged observable = (INotifyCollectionChanged)this.DataContext;
                observable.CollectionChanged += new NotifyCollectionChangedEventHandler(BoundCollectionChanged);
            }

            // handle the selection change events
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (collectionView != null)
            {
                collectionView.CurrentChanged += CollectionViewCurrentChanged;
                collectionView.CurrentChanging += CollectionViewCurrentChanging;
            }
            else
            {
                CleanupPie();
                return;
            }
            
            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        /// Handles changes to the PlottedProperty property.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlottedPropertyChanged(object sender, EventArgs e)
        {
            ConstructPiePieces();
        }

        #endregion

        #region event handlers

        /// <summary>
        /// Handles the MouseUp event from the individual Pie Pieces
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PiePieceMouseUp(object sender, MouseButtonEventArgs e)
        {
            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (collectionView == null)
                return;

            PiePiece piece = sender as PiePiece;
            if (piece == null)
                return;

            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            collectionView.MoveCurrentToPosition(index);
        }

        /// <summary>
        /// Handles the event which occurs when the selected item is about to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionViewCurrentChanging(object sender, CurrentChangingEventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;

            if (collectionView != null && collectionView.CurrentPosition >= 0 && collectionView.CurrentPosition <= piePieces.Count)
            {
                PiePiece piece = piePieces[collectionView.CurrentPosition];

                DoubleAnimation a = new DoubleAnimation();
                a.To = 0;
                a.Duration = new Duration(TimeSpan.FromMilliseconds(50));

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }
        }

        /// <summary>
        /// Handles the event which occurs when the selected item has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CollectionViewCurrentChanged(object sender, EventArgs e)
        {
            CollectionView collectionView = (CollectionView)sender;

            if (collectionView != null && collectionView.CurrentPosition >= 0 && collectionView.CurrentPosition <= piePieces.Count)
            {
                PiePiece piece = piePieces[collectionView.CurrentPosition];

                DoubleAnimation a = new DoubleAnimation();
                a.To = 10;
                a.Duration = new Duration(TimeSpan.FromMilliseconds(200));

                piece.BeginAnimation(PiePiece.PushOutProperty, a);
            }


        }

        /// <summary>
        /// Handles events which are raised when the bound collection changes (i.e. items added/removed)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoundCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ConstructPiePieces();
            ObserveBoundCollectionChanges();
        }

        /// <summary>
        /// Iterates over the items inthe bound collection, adding handlers for PropertyChanged events
        /// </summary>
        private void ObserveBoundCollectionChanges()
        {
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);

            foreach (object item in myCollectionView)
            {
                if (item is INotifyPropertyChanged)
                {
                    INotifyPropertyChanged observable = (INotifyPropertyChanged)item;
                    observable.PropertyChanged += new PropertyChangedEventHandler(ItemPropertyChanged);
                }
            }
        }


        /// <summary>
        /// Handles events which occur when the properties of bound items change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the property which this pie chart represents has changed, re-construct the pie
            if (e.PropertyName.Equals(PlottedProperty))
            {
                ConstructPiePieces();
            }
        }

        #endregion

        #region Private methods

        private double GetPlottedPropertyValue(object item)
        {
            PropertyDescriptorCollection filterPropDesc = TypeDescriptor.GetProperties(item);
            object itemValue = filterPropDesc[PlottedProperty].GetValue(item);

            //TODO possibel type conversion?

            return double.Parse(itemValue.ToString());
        }

        private string GetPlottedCaptionPropertyValue(object item)
        {
            PropertyDescriptorCollection filterPropDesc = TypeDescriptor.GetProperties(item);
            object itemValue = filterPropDesc[PlottedCaptionProperty].GetValue(item);

            //TODO possibel type conversion?

            return (string)itemValue;
        }

        /// <summary>
        /// Constructs pie pieces and adds them to the visual tree for this control's canvas
        /// </summary>
        private void ConstructPiePieces()
        {
            CollectionView myCollectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (myCollectionView == null)
                return;

            double halfWidth = ((this.ActualHeight < this.ActualWidth ? this.ActualHeight : this.ActualWidth) 
                - this.Margin.Top - this.Margin.Bottom) / 2;
            double innerRadius = halfWidth * HoleSize;

            // compute the total for the property which is being plotted
            double total = myCollectionView.Cast<object>().Sum(item => GetPlottedPropertyValue(item));

            CleanupPie();

            // add the pie pieces
            double accumulativeAngle = 0;
            foreach (Object item in myCollectionView)
            {
                bool selectedItem = item == myCollectionView.CurrentItem;

                double wedgeAngle = GetPlottedPropertyValue(item) * 360 / total;

                PiePiece piece = new PiePiece()
                {
                    Radius = halfWidth,
                    InnerRadius = innerRadius,
                    CenterX = halfWidth,
                    CenterY = halfWidth,
                    PushOut = (selectedItem ? 10.0 : 0),
                    WedgeAngle = wedgeAngle,
                    PieceValue = GetPlottedPropertyValue(item),
                    RotationAngle = accumulativeAngle,
                    Fill = ColorSelector != null ? ColorSelector.SelectBrush(item, myCollectionView.IndexOf(item)) : Brushes.Black,
                    PieceCaption = GetPlottedCaptionPropertyValue(item),
                    // record the index of the item which this pie slice represents
                    Tag = myCollectionView.IndexOf(item),
                    ToolTip = new ToolTip()
                };

                piece.ToolTipOpening += PiePieceToolTipOpening;
                piece.MouseUp += PiePieceMouseUp;

                piePieces.Add(piece);
                canvas.Children.Insert(0, piece);

                accumulativeAngle += wedgeAngle;
            }
        }

        private void CleanupPie()
        {
            canvas.Children.Clear();
            piePieces.Clear();
        }

        /// <summary>
        /// Handles the event which occurs just before a pie piece tooltip opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PiePieceToolTipOpening(object sender, ToolTipEventArgs e)
        {
            PiePiece piece = (PiePiece)sender;

            CollectionView collectionView = (CollectionView)CollectionViewSource.GetDefaultView(this.DataContext);
            if (collectionView == null)
                return;

            // select the item which this pie piece represents
            int index = (int)piece.Tag;
            if (piece.ToolTip != null)
            {
                ToolTip tip = (ToolTip)piece.ToolTip;
                tip.DataContext = collectionView.GetItemAt(index);
            }
        }

        #endregion
    }
}
