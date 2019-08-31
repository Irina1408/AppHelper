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
using AppView.Controls.Interfaces;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for PageNavigator.xaml
    /// </summary>
    public partial class PageNavigatorScreen : UserControl, IPage
    {
        #region private fields

        private Page currentPage;

        #endregion

        #region Initialization

        public PageNavigatorScreen()
        {
            InitializeComponent();
        }

        public PageNavigatorScreen(string header, params IPage[] pages)
            : this()
        {
            InitializePagesControl(pages);
            Header = header;
        }

        public PageNavigatorScreen(IEnumerable<IPage> pages, string header = null)
            :this()
        {
            InitializePagesControl(pages);
            Header = header;
        }

        private void InitializePagesControl(IEnumerable<IPage> pages)
        {
            foreach (var page in pages)
            {
                var button = new Button()
                {
                    Margin = new Thickness(0),
                    BorderThickness = new Thickness(0, 0, 1, 1),
                    FontSize = 20,
                    Background = Brushes.Transparent,
                    Height = 50,
                    Width = 120,
                    Content = page.Header
                };

                pageButtonsPanel.Children.Add(button);

                var p = new Page()
                {
                    Content = page.Control
                };

                button.Click += (sender, args) =>
                {
                    if (currentPage == null || currentPage != p)
                    {
                        pageNavigationFrame.NavigationService.Navigate(p);
                        currentPage = p;

                        foreach (var btn in pageButtonsPanel.Children.OfType<Button>())
                        {
                            btn.FontStyle = FontStyles.Normal;
                            btn.Background = Brushes.Transparent;
                        }

                        button.FontStyle = FontStyles.Italic;
                        button.Background = new SolidColorBrush(Colors.WhiteSmoke) { Opacity = 0.2 };
                    }
                };
            }

            // navigate to first page
            var firstPage = pages.FirstOrDefault();
            if(firstPage != null)
                pageNavigationFrame.NavigationService.Navigate(firstPage);
        }

        #endregion

        #region Public properties

        public Dock ButtonsDock 
        { 
            get { return DockPanel.GetDock(pageButtonsPanel); }
            set 
            { 
                DockPanel.SetDock(pageButtonsPanel, value);
                if(value == Dock.Top || value == Dock.Bottom)
                {
                    pageButtonsPanel.Orientation = Orientation.Horizontal;
                }
                else
                {
                    pageButtonsPanel.Orientation = Orientation.Vertical;
                }
            }
        }

        #endregion

        #region IPage implementation

        public string Header { get; private set; }
        public string SpecificHeader { set { pageSpecificHeader.Content = string.IsNullOrEmpty(value) ? Header : value; } }
        public UserControl Control { get { return this; } }
        public EventHandler OnRefreshPage { get; set; }
        public void Refresh()
        {
            if (OnRefreshPage != null)
                OnRefreshPage(this, EventArgs.Empty);
        }

        #endregion
    }
}
