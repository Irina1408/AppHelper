using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppStyle.Controls
{
    public class AppImage : Control, INotifyPropertyChanged
    {
        #region public ImageSource ImageSource { get; set; } // Dependency Property

        public ImageSource ImageSource
        {
            get { return (ImageSource)this.GetValue(SourceProperty); }
            set 
            { 
                this.SetValue(SourceProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(ImageSource), typeof(AppImage),
            new PropertyMetadata(null));

        #endregion

        #region public Stretch Stretch { get; set; } // Dependency Property

        public Stretch Stretch
        {
            get { return (Stretch)this.GetValue(StretchProperty); }
            set
            {
                this.SetValue(StretchProperty, value);
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
            "Stretch", typeof(Stretch), typeof(AppImage),
            new PropertyMetadata(Stretch.Fill));

        #endregion

        #region public Thickness BorderThickness { get; set; } // Dependency Property

        public new Thickness BorderThickness
        {
            get { return (Thickness)this.GetValue(BorderThicknessProperty); }
            set
            {
                this.SetValue(BorderThicknessProperty, value);
                OnPropertyChanged();
            }
        }

        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(
            "BorderThickness", typeof(Thickness), typeof(AppImage),
            new PropertyMetadata(new Thickness(0), BorderThicknessChanged));

        private static void BorderThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldThickness = new Thickness(0);
            var newThickness = new Thickness(0);

            if (e.OldValue != null)
                oldThickness = (Thickness)e.OldValue;
            if (e.NewValue != null)
                newThickness = (Thickness)e.NewValue;

            var appImage = d as AppImage;
            if (appImage == null) return;

            // take await old thickness value
            appImage.Height -= oldThickness.Top + oldThickness.Bottom;
            appImage.Width -= oldThickness.Left + oldThickness.Right;
            // add new thickness value
            appImage.Height += newThickness.Top + newThickness.Bottom;
            appImage.Width += newThickness.Left + newThickness.Right;
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
