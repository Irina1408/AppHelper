using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppView.View;

namespace AppView
{
    public class ViewManager : INotifyPropertyChanged
    {
        #region Private fields

        private string shownHeader;
        private UserControl shownControl;
        private List<IView> viewsHistory; 

        #endregion

        #region Initialization

        public ViewManager()
        {
            viewsHistory = new List<IView>();
        }

        #endregion

        #region Public events

        public EventHandler<ShownControlChangedEventArgs> ShownControlChanged;

        private void OnShownControlChanged()
        {
            if (ShownControlChanged != null)
            {
                ShownControlChanged(this, new ShownControlChangedEventArgs(ShownControl));
            }
        }

        #endregion

        #region Public properties

        public string ShownHeader
        {
            get { return shownHeader; }
            set
            {
                shownHeader = value;
                NotifyPropertyChanged();
            }
        }

        public UserControl ShownControl
        {
            get { return shownControl; }
            set
            {
                shownControl = value;
                NotifyPropertyChanged();
                OnShownControlChanged();
            }
        }

        #endregion

        #region Public methods

        public void ShowControl(UserControl control)
        {
            var defaultView = new DefaultView(control, string.Empty);
            ShowView(defaultView);
        }

        public void ShowView(IView view)
        {
            // show control and header
            ShownControl = view.ControlView;
            ShownHeader = view.Header;

            // add view to the history
            viewsHistory.Add(view);
        }

        public void ReturnToPreviousView()
        {
            IView currentView = viewsHistory.LastOrDefault();

            if (currentView != null)
            {
                // remove current view from history
                viewsHistory.Remove(currentView);
                // find previous view
                IView previousView = viewsHistory.LastOrDefault();

                if (previousView != null)
                {
                    // show previous control and header
                    ShownControl = previousView.ControlView;
                    ShownHeader = previousView.Header;
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class ShownControlChangedEventArgs : EventArgs
    {
        public ShownControlChangedEventArgs(Control control)
        {
            ShownControl = control;
        }

        public Control ShownControl { get; private set; }
    }
}
