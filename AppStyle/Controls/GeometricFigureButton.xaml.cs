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

namespace AppStyle.Controls
{
    /// <summary>
    /// Interaction logic for GeometricFigureButton.xaml
    /// </summary>
    public partial class GeometricFigureButton : UserControl, INotifyPropertyChanged
    {
        #region Private fields

        private PathGeometry geometricFigure;
        private string label;
        private ICommand command;

        #endregion

        #region Initialization

        public GeometricFigureButton()
        {
            InitializeComponent();
        }

        public GeometricFigureButton(PathGeometry geometricFigure, string label)
            :this()
        {
            this.GeometricFigure = geometricFigure;
            this.Label = label;

            this.DataContext = this;
        }

        public GeometricFigureButton(PathGeometry geometricFigure, string label, ICommand command)
            : this(geometricFigure, label)
        {
            this.Command = command;
            this.DataContext = this;
        }

        #endregion

        #region Public properties

        public PathGeometry GeometricFigure
        {
            get { return geometricFigure; }
            set
            {
                geometricFigure = value;
                NotifyPropertyChanged();
            }
        }

        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand Command
        {
            get { return command; }
            set
            {
                command = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
