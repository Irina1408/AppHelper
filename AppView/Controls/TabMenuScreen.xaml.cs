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
    /// Interaction logic for TabMenuScreen.xaml
    /// </summary>
    public partial class TabMenuScreen : UserControl, ITabMenu
    {
        #region Private fields

        private string header;

        #endregion

        #region Initialization

        public TabMenuScreen()
        {
            InitializeComponent();
        }

        public TabMenuScreen(string header, params IPage[] pages)
            : this()
        {
            InitializeTabControl(pages);
            this.header = header;
        }

        public TabMenuScreen(IEnumerable<IPage> pages, string header = null)
            :this()
        {
            InitializeTabControl(pages);
            this.header = header;
        }

        private void InitializeTabControl(IEnumerable<IPage> pages)
        {
            foreach (var page in pages)
            {
                // create new tab item
                var tabItem = new TabItem()
                {
                    Header = page.Header,
                    Content = page.Control
                };

                // add tab item to tab control
                tabControl.Items.Add(tabItem);
                tabControl.DataContext = tabItem;
            }
        }

        #endregion

        #region ITabMenu implementation

        public TabControl TabControl { get { return tabControl; } }

        public string Header { get { return header; } }

        public string SpecificHeader
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                    pageSpecificHeader.Visibility = Visibility.Collapsed;
                else
                {
                    pageSpecificHeader.Content = value;
                    pageSpecificHeader.Visibility = Visibility.Visible;
                }
            }
        }

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
