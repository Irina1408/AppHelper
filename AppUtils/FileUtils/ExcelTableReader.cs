using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace AppUtils.FileUtils
{
    public class ExcelTableReader :IDisposable
    {
        #region Private fields

        private Application excel;
        private Workbook workbook;
        private int currentRow;
        private List<string> columnNames;
        private ExcelRowValues currentRowValues;
        private Worksheet worksheet;

        #endregion

        #region Initialization

        public ExcelTableReader(string filePath, string sheetName, bool startFromSecondRow = false)
        {
            // init workbook
            excel = new Application();
            workbook = excel.Workbooks.Open(filePath);
            worksheet = workbook.Sheets[sheetName];
            currentRow = startFromSecondRow ? 2 : 1;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// First line of the excel
        /// </summary>
        public List<string> ColumnNames
        {
            get
            {
                if (columnNames == null || columnNames.Count == 0)
                {
                    columnNames = new List<string>();
                    for (int i = 1; i <= worksheet.UsedRange.Columns.Count; i++)
                    {
                        if (worksheet.Cells[1, i].Value != null)
                            columnNames.Add(worksheet.Cells[1, i].Value.ToString());
                    }
                }

                return columnNames;
            }
        }

        /// <summary>
        /// Current row number
        /// </summary>
        public int CurrentRow { get { return currentRow; } }

        /// <summary>
        /// Values of the current row
        /// </summary>
        public ExcelRowValues CurrentRowValues { get { return currentRowValues; } }

        #endregion

        #region Public methods

        /// <summary>
        /// Get cell value by row and column
        /// </summary>
        public object GetCellValue(int row, int col)
        {
            return worksheet.Cells[row, col].Value;
        }

        /// <summary>
        /// Current sheet column count
        /// </summary>
        public int ColumnCount { get { return worksheet.UsedRange.Columns.Count; } }

        /// <summary>
        /// Current sheet row count
        /// </summary>
        public int RowCount { get { return worksheet.UsedRange.Rows.Count; } }

        /// <summary>
        /// Read current row
        /// </summary>
        public bool ReadLine()
        {
            if (worksheet.UsedRange.Rows.Count > currentRow)
            {
                currentRowValues = null;
                var values = new Dictionary<string, string>();

                for (int i = 1; i <= worksheet.UsedRange.Columns.Count; i++)
                {
                    values.Add(ColumnNames[i - 1].ToUpper(), (worksheet.Cells[currentRow, i].Value ?? string.Empty).ToString());
                }

                // init new excel row values
                currentRowValues = new ExcelRowValues(values);
                // next row
                currentRow += 1;

                return true;
            }

            return false;
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            workbook.Close();
            workbook = null;
            excel.Quit();
            excel = null;
        }

        #endregion
    }

    public class ExcelRowValues
    {
        private Dictionary<string, string> values;

        public ExcelRowValues(Dictionary<string, string> values)
        {
            this.values = values;
        }

        public string this[string columnName]
        {
            get { return values.ContainsKey(columnName.ToUpper()) ? values[columnName.ToUpper()] : string.Empty; }
        }
    }
}
