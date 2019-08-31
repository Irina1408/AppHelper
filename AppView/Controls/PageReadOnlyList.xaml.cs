using System;
using System.Collections;
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
    /// Interaction logic for PageReadOnlyListControl.xaml
    /// </summary>
    public partial class PageReadOnlyList : UserControl, IPage
    {
        #region Initialization

        public PageReadOnlyList()
        {
            InitializeComponent();
        }

        public PageReadOnlyList(IEnumerable items, string header = null)
            : this()
        {
            pageSpecificHeader.Content = header;
            Header = header;

            foreach (var item in items)
            {
                this.items.Items.Add("● " + item);
            }
        }

        #endregion

        #region Public properties


        #endregion

        #region IPage implementation

        public string Header { get; private set; }
        public string SpecificHeader { set { pageSpecificHeader.Content = value; } }
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
