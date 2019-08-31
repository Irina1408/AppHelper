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
    /// Interaction logic for TextBoxButton.xaml
    /// </summary>
    public partial class TextBoxButton : UserControl, INotifyPropertyChanged
    {
        #region Initialization

        public TextBoxButton()
        {
            InitializeComponent();

            textBox.IsReadOnly = true;
        }

        #endregion

        #region Text Property

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set
            {
                this.SetValue(TextProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(TextBoxButton),
            new FrameworkPropertyMetadata(string.Empty, TextChangedCallback));

        private static void TextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TextBoxButton).TextBox.Text = (string)e.NewValue;
        }

        #endregion

        #region Public properties

        public RoutedEventHandler ButtonOnClick { get; set; }
        public TextBox TextBox { get { return textBox; } }
        public Button Button { get { return btn; } }

        #endregion

        #region Event handlers

        private void Btn_OnClick(object sender, RoutedEventArgs e)
        {
            if (ButtonOnClick != null)
                ButtonOnClick(sender, e);
        }

        private void TextBoxButton_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (e.WidthChanged)
            //{
            //    textBox.Width = e.NewSize.Width - 30;
            //}
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
