using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace AppStyle.Controls
{
    public class AppWindow: Window
    {
        #region Initialization

        public AppWindow()
            :base()
        {
            Loaded += Window_OnLoaded;
        }

        #endregion

        #region Bottom bar text

        public String BottomBarText
        {
            get { return (String)this.GetValue(BottomBarTextProperty); }
            set { this.SetValue(BottomBarTextProperty, value); }
        }

        public static readonly DependencyProperty BottomBarTextProperty = DependencyProperty.Register(
            "BottomBarText", typeof(String), typeof(AppWindow),
            new PropertyMetadata(null));

        #endregion

        #region Bottom bar font size

        public double BottomBarFontSize
        {
            get { return (double)this.GetValue(BottomBarFontSizeProperty); }
            set { this.SetValue(BottomBarFontSizeProperty, value); }
        }

        public static readonly DependencyProperty BottomBarFontSizeProperty = DependencyProperty.Register(
            "BottomBarFontSize", typeof(double), typeof(AppWindow),
            new PropertyMetadata(12.0));

        #endregion

        #region Visibility title bar buttons

        public Visibility TitleBarButtonsVisibility
        {
            get { return (Visibility)this.GetValue(TitleBarButtonsVisibilityProperty); }
            set { this.SetValue(TitleBarButtonsVisibilityProperty, value); }
        }

        public static readonly DependencyProperty TitleBarButtonsVisibilityProperty = DependencyProperty.Register(
            "TitleBarButtonsVisibility", typeof(Visibility), typeof(AppWindow),
            new PropertyMetadata(System.Windows.Visibility.Visible));

        #endregion

        #region Control handlers

        private void Window_OnLoaded(object sender, EventArgs e)
        {
            //set max screen size
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
        }
        
        #endregion
    }
}
