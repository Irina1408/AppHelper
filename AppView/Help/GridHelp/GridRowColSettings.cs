using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppView.Help
{
    public class GridRowColSettings
    {
        #region Private fields

        private Dictionary<int, GridLength> rowSettings;
        private Dictionary<int, GridLength> columnSettings;

        #endregion

        #region Initialization

        public GridRowColSettings()
        {
            rowSettings = new Dictionary<int, GridLength>();
            columnSettings = new Dictionary<int, GridLength>();
        }

        public GridRowColSettings(IEnumerable<KeyValuePair<int, GridLength>> rowHeights,
            IEnumerable<KeyValuePair<int, GridLength>> columnWidths)
            : this()
        {
            if (rowHeights != null)
            {
                foreach (var rowHeight in rowHeights)
                {
                    SetRowHeight(rowHeight.Key, rowHeight.Value);
                }
            }

            if (columnWidths != null)
            {
                foreach (var columnWidth in columnWidths)
                {
                    SetColumnWidth(columnWidth.Key, columnWidth.Value);
                }
            }
        }

        #endregion

        #region Public properties

        public void SetRowHeight(int row, GridLength gridLength)
        {
            if (!rowSettings.ContainsKey(row))
                rowSettings.Add(row, gridLength);
            else
                rowSettings[row] = gridLength;
        }

        public GridLength GetRowHeight(int row)
        {
            if (rowSettings.ContainsKey(row))
                return rowSettings[row];

            return new GridLength(1, GridUnitType.Star);
        }

        public void SetColumnWidth(int column, GridLength gridLength)
        {
            if (!columnSettings.ContainsKey(column))
                columnSettings.Add(column, gridLength);
            else
                columnSettings[column] = gridLength;
        }

        public GridLength GetColumnWidth(int column)
        {
            if (columnSettings.ContainsKey(column))
                return columnSettings[column];

            return new GridLength(1, GridUnitType.Star);
        }

        #endregion
    }
}
