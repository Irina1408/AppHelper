using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppCommon.Commands
{
    public class DataGridSelectedItemCommand<T> : ICommand, IDisposable
         where T : class
    {
        #region Private fields

        private DataGrid dataGrid;
        private Action<T> execute;
        private Func<T, bool> canExecute;

        #endregion

        #region Initialization

        public DataGridSelectedItemCommand(DataGrid dataGrid, Action<T> execute, Func<T, bool> canExecute)
        {
            this.dataGrid = dataGrid;
            this.execute = execute;
            this.canExecute = canExecute;
            this.dataGrid.SelectionChanged += dataGrid_SelectionChanged;
        }

        private void dataGrid_SelectionChanged(object sender, EventArgs e)
        {
            ValidateCanExecute();
        }

        #endregion

        #region Public methods

        public void ValidateCanExecute()
        {
            if (canExecute != null && CanExecuteChanged != null) CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            if (dataGrid.SelectedItem == null)
                return false;
            if (canExecute != null)
                return canExecute(dataGrid.SelectedItem as T);
            else
                return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (execute != null && dataGrid.SelectedItem != null)
                execute(dataGrid.SelectedItem as T);
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            this.dataGrid.SelectionChanged -= dataGrid_SelectionChanged;
        }

        #endregion
    }
}
