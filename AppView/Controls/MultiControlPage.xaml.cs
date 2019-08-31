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
using AppView.Help;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class MultiControlPage : UserControl, IPage
    {
        #region Initialization

        public MultiControlPage()
        {
            InitializeComponent();
        }

        public MultiControlPage(string header)
            : this()
        {
            Header = header;
        }

        public MultiControlPage(string header, params UserControl[] userControls)
            : this(header, null, userControls)
        { }

        public MultiControlPage(string header, GridRowColSettings settings, params UserControl[] userControls)
            : this(header)
        {
            int maxColumnCount = 3;
            int currRow = 0;
            int currCol = 0;

            var pageItems = new GridItem[userControls.Length];

            for (int i = 0; i < userControls.Length; i++)
            {
                pageItems[i] = new GridItem(userControls[i], currCol, currRow);

                // update column and row
                if (currCol >= maxColumnCount - 1)
                {
                    currRow += 1;
                    currCol = 0;
                }
                else
                {
                    currCol += 1;
                }
            }

            // initialize page items
            InitializePage(settings, pageItems);
        }

        public MultiControlPage(string header, params GridItem[] pageItems)
            : this(header, null, pageItems)
        { }

        public MultiControlPage(string header, GridRowColSettings settings, params GridItem[] pageItems)
            : this(header)
        {
            InitializePage(settings, pageItems);
        }

        private void InitializePage(GridRowColSettings settings, GridItem[] pageItems)
        {
            // create grid rows
            int rowCount = pageItems.Max(item => item.GridRow);
            for (int i = 0; i <= rowCount; i++)
            {
                var rowDefinition = new RowDefinition();

                // set settings for row definition
                if (settings != null)
                    rowDefinition.Height = settings.GetRowHeight(i);

                elemsGrid.RowDefinitions.Add(rowDefinition);
            }

            // create grid columns
            int columnCount = pageItems.Max(item => item.GridColumn);
            for (int i = 0; i <= columnCount; i++)
            {
                var columnDefinition = new ColumnDefinition();

                // set settings for row definition
                if (settings != null)
                    columnDefinition.Width = settings.GetColumnWidth(i);
                else if (i != 0) 
                    columnDefinition.Width = GridLength.Auto;

                elemsGrid.ColumnDefinitions.Add(columnDefinition);
            }

            foreach (var pageItem in pageItems)
            {
                // set grid settings
                Grid.SetColumn(pageItem.Control, pageItem.GridColumn);
                Grid.SetRow(pageItem.Control, pageItem.GridRow);
                Grid.SetColumnSpan(pageItem.Control, pageItem.GridColumnSpan);
                Grid.SetRowSpan(pageItem.Control, pageItem.GridRowSpan);

                // add control to grid
                elemsGrid.Children.Add(pageItem.Control);
            }
        }

        #endregion

        #region IPage implementation

        public string SpecificHeader
        {
            set { }
        }

        public string Header { get; private set; }

        public UserControl Control
        {
            get { return this; }
        }

        public EventHandler OnRefreshPage { get; set; }

        public void Refresh()
        {
            if (OnRefreshPage != null)
                OnRefreshPage(this, EventArgs.Empty);
        }

        #endregion
    }
}
