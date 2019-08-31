using System;
using System.Collections.Generic;
using System.Globalization;
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
using AppCommon.Commands;
using AppUtils.Settings;
using AppView.Controls.Interfaces;
using AppView.Help;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for PageReadOnlyDataGrid.xaml
    /// </summary>
    public partial class PageReadOnlyDataGrid : UserControl, IPage
    {
        #region Initialization

        public PageReadOnlyDataGrid(Action closeControlAction = null, string header = null)
        {
            InitializeComponent();
            this.Header = header;
            pageSpecificHeader.Content = header;
            InitializeCommands(closeControlAction);
        }

        public void InitializeCommands(Action closeControlAction)
        {
            OkCommand = new Command(() =>
            {
                if (closeControlAction != null)
                    closeControlAction();
            }, () => true);

            this.DataContext = this;
        }

        public void InitializeDataGrid<T>()
            where T : class
        {
            InitializeDataGrid(TypeProcessor.GenerateColumnsInfo<T>());
        }

        public void InitializeDataGrid(IEnumerable<SimpleBindPropertyInfo> columnsInfo)
        {
            InitializeDataGridColumns(columnsInfo);
        }

        private void InitializeDataGridColumns(IEnumerable<SimpleBindPropertyInfo> columnsInfo)
        {
            foreach (var columnInfo in columnsInfo)
            {
                DataGridBoundColumn dataGridColumn = null;

                // create new column
                if (columnInfo.DataType == typeof(bool))
                {
                    dataGridColumn = new DataGridCheckBoxColumn()
                    {
                        Header = columnInfo.Header,
                        Binding = new Binding(columnInfo.Name)
                    };
                }
                else
                {
                    dataGridColumn = new DataGridTextColumn()
                    {
                        Header = columnInfo.Header,
                        MinWidth = 100,
                        MaxWidth = 400,
                        Binding = new Binding(columnInfo.Name)
                        {
                            StringFormat = columnInfo.DataType == typeof(decimal)
                                ? "##0.00"
                                : columnInfo.Format,
                            ConverterCulture = AppSettings.DecimalSeparator == "."
                                ? CultureInfo.CreateSpecificCulture("en-US")
                                : CultureInfo.CreateSpecificCulture("de-DE")
                        }
                    };
                }
                
                // add column to data grid
                dataGrid.Columns.Add(dataGridColumn);
            }

            BindingOperations.SetBinding(dataGrid, MaxWidthProperty, new Binding("ActualWidth")
            {
                Source = this,
                NotifyOnSourceUpdated = true,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                NotifyOnTargetUpdated = true
            });
        }

        public void SetDataSource<T>(IEnumerable<T> elements)
            where T : class
        {
            dataGrid.ItemsSource = elements;
        }

        #endregion

        #region Public properties
        
        public DataGrid DataGrid { get { return dataGrid; } }

        public ICommand OkCommand { get; private set; }

        #endregion

        #region IPage implementation

        public string Header { get; private set; }
        public string SpecificHeader 
        { 
            set 
            { 
                pageSpecificHeader.Content = value;

                if (string.IsNullOrEmpty(value)) pageSpecificHeader.Visibility = Visibility.Collapsed;
                else pageSpecificHeader.Visibility = Visibility.Visible;
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
