using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppView.Controls;
using AppView.Controls.Interfaces;

namespace AppView.View
{
    public class PageNavigationView : IView
    {
        #region Initialization

        public PageNavigationView()
        {
            PageNavigator = new PageNavigatorScreen();
        }

        public PageNavigationView(IEnumerable<IPage> pages, string header = null)
        {
            PageNavigator = new PageNavigatorScreen(pages, header);
            Pages = pages.ToArray();
        }

        public PageNavigationView(params IPage[] pages)
        {
            PageNavigator = new PageNavigatorScreen(pages);
            Pages = pages;
        }

        public PageNavigationView(string header, params IPage[] pages)
        {
            PageNavigator = new PageNavigatorScreen(pages, header);
            Pages = pages;
        }

        #endregion

        #region Public properties

        public PageNavigatorScreen PageNavigator { get; private set; }

        public IPage[] Pages { get; private set; }

        #endregion

        #region IView implementation

        public string Header { get { return PageNavigator.Header; } }

        public System.Windows.Controls.UserControl ControlView
        {
            get { return PageNavigator.Control; }
        }

        #endregion
    }
}
