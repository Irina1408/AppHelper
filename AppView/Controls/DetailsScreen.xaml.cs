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
using AppCommon.Commands;
using AppStyle.Controls;
using AppView.Help;

namespace AppView.Controls
{
    /// <summary>
    /// Interaction logic for DetailsScreen.xaml
    /// </summary>
    public partial class DetailsScreen : UserControl
    {
        #region Initialization

        public DetailsScreen(string header = null)
        {
            InitializeComponent();
            this.header.Content = header;
        }

        public DetailsScreen(CommandContainer commandContainer, string header = null)
            : this(header)
        {
            this.DataContext = commandContainer;
        }

        public virtual void InitEntity<T>(T entity)
        {
            var detailsInfo = TypeProcessor.GenerateDetailsInfo<T>();
            InitEntity(entity, detailsInfo);
        }

        public virtual void InitEntity<T>(T entity, IEnumerable<BindPropertyInfo> detailsInfo)
        {
            // fill properties
            GridFiller.FillGrid(gridDetails, detailsInfo);

            // init data context
            gridDetails.DataContext = entity;
        }

        #endregion
    }
}
