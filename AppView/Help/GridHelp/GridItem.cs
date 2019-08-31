using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppView.Help
{
    public class GridItem
    {
        public GridItem(UserControl control)
        {
            Control = control;

            // set defaults
            GridColumnSpan = 1;
            GridRowSpan = 1;
        }

        public GridItem(UserControl control, int gridColumn, int gridRow)
            : this(control)
        {
            GridColumn = gridColumn;
            GridRow = gridRow;
        }

        public GridItem(UserControl control, int gridColumn, int gridRow, int gridColumnSpan,
            int gridRowSpan)
            : this(control, gridColumn, gridRow)
        {
            GridColumnSpan = gridColumnSpan;
            GridRowSpan = gridRowSpan;
        }

        public UserControl Control { get; private set; }
        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public int GridColumnSpan { get; private set; }
        public int GridRowSpan { get; private set; }
    }

}
