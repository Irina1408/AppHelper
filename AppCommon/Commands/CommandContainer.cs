using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppCommon.Commands
{
    public class CommandContainer : INotifyPropertyChanged
    {
        #region Private fields

        private ICommand addCommand;
        private ICommand editCommand;
        private ICommand deleteCommand;
        private ICommand saveCommand;
        private ICommand cancelCommand;

        #endregion

        #region Public properties

        public ICommand AddCommand
        {
            get { return addCommand; }
            set
            {
                addCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand EditCommand
        {
            get { return editCommand; }
            set
            {
                editCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                deleteCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                saveCommand = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set
            {
                cancelCommand = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
