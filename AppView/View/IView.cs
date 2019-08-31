using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using AppView.CommonInterfaces;

namespace AppView.View
{
    public interface IView : IHeader
    {
        UserControl ControlView { get; }
    }
}
