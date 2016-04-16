﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppCommon.Commands
{
    public class Command : ICommand
    {
        #region Private fields

        private Action execute;
        private Func<bool> canExecute;

        #endregion

        #region Initialization

        public Command(Action execute, Func<bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion

        #region Public methods

        public void ValidateCanExecute()
        {
            if (canExecute != null && CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        #region ICommand implementation

        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (execute != null) execute();
        }

        #endregion
    }
}
