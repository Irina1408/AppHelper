using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppView.Controls;
using AppView.Controls.Interfaces;

namespace AppView.View
{
    public class TabMenuView: IView
    {
        #region Initialization

        public TabMenuView()
        {
            TabMenu = new TabMenuScreen();
        }

        public TabMenuView(IEnumerable<IPage> pages, string header = null)
        {
            TabMenu = new TabMenuScreen(pages, header);
        }

        public TabMenuView(params IPage[] pages)
        {
            TabMenu = new TabMenuScreen(pages);
        }

        public TabMenuView(string header, params IPage[] pages)
        {
            TabMenu = new TabMenuScreen(pages, header);
        }

        #endregion

        #region Public properties

        public ITabMenu TabMenu { get; private set; }

        #endregion

        #region IView implementation

        public string Header { get { return TabMenu.Header; } }

        public System.Windows.Controls.UserControl ControlView
        {
            get { return TabMenu.Control; }
        }

        #endregion
    }
}
