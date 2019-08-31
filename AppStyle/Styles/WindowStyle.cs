using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace AppStyle.Styles
{
    public partial class WindowStyle
    {
        private void HideWindow(object sender, EventArgs e)
        {
            Window window = ((FrameworkElement)sender).TemplatedParent as Window;
            if (window != null) window.WindowState = WindowState.Minimized;
        }

        private void ChangeWindowState(object sender, EventArgs e)
        {
            Window window = ((FrameworkElement)sender).TemplatedParent as Window;
            if (window == null) return;

            switch (window.WindowState)
            {
                case WindowState.Normal:
                    window.WindowState = WindowState.Maximized;
                    break;

                case WindowState.Maximized:
                    window.WindowState = WindowState.Normal;
                    break;
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            Window window = ((FrameworkElement)sender).TemplatedParent as Window;
            if (window != null) window.Close();
        }

        private void TopBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Window window = ((FrameworkElement)sender).TemplatedParent as Window;
                if (window != null) window.DragMove();
            }
        }

        private void backgroundBorder_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            Panel.SetZIndex(border, int.MaxValue);
        }
    }
}
