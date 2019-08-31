using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppView.View
{
    public class DefaultView : IView
    {
        #region Initialization

        public DefaultView()
        { }

        public DefaultView(UserControl userControl, string header = null)
        {
            ControlView = userControl;
            Header = header;
        }

        #endregion

        #region IView implementation

        public string Header { get; private set; }

        public System.Windows.Controls.UserControl ControlView { get; private set; }

        #endregion
    }
}
